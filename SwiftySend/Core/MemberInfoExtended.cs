using System;
using System.Collections.Generic;
using System.Reflection;

namespace SwiftySend.Core
{
    public class MemberInfoExtended
    {
        public MemberInfoExtended(MemberInfo info, Type memberType)
        {
            MemberInfo = info;
            NestedProperties = new List<MemberInfoExtended>();
            MemberType = memberType;
        }

        public Type MemberType;
        public bool SimpleType => NestedProperties.Count == 0;
        public MemberInfo MemberInfo;
        public List<MemberInfoExtended> NestedProperties;
    }
}
