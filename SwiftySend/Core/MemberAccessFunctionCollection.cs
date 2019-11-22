using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace SwiftySend.Core
{
    internal class MemberAccessFunctionCollection
    {
        private static MethodInfo toString = typeof(object).GetMethod("ToString");
        public static IList<FieldInfo> serElementField = typeof(SerializationNode).GetFields();

        

        public static Delegate MakeFuncToMemberAccess<T>(IList<MemberInfoExtended> memberInfoCollection)
        {
            DynamicMethod dynamicMethod = new DynamicMethod("MemberAccess", typeof(SerializationNode[]), new Type[] { typeof(T) }, typeof(T));
            var ilGenerator = dynamicMethod.GetILGenerator();

            ilGenerator.DeclareLocal(typeof(T));
            ilGenerator.DeclareLocal(typeof(SerializationNode[]));
            ilGenerator.DeclareLocal(typeof(SerializationNode));

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Stloc_0);

            ilGenerator.Emit(OpCodes.Ldc_I4_S, memberInfoCollection.Count);
            ilGenerator.Emit(OpCodes.Newarr, typeof(SerializationNode));
            ilGenerator.Emit(OpCodes.Stloc_1);


            for (int i = 0; i < memberInfoCollection.Count; i++)
            {
                ilGenerator.Emit(OpCodes.Ldloc_1);
                ilGenerator.Emit(OpCodes.Ldc_I4, i);

                ilGenerator.Emit(OpCodes.Ldloca_S, 2);
                ilGenerator.Emit(OpCodes.Initobj, typeof(SerializationNode));

                ilGenerator.Emit(OpCodes.Ldloca_S, 2);
                ilGenerator.Emit(OpCodes.Ldstr, memberInfoCollection[i].MemberInfo.Name);
                ilGenerator.Emit(OpCodes.Stfld, serElementField[0]);

                ilGenerator.Emit(OpCodes.Ldloca_S, 2);
                ilGenerator.Emit(OpCodes.Ldloc_0);


                _CodeGenerationAccordingToMember(ilGenerator, memberInfoCollection[i]);

                ilGenerator.Emit(OpCodes.Stfld, serElementField[1]);


                ilGenerator.Emit(OpCodes.Ldloc_2);
                ilGenerator.Emit(OpCodes.Stelem, typeof(SerializationNode));
            }

            ilGenerator.Emit(OpCodes.Ldloc_1);

            ilGenerator.Emit(OpCodes.Ret);


            return dynamicMethod.CreateDelegate(typeof(Func<T, SerializationNode[]>));
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void _CodeGenerationAccordingToMember(ILGenerator ilGenerator, MemberInfoExtended memberInfoExtended)
        {
            if (memberInfoExtended.MemberInfo.MemberType == MemberTypes.Property)
            {
                var property = (PropertyInfo)memberInfoExtended.MemberInfo;
                ilGenerator.Emit(OpCodes.Callvirt, property.GetMethod);
            }
            else
            {
                var field = (FieldInfo)memberInfoExtended.MemberInfo;
                ilGenerator.Emit(OpCodes.Ldfld, field);
            }

            if (memberInfoExtended.MemberType.IsValueType)
                ilGenerator.Emit(OpCodes.Box, memberInfoExtended.MemberType);

            if (memberInfoExtended.MemberType == typeof(DateTime))
                ilGenerator.Emit(OpCodes.Callvirt, toString);
        }
    }
}
