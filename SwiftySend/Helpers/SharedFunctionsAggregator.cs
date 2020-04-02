using System;
using System.Reflection;

namespace SwiftySend.Helpers
{
    internal static class SharedFunctionsAggregator
    {
        public static class Reflections
        {
            public static readonly MethodInfo __GetTypeFromHanlde = typeof(Type).GetMethod("GetTypeFromHandle", BindingFlags.Static | BindingFlags.Public);
        }

        public static class Converters
        {
            public static readonly MethodInfo __ToString = typeof(object).GetMethod("ToString");
            public static readonly MethodInfo __EnumParse = typeof(Enum).GetMethod("Parse", new Type[] { typeof(Type), typeof(string) });
            public static readonly MethodInfo __ToInt32 = typeof(Convert).GetMethod("ToInt32", new Type[] { typeof(object) });
            public static readonly MethodInfo __ToUInt32 = typeof(Convert).GetMethod("ToUInt32", new Type[] { typeof(object) });
            public static readonly MethodInfo __ToInt64 = typeof(Convert).GetMethod("ToInt64", new Type[] { typeof(object) });
            public static readonly MethodInfo __ToUInt64 = typeof(Convert).GetMethod("ToUInt64", new Type[] { typeof(object) });
            public static readonly MethodInfo __ToInt16 = typeof(Convert).GetMethod("ToInt16", new Type[] { typeof(object) });
            public static readonly MethodInfo __ToUInt16 = typeof(Convert).GetMethod("ToUInt16", new Type[] { typeof(object) });
            public static readonly MethodInfo __ToDouble = typeof(Convert).GetMethod("ToDouble", new Type[] { typeof(object) });
            public static readonly MethodInfo __ToSingle = typeof(Convert).GetMethod("ToSingle", new Type[] { typeof(object) });
            public static readonly MethodInfo __ToByte = typeof(Convert).GetMethod("ToByte", new Type[] { typeof(object) });
            public static readonly MethodInfo __ToSByte = typeof(Convert).GetMethod("ToSByte", new Type[] { typeof(object) });
            public static readonly MethodInfo __ToBoolean = typeof(Convert).GetMethod("ToBoolean", new Type[] { typeof(object) });
            public static readonly MethodInfo __ToDecimal = typeof(Convert).GetMethod("ToDecimal", new Type[] { typeof(object) });
            public static readonly MethodInfo __ToChar = typeof(Convert).GetMethod("ToChar", new Type[] { typeof(object) });
            public static readonly MethodInfo __ToDateTime = typeof(Convert).GetMethod("ToDateTime", new Type[] { typeof(object) });
        }
    }
}
