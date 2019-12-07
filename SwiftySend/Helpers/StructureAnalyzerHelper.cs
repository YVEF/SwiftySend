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


            foreach(var memberInfo in targetType.GetMembers(_BindingFlags))
            {
                if(memberInfo.MemberType == MemberTypes.Property && memberInfo is PropertyInfo propertyInfo)
                {
                    if (_IsSimpleType(propertyInfo.PropertyType))
                        memberCollection.Add(new MemberInfoExtended(propertyInfo, propertyInfo.PropertyType));

                    else if (_IsCollection(propertyInfo.PropertyType))
                    {
                        var memberInfoExtended = new MemberInfoExtended(propertyInfo, propertyInfo.PropertyType, isCollection: true);
                        if (propertyInfo.PropertyType.IsGenericType)
                        {
                            memberInfoExtended.GenericParameters = propertyInfo.PropertyType.GetGenericArguments();
                            foreach (var type in memberInfoExtended.GenericParameters)
                            {
                                if (_IsSimpleType(type))
                                    continue;
                                memberInfoExtended.NestedMembers.AddRange(AnalyzeAndPrepareSerializationStructure(type));
                            }
                        }
                        memberCollection.Add(memberInfoExtended);
                    }
                }
                else if(memberInfo.MemberType == MemberTypes.Field && memberInfo is FieldInfo fieldInfo)
                {
                    if (fieldInfo.Name.EndsWith(">k__BackingField"))
                        continue;

                    if (_IsSimpleType(fieldInfo.FieldType))
                        memberCollection.Add(new MemberInfoExtended(fieldInfo, fieldInfo.FieldType));
                    else
                    {
                        var memberInfoExtended = new MemberInfoExtended(fieldInfo, fieldInfo.FieldType);
                        memberInfoExtended.NestedMembers.AddRange(AnalyzeAndPrepareSerializationStructure(fieldInfo.FieldType));
                        memberCollection.Add(memberInfoExtended);
                    }
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
