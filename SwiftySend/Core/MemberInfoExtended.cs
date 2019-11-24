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
            IsCollection = isCollection;
            Type = memberType;

        }


        public Type Type;
        public bool IsSimpleType => NestedMembers.Count == 0 && !IsCollection;
        public bool IsSimpleCollection => NestedMembers.Count == 0 && IsCollection;
        public MemberInfo MemberInfo;
        public List<MemberInfoExtended> NestedMembers;

        public bool IsCollection;
        public Type[] GenericParameters;
        public bool HasGenericParameters => GenericParameters != null && GenericParameters.Length != 0;

    }
}
