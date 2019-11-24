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
        private IList<MemberInfoExtended> _propertyInfoExtendeds;
        private MethodInfo _makeFuncMethod =
            typeof(MemberAccessFunctionCollection).GetMethod("MakeFuncToMemberAccess",
                BindingFlags.Static | BindingFlags.Public);


        private Dictionary<Type, (MethodInfo, object)> _typeToMemberAccessFunction = new Dictionary<Type, (MethodInfo, object)>();


        public SerializableStructureBuilder Build(Type targetType)
        {
            _propertyInfoExtendeds = StructureAnalyzerHelper.AnalyzeAndPrepareSerializationStructure(targetType);
            PrepareMemberAccessFunctionsInternal(targetType, _propertyInfoExtendeds);
            return this;
        }
        

        public SerializationNode[] GenerateSerializableStructure(object @object) =>
            GenerateSerializableStructureInternal(@object, _propertyInfoExtendeds);


        private void PrepareMemberAccessFunctionsInternal(Type targetType, IList<MemberInfoExtended> propertyInfoExtendeds)
        {
            var memberAccessFunction = _makeFuncMethod.MakeGenericMethod(targetType).Invoke(null, new object[] { propertyInfoExtendeds });
            var memberAccessInvoker = typeof(Func<,>).MakeGenericType(targetType, typeof(SerializationNode[])).GetMethod("Invoke");
            _typeToMemberAccessFunction.Add(targetType, (memberAccessInvoker, memberAccessFunction));

            for (int i = 0; i < propertyInfoExtendeds.Count; i++)
            {
                if (propertyInfoExtendeds[i].IsSimpleType)
                {
                    continue;
                }
                else if(propertyInfoExtendeds[i].IsCollection)
                {
                    // TODO
                }
                else
                    PrepareMemberAccessFunctionsInternal(propertyInfoExtendeds[i].Type, propertyInfoExtendeds[i].NestedMembers);
                
            }
        }


        private SerializationNode[] GenerateSerializableStructureInternal(object @object, IList<MemberInfoExtended> propertyInfoExtendeds)
        {
            var serializationNodes = GenerateSerializableStructureInternal(@object);

            for (int i = 0; i < propertyInfoExtendeds.Count; i++)
            {
                if (propertyInfoExtendeds[i].IsSimpleType)
                {
                    continue;
                }
                else if (propertyInfoExtendeds[i].IsCollection)
                {
                    if(propertyInfoExtendeds[i].IsSimpleCollection)
                    {
                        string nodeName = propertyInfoExtendeds[i].HasGenericParameters ? propertyInfoExtendeds[i].GenericParameters[0].Name : "object";
                        serializationNodes[i].NestedNodes = ((IEnumerable)serializationNodes[i].Value)
                            .Select(x => new SerializationNode() { Value = x, Name = nodeName });                        
                    }                    
                }
                
                else
                    serializationNodes[i].NestedNodes = GenerateSerializableStructureInternal(serializationNodes[i].Value, propertyInfoExtendeds[i].NestedMembers);
            }
            return serializationNodes;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private SerializationNode[] GenerateSerializableStructureInternal(object @object)
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
