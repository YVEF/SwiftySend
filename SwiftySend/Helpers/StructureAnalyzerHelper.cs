using SwiftySend.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SwiftySend.Helpers
{
    internal class StructureAnalyzerHelper
    {
        public static IList<MemberInfoExtended> AnalyzeAndPrepareSerializationStructure(Type targetType)
        {
            var memberCollection = new List<MemberInfoExtended>();

            foreach (var propertyInfo in targetType.GetProperties())
            {
                if (_IsSimpleType(propertyInfo.PropertyType))
                    memberCollection.Add(new MemberInfoExtended(propertyInfo, propertyInfo.PropertyType));
                else
                {
                    var memberInfo = new MemberInfoExtended(propertyInfo, propertyInfo.PropertyType);
                    memberInfo.NestedProperties.AddRange(AnalyzeAndPrepareSerializationStructure(propertyInfo.PropertyType));
                    memberCollection.Add(memberInfo);
                }
            }


            foreach (var fieldInfo in targetType.GetFields())
            {
                if (_IsSimpleType(fieldInfo.FieldType))
                    memberCollection.Add(new MemberInfoExtended(fieldInfo, fieldInfo.FieldType));
                else
                {
                    var memberInfo = new MemberInfoExtended(fieldInfo, fieldInfo.FieldType);
                    memberInfo.NestedProperties.AddRange(AnalyzeAndPrepareSerializationStructure(fieldInfo.FieldType));
                    memberCollection.Add(memberInfo);
                }
            }
            return memberCollection;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool _IsSimpleType(Type type) =>
            type.IsPrimitive || type == typeof(string)
            || type.IsValueType || type == typeof(object);
    }
}
