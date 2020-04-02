using SwiftySend.Core;
using SwiftySend.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections;

namespace SwiftySend
{
    internal class SerializableStructureBuilder
    {
        private IList<MemberInfoExtended> _memberInfoCollection;

        private MethodInfo _memberAccessingInlinedMethod =
            typeof(MemberAccessFunctionCollection).GetMethod("MakeFuncToMemberAccess",
                BindingFlags.Static | BindingFlags.Public);

        private MethodInfo _objectCreatingInlinedMethod = 
            typeof(MemberAccessFunctionCollection).GetMethod("MakeFuncToObjectCreatingAndFilling",
            BindingFlags.Static | BindingFlags.Public);

        private MethodInfo _memberAssigningInlinedMethod =
            typeof(MemberAccessFunctionCollection).GetMethod("MakeActionToMemberAssigning",
            BindingFlags.Static | BindingFlags.Public);




        private Dictionary<Type, (MethodInfo, object)> _typeToMemberAccessFunction = new Dictionary<Type, (MethodInfo, object)>();
        private Dictionary<Type, (MethodInfo, object)> _typeToMemberAccessFunction2 = new Dictionary<Type, (MethodInfo, object)>();
        private Dictionary<Type, (MethodInfo, object)> _typeToMemberAssigningFunction = new Dictionary<Type, (MethodInfo, object)>();


        public SerializableStructureBuilder Build(Type targetType)
        {
            _memberInfoCollection = StructureAnalyzerHelper.AnalyzeAndPrepareSerializationStructure(targetType);
            PrepareMemberAccessFunctionsInternal(targetType, _memberInfoCollection);
            return this;
        }

        public IList<SerializationNode> ReorderObjectMembersIfNeeded(IList<SerializationNode> serializationNodes)
        {
            var nameSet = new Dictionary<string, int>();

            for (int i = 0; i < _memberInfoCollection.Count; i++)
                nameSet.Add(_memberInfoCollection[i].MemberInfo.Name, i);

            // [TODO]: change this behavior when attribute for node name handling will be introduced
            bool needReordering = false;
            for (int i = 0; i < serializationNodes.Count; i++)
            {
                if(nameSet[serializationNodes[i].Name] != i)
                {
                    needReordering = true;
                    break;
                }    
            }
            var result = serializationNodes;
            if (needReordering)
                result = serializationNodes.OrderBy(x => nameSet[x.Name]).ToList();

            return result;
        }

        public SerializationNode[] GenerateSerializableStructure(object @object) =>
            _GenerateSerializableStructureInternal(@object, _memberInfoCollection);


        public TObject GenerateObject<TObject>(SerializationNode[] serializationNodes) =>
            (TObject)_GenerateObjectInternal(typeof(TObject), serializationNodes, _memberInfoCollection);




        private object _GenerateObjectInternal(Type targetType, SerializationNode[] serializationNodes, IList<MemberInfoExtended> memberInfoExtendeds)
        {
            object @object = _ExecuteFunction(() => _typeToMemberAccessFunction2[targetType], serializationNodes);

            for (int i = 0; i < memberInfoExtendeds.Count; i++)
            {
                if (memberInfoExtendeds[i].IsSimpleType)
                    continue;

                //else if (memberInfoExtendeds[i].IsCollection)
                //{
                //    string nodeName = memberInfoExtendeds[i].HasGenericParameters ? memberInfoExtendeds[i].GenericParameters[0].Name : "object";

                //    if (memberInfoExtendeds[i].IsSimpleCollection)
                //        serializationNodes[i].NestedNodes = ((IEnumerable)serializationNodes[i].Value)
                //            .Select(x => new SerializationNode() { Value = x, Name = nodeName });

                //    else
                //        serializationNodes[i].NestedNodes = ((IEnumerable)serializationNodes[i].Value)
                //            .Select(x => new SerializationNode() { Name = nodeName, NestedNodes = _GenerateSerializableStructureInternal(x, memberInfoExtendeds[i].NestedMembers) });
                //}
                else
                {
                    object result = _GenerateObjectInternal(memberInfoExtendeds[i].Type, serializationNodes[i].NestedNodes.ToArray(), memberInfoExtendeds[i].NestedMembers);
                    _ExecuteFunction(() => _typeToMemberAssigningFunction[memberInfoExtendeds[i].Type], @object, result);
                }
            }

