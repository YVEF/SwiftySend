using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace SwiftySend.Core
{
    internal class MemberAccessFunctionCollection
    {
        private static MethodInfo _toString = typeof(object).GetMethod("ToString");
        public static IList<FieldInfo> serElementField = typeof(SerializationNode).GetFields();

        

        public static Delegate MakeFuncToMemberAccess<TTarget>(IList<MemberInfoExtended> memberInfoCollection)
        {
            DynamicMethod dynamicMethod = new DynamicMethod("MemberAccess", typeof(SerializationNode[]), new Type[] { typeof(TTarget) }, typeof(TTarget));
            var ilGenerator = dynamicMethod.GetILGenerator();

            ilGenerator.DeclareLocal(typeof(TTarget));
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


                _CodeGenerationAccordingToMemberForMemberAccess(ilGenerator, memberInfoCollection[i]);

                ilGenerator.Emit(OpCodes.Stfld, serElementField[1]);


                ilGenerator.Emit(OpCodes.Ldloc_2);
                ilGenerator.Emit(OpCodes.Stelem, typeof(SerializationNode));
            }

            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Ret);

            return dynamicMethod.CreateDelegate(typeof(Func<TTarget, SerializationNode[]>));
        }



        public static Func<SerializationNode[], TTarget> MakeFuncToObjectCreatingAndFilling<TTarget>(IList<MemberInfoExtended> memberInfoCollection)
        {
            DynamicMethod dynamicMethod = new DynamicMethod("ObjectCreating", typeof(TTarget), new Type[] { typeof(SerializationNode[]) }, typeof(TTarget));

            var ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.DeclareLocal(typeof(TTarget));
            ilGenerator.DeclareLocal(typeof(SerializationNode[]));
            ilGenerator.DeclareLocal(typeof(SerializationNode));


            ilGenerator.Emit(OpCodes.Newobj, typeof(TTarget).GetConstructors()[0]);
            ilGenerator.Emit(OpCodes.Stloc_0);


            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Stloc_1);


            for (int i = 0; i < memberInfoCollection.Count; i++)
            {
                ilGenerator.Emit(OpCodes.Ldloc_1);
                ilGenerator.Emit(OpCodes.Ldc_I4, i);
                ilGenerator.Emit(OpCodes.Ldelem, typeof(SerializationNode));
                ilGenerator.Emit(OpCodes.Stloc_2);

                ilGenerator.Emit(OpCodes.Ldloc_0);

                ilGenerator.Emit(OpCodes.Ldloca_S, 2);
                ilGenerator.Emit(OpCodes.Ldfld, serElementField[1]);
                ilGenerator.Emit(OpCodes.Callvirt, _toString);
                _CodeGenerationAccordingToMemberForObjectCreating(ilGenerator, memberInfoCollection[i]);
            }


            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Ret);

            return (Func<SerializationNode[], TTarget>)dynamicMethod.CreateDelegate(typeof(Func<SerializationNode[], TTarget>));
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void _CodeGenerationAccordingToMemberForMemberAccess(ILGenerator ilGenerator, MemberInfoExtended memberInfoExtended)
        {
            if (memberInfoExtended.MemberInfo.MemberType == MemberTypes.Property)
                ilGenerator.Emit(OpCodes.Callvirt, ((PropertyInfo)memberInfoExtended.MemberInfo).GetMethod);
            else
                ilGenerator.Emit(OpCodes.Ldfld, (FieldInfo)memberInfoExtended.MemberInfo);

            if (memberInfoExtended.Type.IsValueType)
                ilGenerator.Emit(OpCodes.Box, memberInfoExtended.Type);

            if (memberInfoExtended.Type == typeof(DateTime))
                ilGenerator.Emit(OpCodes.Callvirt, _toString);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void _CodeGenerationAccordingToMemberForObjectCreating(ILGenerator ilGenerator, MemberInfoExtended memberInfoExtended)
        {
            if (memberInfoExtended.MemberInfo.MemberType == MemberTypes.Property)
                ilGenerator.Emit(OpCodes.Callvirt, ((PropertyInfo)memberInfoExtended.MemberInfo).SetMethod);
            else
                ilGenerator.Emit(OpCodes.Stfld, (FieldInfo)memberInfoExtended.MemberInfo);

            //if (memberInfoExtended.Type.IsValueType)
            //    ilGenerator.Emit(OpCodes.Box, memberInfoExtended.Type);

            //if (memberInfoExtended.Type == typeof(DateTime))
            //    ilGenerator.Emit(OpCodes.Callvirt, _toString);
        }
    }
}
