using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace SwiftySend.ConsoleRunner
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SerElement
    {
        public string Name;
        public object Value;
        public int Nesting;
    }

    public class Dummy
    {
        public string Value2 { get; set; }
        public string Value { get; set; }

        public Dummy2 Value3 { get; set; }
    }

    public class Dummy2
    {
        public string Dummy2Value { get; set; }
    }

 


    class Program
    {

        public static Func<T, string> __MakeFunc<T>(T @object)
        {
            if (typeof(T) == typeof(string))
                return (x) => $"{x.ToString()} Very good string";
            else
                return (x) => $"{x.ToString()} Don't worry about it";
        }

        public static IList<Func<Dummy, SerElement[]>> listOfFunc = new List<Func<Dummy, SerElement[]>>();
        public static IList<FieldInfo> serElementField = typeof(SerElement).GetFields();
        unsafe static void Main(string[] args)
        {
            var invoke = typeof(Func<,>).MakeGenericType(typeof(string), typeof(string)).GetMethod("Invoke");
            var method123 = typeof(Program).GetMethod("__MakeFunc").MakeGenericMethod(typeof(string));
            var func123 = method123.Invoke(null, new object[] { "hihi" });
            var result123 = invoke.Invoke(func123, new object[] { "hhhh" });

            Func<string, string> x = (s) => "hi";
            var y = x as Delegate;
            var z = y.Method.Invoke(new Program(), new object[] { "hello" });

            






            var d = new Dummy() { Value = "ValueP", Value2 = "Value2P", Value3 = new Dummy2() { Dummy2Value = "nestedValueP" } };

            var list = Analyze(typeof(Dummy));
            var method = typeof(Program).GetMethod("_MakeFunc", BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(typeof(Dummy));


            var resultFromMethod = method.Invoke(null, new object[] { list });
            var a = resultFromMethod as MulticastDelegate;

            var result = (SerElement[])a.DynamicInvoke(d);

            var aaa = result[2];
            var list2 = Analyze(aaa.Value.GetType());

            var method2 = typeof(Program).GetMethod("_MakeFunc", BindingFlags.Static | BindingFlags.NonPublic);//.MakeGenericMethod(aaa.Value.GetType());
            var resultFromMethod2 = method2.Invoke(null, new object[] { list2 });
            var a2 = resultFromMethod2 as MulticastDelegate;

            var result2 = (SerElement[])a2.DynamicInvoke(aaa.Value);

            Console.WriteLine("Hello World!");
        }

        public SerElement[] _T(Dummy @object)
        {
            var a = new SerElement[2];
            a[0] = new SerElement() { Name = "hello", Value = "value" };
            return a;
        }

        private static object _MakeFunc<TSource>(IList<PropertyInfoExtended> list)
        {
            DynamicMethod dynamicMethod = new DynamicMethod("sdf", typeof(SerElement[]), new Type[] { typeof(TSource) });
            var ilGenerator = dynamicMethod.GetILGenerator();

            ilGenerator.DeclareLocal(typeof(Dummy));
            ilGenerator.DeclareLocal(typeof(SerElement[]));
            ilGenerator.DeclareLocal(typeof(SerElement));

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Stloc_0);

            ilGenerator.Emit(OpCodes.Ldc_I4_S, list.Count);
            ilGenerator.Emit(OpCodes.Newarr, typeof(SerElement));
            ilGenerator.Emit(OpCodes.Stloc_1);


            for (int i = 0; i < list.Count; i++)
            {
                ilGenerator.Emit(OpCodes.Ldloc_1);
                ilGenerator.Emit(OpCodes.Ldc_I4, i);

                ilGenerator.Emit(OpCodes.Ldloca_S, 2);
                ilGenerator.Emit(OpCodes.Newobj, typeof(SerElement));
                ilGenerator.Emit(OpCodes.Dup);
                //ilGenerator.Emit(OpCodes.Stloc_2);

                //ilGenerator.Emit(OpCodes.Ldloca_S, 2);
                //ilGenerator.Emit(OpCodes.Ldloc_2);
                ilGenerator.Emit(OpCodes.Ldstr, list[i].PropertyInfo.Name);
                ilGenerator.Emit(OpCodes.Stfld, serElementField[0]);
                ilGenerator.Emit(OpCodes.Dup);


                //ilGenerator.Emit(OpCodes.Ldloca_S, 2);
                //ilGenerator.Emit(OpCodes.Ldloc_2);
                ilGenerator.Emit(OpCodes.Ldloc_0);
                ilGenerator.Emit(OpCodes.Callvirt, list[i].PropertyInfo.GetGetMethod());
                ilGenerator.Emit(OpCodes.Stfld, serElementField[1]);

                ilGenerator.Emit(OpCodes.Dup);

                //ilGenerator.Emit(OpCodes.Ldloc_2);
                ilGenerator.Emit(OpCodes.Stelem, typeof(SerElement));
            }            

            ilGenerator.Emit(OpCodes.Ldloc_1);

            ilGenerator.Emit(OpCodes.Ret);




            return dynamicMethod.CreateDelegate(typeof(Func<TSource, SerElement[]>));

        }


        private static List<PropertyInfoExtended> Analyze(Type type)
        {
            var list = new List<PropertyInfoExtended>();
            foreach (var item in type.GetProperties())
            {
                if(item.PropertyType.IsPrimitive || item.PropertyType == typeof(string))
                    list.Add(new PropertyInfoExtended(item));
                else
                {
                    var prop = new PropertyInfoExtended(item);
                    prop.NestedProperty.AddRange(Analyze(item.PropertyType));
                    list.Add(prop);
                }
            }
            return list;
        }

        public class PropertyInfoExtended
        {
            public PropertyInfoExtended(PropertyInfo info)
            {
                PropertyInfo = info;
                NestedProperty = new List<PropertyInfoExtended>();
            }
            public PropertyInfo PropertyInfo;
            public List<PropertyInfoExtended> NestedProperty;
        }
    }
}
