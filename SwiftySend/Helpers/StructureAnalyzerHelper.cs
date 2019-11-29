using SwiftySend.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SwiftySend.Helpers
{
    internal class StructureAnalyzerHelper
    {
        private static BindingFlags _BindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        public static IList<MemberInfoExtended> AnalyzeAndPrepareSerializationStructure(Type targetType)
        {
            var memberCollection = new List<MemberInfoExtended>();

            foreach (var propertyInfo in targetType.GetProperties(_BindingFlags))
            {
                if (_IsSimpleType(propertyInfo.PropertyType))
                    memberCollection.Add(new MemberInfoExtended(propertyInfo, propertyInfo.PropertyType));

                else if (_IsCollection(propertyInfo.PropertyType))
                {
                    var memberInfo = new MemberInfoExtended(propertyInfo, propertyInfo.PropertyType, isCollection: true);
                    if (propertyInfo.PropertyType.IsGenericType)
                    {
                        memberInfo.GenericParameters = propertyInfo.PropertyType.GetGenericArguments();
                        foreach (var type in memberInfo.GenericParameters)
                        {
                            if (_IsSimpleType(type))
                                continue;
                            memberInfo.NestedMembers.AddRange(AnalyzeAndPrepareSerializationStructure(type));
                        }                            
                    }
                    memberCollection.Add(memberInfo);
                }
                else
                {
                    var memberInfo = new MemberInfoExtended(propertyInfo, propertyInfo.PropertyType);
                    memberInfo.NestedMembers.AddRange(AnalyzeAndPrepareSerializationStructure(propertyInfo.PropertyType));
                    memberCollection.Add(memberInfo);
                }
            }


            foreach (var fieldInfo in targetType.GetFields(_BindingFlags))
            {
                if (fieldInfo.Name.EndsWith(">k__BackingField"))
                    continue;

                if (_IsSimpleType(fieldInfo.FieldType))
                    memberCollection.Add(new MemberInfoExtended(fieldInfo, fieldInfo.FieldType));
                else
                {
                    var memberInfo = new MemberInfoExtended(fieldInfo, fieldInfo.FieldType);
                    memberInfo.NestedMembers.AddRange(AnalyzeAndPrepareSerializationStructure(fieldInfo.FieldType));
                    memberCollection.Add(memberInfo);
                }
            }
            return memberCollection;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool _IsCollection(Type type) => typeof(IEnumerable).IsAssignableFrom(type);

            


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool _IsSimpleType(Type type) =>
            type.IsPrimitive || type == typeof(string)
            || type.IsValueType || type == typeof(object);
    }
}
