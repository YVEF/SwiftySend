using System.Collections.Generic;
using System.Reflection;

namespace SwiftySend.Core
{
    public class PropertyInfoExtended
    {
        public PropertyInfoExtended(PropertyInfo info/*, bool simplyType = true*/)
        {
            //SimpleType = simplyType;
            PropertyInfo = info;
            NestedProperties = new List<PropertyInfoExtended>();
        }

        public bool SimpleType => NestedProperties.Count == 0;
        public PropertyInfo PropertyInfo;
        public List<PropertyInfoExtended> NestedProperties;
    }
}
