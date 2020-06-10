using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;

namespace DoDSamples.Samples
{
    public static class ReferenceHelpers
    {
        public static unsafe long GetAddress(object o)
        {
            TypedReference reference = __makeref(o);
            IntPtr ptr = **(IntPtr**)(&reference);
            var result = (long)ptr;

            return result;
        }

        public static unsafe IntPtr GetAddressPtr(object o)
        {
            var a1 = (long)AddressHelper.GetAddress(o);

            TypedReference reference = __makeref(o);
            IntPtr ptr = **(IntPtr**)(&reference);
            return ptr;
        }

        public static Action<object, Action<IntPtr>> GetPinnedPointerInit()
        {
            var dyn = new DynamicMethod("GetPinnedPtr", typeof(void), new[] { typeof(object), typeof(Action<IntPtr>) }, typeof(ReferenceHelpers).Module);
            var il = dyn.GetILGenerator();
            il.DeclareLocal(typeof(object), true);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Conv_I);
            il.Emit(OpCodes.Call, typeof(Action<IntPtr>).GetMethod("Invoke"));
            il.Emit(OpCodes.Ret);
            return (Action<object, Action<IntPtr>>)dyn.CreateDelegate(typeof(Action<object, Action<IntPtr>>));
        }
    }

    public static class AddressHelper
    {
        private static object mutualObject;
        private static ObjectReinterpreter reinterpreter;

        static AddressHelper()
        {
            AddressHelper.mutualObject = new object();
            AddressHelper.reinterpreter = new ObjectReinterpreter();
            AddressHelper.reinterpreter.AsObject = new ObjectWrapper();
        }

        public static IntPtr GetAddress(object obj)
        {
            lock (AddressHelper.mutualObject)
            {
                AddressHelper.reinterpreter.AsObject.Object = obj;
                IntPtr address = AddressHelper.reinterpreter.AsIntPtr.Value;
                AddressHelper.reinterpreter.AsObject.Object = null;
                return address;
            }
        }

        public static T GetInstance<T>(IntPtr address)
        {
            lock (AddressHelper.mutualObject)
            {
                AddressHelper.reinterpreter.AsIntPtr.Value = address;
                return (T)AddressHelper.reinterpreter.AsObject.Object;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct ObjectReinterpreter
        {
            [FieldOffset(0)] public ObjectWrapper AsObject;
            [FieldOffset(0)] public IntPtrWrapper AsIntPtr;
        }

        private class ObjectWrapper
        {
            public object Object;
        }

        private class IntPtrWrapper
        {
            public IntPtr Value;
        }
    }
}
