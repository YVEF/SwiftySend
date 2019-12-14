using SwiftySend.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace SwiftySend.Core
{
    internal class MemberAccessFunctionCollection
    {
        
        private static IList<FieldInfo> _serElementField = typeof(SerializationNode).GetFields();      

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
                ilGenerator.Emit(OpCodes.Stfld, _serElementField[0]);

                ilGenerator.Emit(OpCodes.Ldloca_S, 2);
                ilGenerator.Emit(OpCodes.Ldloc_0);

                _CodeGenerationAccordingToMemberForMemberAccess(ilGenerator, memberInfoCollection[i]);

                ilGenerator.Emit(OpCodes.Stfld, _serElementField[1]);

                ilGenerator.Emit(OpCodes.Ldloc_2);
                ilGenerator.Emit(OpCodes.Stelem, typeof(SerializationNode));
            }

            ilGenerator.Emit(OpCodes.Ldloc_1);
            ilGenerator.Emit(OpCodes.Ret);

            return dynamicMethod.CreateDelegate(typeof(Func<TTarget, SerializationNode[]>));
        }



        public static Delegate MakeFuncToObjectCreatingAndFilling<TTarget>(IList<MemberInfoExtended> memberInfoCollection)
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
                
                _CodeGenerationAccordingToMemberForObjectCreating(ilGenerator, memberInfoCollection[i]);
            }


            ilGenerator.Emit(OpCodes.Ldloc_0);
            ilGenerator.Emit(OpCodes.Ret);

            return dynamicMethod.CreateDelegate(typeof(Func<SerializationNode[], TTarget>));
        }


        public static Delegate MakeActionToMemberAssigning<TObject, TMember>(MemberInfoExtended memberInfoExtended)
        {
            var dynamicMethod = new DynamicMethod("MemberAssigning", typeof(void), new Type[] { typeof(object), typeof(object) }, typeof(TObject));
            var ilGenerator = dynamicMethod.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Castclass, typeof(TObject));

            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Castclass, typeof(TMember));

            _AssignToMember(ilGenerator, memberInfoExtended);            

            ilGenerator.Emit(OpCodes.Ret);

            return dynamicMethod.CreateDelegate(typeof(Action<object, object>));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void _AssignToMember(ILGenerator ilGenerator, MemberInfoExtended memberInfoExtended)
        {
            if (memberInfoExtended.MemberInfo.MemberType == MemberTypes.Property)
                ilGenerator.Emit(OpCodes.Callvirt, ((PropertyInfo)memberInfoExtended.MemberInfo).SetMethod);
            else
                ilGenerator.Emit(OpCodes.Stfld, (FieldInfo)memberInfoExtended.MemberInfo);
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
                ilGenerator.Emit(OpCodes.Callvirt, SharedFunctionsAggregator.Converters.__ToString);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void _CodeGenerationAccordingToMemberForObjectCreating(ILGenerator ilGenerator, MemberInfoExtended memberInfoExtended)
        {
            if(!memberInfoExtended.IsSimpleType && !memberInfoExtended.IsCollection)
                ilGenerator.Emit(OpCodes.Newobj, memberInfoExtended.Type.GetConstructors()[0]);

            else if (memberInfoExtended.Type.IsEnum)
                _EnumHandling(ilGenerator, memberInfoExtended);

            else if (memberInfoExtended.Type.IsPrimitive || memberInfoExtended.Type == typeof(decimal))
                _PrimitiveHandling(ilGenerator, memberInfoExtended);

            else if (memberInfoExtended.Type == typeof(DateTime))
                _DateTimeHandling(ilGenerator);

            else
                _LeftTypesHandling(ilGenerator, memberInfoExtended);

            _AssignToMember(ilGenerator, memberInfoExtended);          
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void _EnumHandling(ILGenerator ilGenerator, MemberInfoExtended memberInfoExtended)
        {
            ilGenerator.Emit(OpCodes.Ldtoken, memberInfoExtended.Type);
            ilGenerator.Emit(OpCodes.Call, SharedFunctionsAggregator.Reflections.__GetTypeFromHanlde);
            ilGenerator.Emit(OpCodes.Ldloca_S, 2);
            ilGenerator.Emit(OpCodes.Ldfld, _serElementField[1]);
            ilGenerator.Emit(OpCodes.Callvirt, SharedFunctionsAggregator.Converters.__ToString);

            ilGenerator.Emit(OpCodes.Call, SharedFunctionsAggregator.Converters.__EnumParse);
            ilGenerator.Emit(OpCodes.Unbox_Any, memberInfoExtended.Type);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void _PrimitiveHandling(ILGenerator ilGenerator, MemberInfoExtended memberInfoExtended)
        {
            ilGenerator.Emit(OpCodes.Ldloca_S, 2);
            ilGenerator.Emit(OpCodes.Ldfld, _serElementField[1]);
            ilGenerator.Emit(OpCodes.Callvirt, SharedFunctionsAggregator.Converters.__ToString);

            // frequently-used
            if (memberInfoExtended.Type == typeof(int))
                ilGenerator.Emit(OpCodes.Call, SharedFunctionsAggregator.Converters.__ToInt32);
            else if (memberInfoExtended.Type == typeof(bool))
                ilGenerator.Emit(OpCodes.Call, SharedFunctionsAggregator.Converters.__ToBoolean);
            else if (memberInfoExtended.Type == typeof(char))
                ilGenerator.Emit(OpCodes.Call, SharedFunctionsAggregator.Converters.__ToChar);
            else if (memberInfoExtended.Type == typeof(double))
                ilGenerator.Emit(OpCodes.Call, SharedFunctionsAggregator.Converters.__ToDouble);
            else if (memberInfoExtended.Type == typeof(byte))
                ilGenerator.Emit(OpCodes.Call, SharedFunctionsAggregator.Converters.__ToByte);
            else if (memberInfoExtended.Type == typeof(long))
                ilGenerator.Emit(OpCodes.Call, SharedFunctionsAggregator.Converters.__ToInt64);
            else if (memberInfoExtended.Type == typeof(decimal))
                ilGenerator.Emit(OpCodes.Call, SharedFunctionsAggregator.Converters.__ToDecimal);

            // rarely-used
            else if (memberInfoExtended.Type == typeof(float))
                ilGenerator.Emit(OpCodes.Call, SharedFunctionsAggregator.Converters.__ToSingle);
            else if (memberInfoExtended.Type == typeof(short))
                ilGenerator.Emit(OpCodes.Call, SharedFunctionsAggregator.Converters.__ToInt16);
            else if (memberInfoExtended.Type == typeof(uint))
                ilGenerator.Emit(OpCodes.Call, SharedFunctionsAggregator.Converters.__ToUInt32);
            else if (memberInfoExtended.Type == typeof(ulong))
                ilGenerator.Emit(OpCodes.Call, SharedFunctionsAggregator.Converters.__ToUInt64);
            else if (memberInfoExtended.Type == typeof(ushort))
                ilGenerator.Emit(OpCodes.Call, SharedFunctionsAggregator.Converters.__ToUInt16);
            else if (memberInfoExtended.Type == typeof(sbyte))
                ilGenerator.Emit(OpCodes.Call, SharedFunctionsAggregator.Converters.__ToSByte);
            
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void _DateTimeHandling(ILGenerator ilGenerator)
        {
            ilGenerator.Emit(OpCodes.Ldloca_S, 2);
            ilGenerator.Emit(OpCodes.Ldfld, _serElementField[1]);
            ilGenerator.Emit(OpCodes.Callvirt, SharedFunctionsAggregator.Converters.__ToString);

            ilGenerator.Emit(OpCodes.Call, SharedFunctionsAggregator.Converters.__ToDateTime);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void _LeftTypesHandling(ILGenerator ilGenerator, MemberInfoExtended memberInfoExtended)
        {
            ilGenerator.Emit(OpCodes.Ldloca_S, 2);
            ilGenerator.Emit(OpCodes.Ldfld, _serElementField[1]);
            ilGenerator.Emit(OpCodes.Callvirt, SharedFunctionsAggregator.Converters.__ToString);
        }
    }
}
