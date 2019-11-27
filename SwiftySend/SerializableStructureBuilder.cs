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
        //private Type _targetType;
        private IList<MemberInfoExtended> _memberInfoExtendeds;
        private MethodInfo _makeFuncMethod =
            typeof(MemberAccessFunctionCollection).GetMethod("MakeFuncToMemberAccess",
                BindingFlags.Static | BindingFlags.Public);

        private MethodInfo _makeFuncMethod2 = typeof(MemberAccessFunctionCollection).GetMethod("MakeFuncToObjectCreatingAndFilling",
            BindingFlags.Static | BindingFlags.Public);


        private Dictionary<Type, (MethodInfo, object)> _typeToMemberAccessFunction = new Dictionary<Type, (MethodInfo, object)>();
        private Dictionary<Type, (MethodInfo, object)> _typeToMemberAccessFunction2 = new Dictionary<Type, (MethodInfo, object)>();


        public SerializableStructureBuilder Build(Type targetType)
        {
            //_targetType = targetType;
            _memberInfoExtendeds = StructureAnalyzerHelper.AnalyzeAndPrepareSerializationStructure(targetType);
            PrepareMemberAccessFunctionsInternal(targetType, _memberInfoExtendeds);
            return this;
        }
        

        public SerializationNode[] GenerateSerializableStructure(object @object) =>
            _GenerateSerializableStructureInternal(@object, _memberInfoExtendeds);


        public TObject GenerateObject<TObject>(SerializationNode[] serializationNodes) =>
            (TObject)_GenerateObjectInternal(typeof(TObject), serializationNodes, _memberInfoExtendeds);


        private object _GenerateObjectInternal(Type targetType, SerializationNode[] serializationNodes, IList<MemberInfoExtended> memberInfoExtendeds)
        {
            object @object = null;
            foreach(var item in memberInfoExtendeds)
            {
                var func = _typeToMemberAccessFunction2[targetType];
                @object = func.Item1.Invoke(func.Item2, new object[] { serializationNodes });
            }
            return @object;
        }


        private void PrepareMemberAccessFunctionsInternal(Type targetType, IList<MemberInfoExtended> memberInfoExtended)
        {
            var memberAccessFunction = _makeFuncMethod.MakeGenericMethod(targetType).Invoke(null, new object[] { memberInfoExtended });
            var objectCreatingFunction = _makeFuncMethod2.MakeGenericMethod(targetType).Invoke(null, new object[] { memberInfoExtended });

            var memberAccessInvoker = typeof(Func<,>).MakeGenericType(targetType, typeof(SerializationNode[])).GetMethod("Invoke");
            var objectCreateInvoker = typeof(Func<,>).MakeGenericType(typeof(SerializationNode[]), targetType).GetMethod("Invoke");

            _typeToMemberAccessFunction.Add(targetType, (memberAccessInvoker, memberAccessFunction));
            _typeToMemberAccessFunction2.Add(targetType, (objectCreateInvoker, objectCreatingFunction));

            for (int i = 0; i < memberInfoExtended.Count; i++)
            {
                if (memberInfoExtended[i].IsSimpleType || memberInfoExtended[i].IsSimpleCollection)
                    continue;

                else if(memberInfoExtended[i].IsCollection)
                    PrepareMemberAccessFunctionsInternal(memberInfoExtended[i].GenericParameters[0], memberInfoExtended[i].NestedMembers);

                else
                    PrepareMemberAccessFunctionsInternal(memberInfoExtended[i].Type, memberInfoExtended[i].NestedMembers);
                
            }
        }


        private SerializationNode[] _GenerateSerializableStructureInternal(object @object, IList<MemberInfoExtended> memberInfoExtended)
        {
            var serializationNodes = _GenerateSerializableStructureInternal(@object);

            for (int i = 0; i < memberInfoExtended.Count; i++)
            {
                if (memberInfoExtended[i].IsSimpleType)
                {
                    continue;
                }
                else if (memberInfoExtended[i].IsCollection)
                {
                    string nodeName = memberInfoExtended[i].HasGenericParameters ? memberInfoExtended[i].GenericParameters[0].Name : "object";

                    if (memberInfoExtended[i].IsSimpleCollection)
                        serializationNodes[i].NestedNodes = ((IEnumerable)serializationNodes[i].Value)
                            .Select(x => new SerializationNode() { Value = x, Name = nodeName });

                    else
                        serializationNodes[i].NestedNodes = ((IEnumerable)serializationNodes[i].Value)
                            .Select(x => new SerializationNode() { Name = nodeName, NestedNodes = _GenerateSerializableStructureInternal(x, memberInfoExtended[i].NestedMembers) });
                }
                else
                    serializationNodes[i].NestedNodes = _GenerateSerializableStructureInternal(serializationNodes[i].Value, memberInfoExtended[i].NestedMembers);
            }
            return serializationNodes;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private SerializationNode[] _GenerateSerializableStructureInternal(object @object)
        {
            try
            {
                var memberAccessAggregator = _typeToMemberAccessFunction[@object.GetType()];
                return (SerializationNode[])memberAccessAggregator.Item1.Invoke(memberAccessAggregator.Item2, new object[] { @object });
            }
            catch(KeyNotFoundException)
            {
                throw new InvalidOperationException($"Could not found the definition of type: {@object.GetType()}");
            }
        }
    }
}
