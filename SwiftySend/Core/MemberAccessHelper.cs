using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace SwiftySend.Core
{
    internal class MemberAccessHelper
    {
        public static IList<FieldInfo> serElementField = typeof(SerializationNode).GetFields();
        public static Delegate MakeFuncToMemberAccess<T>(IList<PropertyInfoExtended> list)
        {
            DynamicMethod dynamicMethod = new DynamicMethod("sdf", typeof(SerializationNode[]), new Type[] { typeof(T) });
            var ilGenerator = dynamicMethod.GetILGenerator();

            ilGenerator.DeclareLocal(typeof(T));
            ilGenerator.DeclareLocal(typeof(SerializationNode[]));
            ilGenerator.DeclareLocal(typeof(SerializationNode));

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Stloc_0);

            ilGenerator.Emit(OpCodes.Ldc_I4_S, list.Count);
            ilGenerator.Emit(OpCodes.Newarr, typeof(SerializationNode));
            ilGenerator.Emit(OpCodes.Stloc_1);


            for (int i = 0; i < list.Count; i++)
            {
                ilGenerator.Emit(OpCodes.Ldloc_1);
                ilGenerator.Emit(OpCodes.Ldc_I4, i);

                ilGenerator.Emit(OpCodes.Ldloca_S, 2);
                ilGenerator.Emit(OpCodes.Initobj, typeof(SerializationNode));

                ilGenerator.Emit(OpCodes.Ldloca_S, 2);
                ilGenerator.Emit(OpCodes.Ldstr, list[i].PropertyInfo.Name);
                ilGenerator.Emit(OpCodes.Stfld, serElementField[0]);


                ilGenerator.Emit(OpCodes.Ldloca_S, 2);
                ilGenerator.Emit(OpCodes.Ldloc_0);
                ilGenerator.Emit(OpCodes.Callvirt, list[i].PropertyInfo.GetGetMethod());
                ilGenerator.Emit(OpCodes.Stfld, serElementField[1]);

                ilGenerator.Emit(OpCodes.Ldloc_2);
                ilGenerator.Emit(OpCodes.Stelem, typeof(SerializationNode));
            }

            ilGenerator.Emit(OpCodes.Ldloc_1);

            ilGenerator.Emit(OpCodes.Ret);


            return dynamicMethod.CreateDelegate(typeof(Func<T, SerializationNode[]>));

        }
    }
}
