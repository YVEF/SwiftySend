using System;
using System.Collections.Generic;
using System.Reflection;

namespace SwiftySend.Core
{
    public class MemberInfoExtended
    {
        public MemberInfoExtended(MemberInfo info, Type memberType, bool isCollection = false)
        {
            MemberInfo = info;
            NestedMembers = new List<MemberInfoExtended>();
            TypeInfo = new TypeInfoExtended(memberType, isCollection);
        }

        public TypeInfoExtended TypeInfo;
        public bool SimpleType => NestedMembers.Count == 0 && !TypeInfo.IsCollection;
        public bool SimpleCollection => NestedMembers.Count == 0 && TypeInfo.IsCollection;
        public MemberInfo MemberInfo;
        public List<MemberInfoExtended> NestedMembers;

    }

    public class TypeInfoExtended
    {
        public TypeInfoExtended(Type type, bool isCollection)
        {
            Type = type;
            IsCollection = isCollection;
            if (type.IsGenericType)
                GenericParameters = type.GenericTypeArguments;
            

        }
        public Type Type;
        public Type[] GenericParameters;
        public bool IsCollection;

        public bool HasGenericParameters => GenericParameters != null && GenericParameters.Length != 0;
    }
}
