using SwiftySend.Core;
using System;
using System.Collections.Generic;

namespace SwiftySend.Helpers
{
    internal class StructureAnalyzerHelper
    {
        public static IList<PropertyInfoExtended> AnalyzeAndPrepareSerializationStructure(Type targetType)
        {
            var list = new List<PropertyInfoExtended>();
            foreach (var item in targetType.GetProperties())
            {
                if (item.PropertyType.IsPrimitive || item.PropertyType == typeof(string))
                    list.Add(new PropertyInfoExtended(item));
                else
                {
                    var prop = new PropertyInfoExtended(item);
                    prop.NestedProperties.AddRange(AnalyzeAndPrepareSerializationStructure(item.PropertyType));
                    list.Add(prop);
                }
            }
            return list;
        }
    }
}
