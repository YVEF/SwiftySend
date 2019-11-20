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
                    var prop = new MemberInfoExtended(propertyInfo, propertyInfo.PropertyType);
                    prop.NestedProperties.AddRange(AnalyzeAndPrepareSerializationStructure(propertyInfo.PropertyType));
                    memberCollection.Add(prop);
                }
            }


            foreach (var fieldInfo in targetType.GetFields())
            {
                if (_IsSimpleType(fieldInfo.FieldType))
                    memberCollection.Add(new MemberInfoExtended(fieldInfo, fieldInfo.FieldType));
                else
                {
                    var prop = new MemberInfoExtended(fieldInfo, fieldInfo.FieldType);
                    prop.NestedProperties.AddRange(AnalyzeAndPrepareSerializationStructure(fieldInfo.FieldType));
                    memberCollection.Add(prop);
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