            return @object;
        }

        private void PrepareMemberAccessFunctionsInternal(Type targetType, IList<MemberInfoExtended> memberInfoExtended)
        {
            var memberAccessFunction = _memberAccessingInlinedMethod.MakeGenericMethod(targetType).Invoke(null, new object[] { memberInfoExtended });
            var instanceCreatingFunction = _objectCreatingInlinedMethod.MakeGenericMethod(targetType).Invoke(null, new object[] { memberInfoExtended });

            var memberAccessInvoker = typeof(Func<,>).MakeGenericType(targetType, typeof(SerializationNode[])).GetMethod("Invoke");
            var instanceCreateInvoker = typeof(Func<,>).MakeGenericType(typeof(SerializationNode[]), targetType).GetMethod("Invoke");            

            _typeToMemberAccessFunction.Add(targetType, (memberAccessInvoker, memberAccessFunction));
            _typeToMemberAccessFunction2.Add(targetType, (instanceCreateInvoker, instanceCreatingFunction));            

            for (int i = 0; i < memberInfoExtended.Count; i++)
            {
                if (memberInfoExtended[i].IsSimpleType || memberInfoExtended[i].IsSimpleCollection)
                    continue;                

                else if(memberInfoExtended[i].IsCollection)
                    PrepareMemberAccessFunctionsInternal(memberInfoExtended[i].GenericParameters[0], memberInfoExtended[i].NestedMembers);

                else
                    PrepareMemberAccessFunctionsInternal(memberInfoExtended[i].Type, memberInfoExtended[i].NestedMembers);

                var memberAssigningAction = _memberAssigningInlinedMethod.MakeGenericMethod(targetType, memberInfoExtended[i].Type)
                    .Invoke(null, new object[] { memberInfoExtended[i] });
                var memberAssigningInvoker = typeof(Action<object, object>).GetMethod("Invoke");
                _typeToMemberAssigningFunction.Add(memberInfoExtended[i].Type, (memberAssigningInvoker, memberAssigningAction));
            }
        }


        private SerializationNode[] _GenerateSerializableStructureInternal(object @object, IList<MemberInfoExtended> memberInfoExtendeds)
        {
            var serializationNodes = (SerializationNode[])_ExecuteFunction(() => _typeToMemberAccessFunction[@object.GetType()], @object);

            for (int i = 0; i < memberInfoExtendeds.Count; i++)
            {
                if (memberInfoExtendeds[i].IsSimpleType)
                    continue;

                else if (memberInfoExtendeds[i].IsCollection)
                {
                    string nodeName = memberInfoExtendeds[i].HasGenericParameters ? memberInfoExtendeds[i].GenericParameters[0].Name : "object";

                    if (memberInfoExtendeds[i].IsSimpleCollection)
                        serializationNodes[i].NestedNodes = ((IEnumerable)serializationNodes[i].Value)
                            .Select(x => new SerializationNode() { Value = x, Name = nodeName });

                    else
                        serializationNodes[i].NestedNodes = ((IEnumerable)serializationNodes[i].Value)
                            .Select(x => new SerializationNode() { Name = nodeName, NestedNodes = _GenerateSerializableStructureInternal(x, memberInfoExtendeds[i].NestedMembers) });
                }
                else
                    serializationNodes[i].NestedNodes = _GenerateSerializableStructureInternal(serializationNodes[i].Value, memberInfoExtendeds[i].NestedMembers);
            }
            return serializationNodes;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private object _ExecuteFunction(Func<(MethodInfo, object)> functionResolver, params object[] parameter)
        {
            try
            {
                var functionAggregator = functionResolver.Invoke();
                return functionAggregator.Item1.Invoke(functionAggregator.Item2, parameter);
            }
            catch (KeyNotFoundException)
            {
                throw new InvalidOperationException($"Could not found the definition of current type");
            }
            catch(Exception ex)
            {
                throw new Exception("Something wrong");
            }
        }
    }
}
