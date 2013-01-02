namespace MK.Majala.Core
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using MK.Majala.Core.Properties;

    /// <summary>
    /// This class wraps JNI Interface Pointers.
    /// </summary>
    public unsafe class JniEnv
    {
        private IntPtr env;
        private FindClassDelegate findClass;
        private GetStaticMethodIdDelegate getStaticMethodId;
        private ExceptionCheckDelegate exceptionCheck;
        private CallStaticVoidMethodDelegate callStaticVoidMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="JniEnv" /> class.
        /// </summary>
        /// <param name="env">Pointer to the native JNINativeInterface structure, returned from the native function JNI_CreateJavaVM.</param>
        public JniEnv(IntPtr env)
        {
            this.env = env;

            JNINativeInterface functions = *(*(JNINativeInterfacePtr*)env.ToPointer()).Functions;

            this.findClass = (FindClassDelegate)Marshal.GetDelegateForFunctionPointer(functions.FindClass, typeof(FindClassDelegate));
            this.getStaticMethodId = (GetStaticMethodIdDelegate)Marshal.GetDelegateForFunctionPointer(functions.GetStaticMethodID, typeof(GetStaticMethodIdDelegate));
            this.exceptionCheck = (ExceptionCheckDelegate)Marshal.GetDelegateForFunctionPointer(functions.ExceptionCheck, typeof(ExceptionCheckDelegate));
            this.callStaticVoidMethod = (CallStaticVoidMethodDelegate)Marshal.GetDelegateForFunctionPointer(functions.CallStaticVoidMethod, typeof(CallStaticVoidMethodDelegate));
        }

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private unsafe delegate IntPtr FindClassDelegate(IntPtr env, [MarshalAs(UnmanagedType.LPStr)] string name);

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private unsafe delegate IntPtr GetStaticMethodIdDelegate(IntPtr env, IntPtr javaClass, [MarshalAs(UnmanagedType.LPStr)] string name, [MarshalAs(UnmanagedType.LPStr)] string signature);

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private unsafe delegate byte ExceptionCheckDelegate(IntPtr env);

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private unsafe delegate int CallStaticVoidMethodDelegate(IntPtr env, IntPtr jacaClass, IntPtr javaMethod, params IntPtr[] args);

        /// <summary>
        /// Finds the given class.
        /// </summary>
        /// <param name="name">Name of the class to be looked up (fully qualified).</param>
        /// <returns>A pointer to the class.</returns>
        public IntPtr FindClass(string name)
        {
            IntPtr result = this.findClass.Invoke(this.env, name);
            this.CheckForJavaException();

            return result;
        }

        /// <summary>
        /// Looks up the given static method in the supplied class.
        /// </summary>
        /// <param name="javaClass">Pointer to the class containing the method.</param>
        /// <param name="name">Name of the static method to be looked up.</param>
        /// <param name="signature">Signature of the method.</param>
        /// <returns>A pointer to the method.</returns>
        public IntPtr GetStaticMethodId(IntPtr javaClass, string name, string signature)
        {
            IntPtr result = this.getStaticMethodId.Invoke(this.env, javaClass, name, signature);
            this.CheckForJavaException();
            
            return result;
        }

        /// <summary>
        /// Class the given method.
        /// </summary>
        /// <param name="javaClass">Class containing the method.</param>
        /// <param name="javaMethod">Method to be called.</param>
        /// <param name="arguments">Command line arguments.</param>
        public void CallStaticVoidMethod(IntPtr javaClass, IntPtr javaMethod, IntPtr[] arguments)
        {
            this.callStaticVoidMethod.Invoke(this.env, javaClass, javaMethod, arguments);
            this.CheckForJavaException();
        }

        private unsafe void CheckForJavaException()
        {
            // exceptionCheck() returns a boolean value describing if an exception is pending or not.
            // On the next JNI call the exception will be thrown  - for this reason we call
            // exceptionCheck() twice...
            if (this.exceptionCheck(this.env) != 0)
            {
                Logger.Log(Resources.LogExceptionOccured);
                this.exceptionCheck(this.env);
            }
        }

        [StructLayout(LayoutKind.Sequential), NativeCppClass]
        private struct JNINativeInterface
        {
            public IntPtr Reserved0;
            public IntPtr Reserved1;
            public IntPtr Reserved2;
            public IntPtr Reserved3;
            public IntPtr GetVersion;
            public IntPtr DefineClass;
            public IntPtr FindClass;
            public IntPtr FromReflectedMethod;
            public IntPtr FromReflectedField;
            public IntPtr ToReflectedMethod;
            public IntPtr GetSuperclass;
            public IntPtr IsAssignableFrom;
            public IntPtr ToReflectedField;
            public IntPtr Throw;
            public IntPtr ThrowNew;
            public IntPtr ExceptionOccurred;
            public IntPtr ExceptionDescribe;
            public IntPtr ExceptionClear;
            public IntPtr FatalError;
            public IntPtr PushLocalFrame;
            public IntPtr PopLocalFrame;
            public IntPtr NewGlobalRef;
            public IntPtr DeleteGlobalRef;
            public IntPtr DeleteLocalRef;
            public IntPtr IsSameObject;
            public IntPtr NewLocalRef;
            public IntPtr EnsureLocalCapacity;
            public IntPtr AllocObject;
            public IntPtr NewObject;
            public IntPtr NewObjectV;
            public IntPtr NewObjectA;
            public IntPtr GetObjectClass;
            public IntPtr IsInstanceOf;
            public IntPtr GetMethodID;
            public IntPtr CallObjectMethod;
            public IntPtr CallObjectMethodV;
            public IntPtr CallObjectMethodA;
            public IntPtr CallBooleanMethod;
            public IntPtr CallBooleanMethodV;
            public IntPtr CallBooleanMethodA;
            public IntPtr CallByteMethod;
            public IntPtr CallByteMethodV;
            public IntPtr CallByteMethodA;
            public IntPtr CallCharMethod;
            public IntPtr CallCharMethodV;
            public IntPtr CallCharMethodA;
            public IntPtr CallShortMethod;
            public IntPtr CallShortMethodV;
            public IntPtr CallShortMethodA;
            public IntPtr CallIntMethod;
            public IntPtr CallIntMethodV;
            public IntPtr CallIntMethodA;
            public IntPtr CallLongMethod;
            public IntPtr CallLongMethodV;
            public IntPtr CallLongMethodA;
            public IntPtr CallFloatMethod;
            public IntPtr CallFloatMethodV;
            public IntPtr CallFloatMethodA;
            public IntPtr CallDoubleMethod;
            public IntPtr CallDoubleMethodV;
            public IntPtr CallDoubleMethodA;
            public IntPtr CallVoidMethod;
            public IntPtr CallVoidMethodV;
            public IntPtr CallVoidMethodA;
            public IntPtr CallNonvirtualObjectMethod;
            public IntPtr CallNonvirtualObjectMethodV;
            public IntPtr CallNonvirtualObjectMethodA;
            public IntPtr CallNonvirtualBooleanMethod;
            public IntPtr CallNonvirtualBooleanMethodV;
            public IntPtr CallNonvirtualBooleanMethodA;
            public IntPtr CallNonvirtualByteMethod;
            public IntPtr CallNonvirtualByteMethodV;
            public IntPtr CallNonvirtualByteMethodA;
            public IntPtr CallNonvirtualCharMethod;
            public IntPtr CallNonvirtualCharMethodV;
            public IntPtr CallNonvirtualCharMethodA;
            public IntPtr CallNonvirtualShortMethod;
            public IntPtr CallNonvirtualShortMethodV;
            public IntPtr CallNonvirtualShortMethodA;
            public IntPtr CallNonvirtualIntMethod;
            public IntPtr CallNonvirtualIntMethodV;
            public IntPtr CallNonvirtualIntMethodA;
            public IntPtr CallNonvirtualLongMethod;
            public IntPtr CallNonvirtualLongMethodV;
            public IntPtr CallNonvirtualLongMethodA;
            public IntPtr CallNonvirtualFloatMethod;
            public IntPtr CallNonvirtualFloatMethodV;
            public IntPtr CallNonvirtualFloatMethodA;
            public IntPtr CallNonvirtualDoubleMethod;
            public IntPtr CallNonvirtualDoubleMethodV;
            public IntPtr CallNonvirtualDoubleMethodA;
            public IntPtr CallNonvirtualVoidMethod;
            public IntPtr CallNonvirtualVoidMethodV;
            public IntPtr CallNonvirtualVoidMethodA;
            public IntPtr GetFieldID;
            public IntPtr GetObjectField;
            public IntPtr GetBooleanField;
            public IntPtr GetByteField;
            public IntPtr GetCharField;
            public IntPtr GetShortField;
            public IntPtr GetIntField;
            public IntPtr GetLongField;
            public IntPtr GetFloatField;
            public IntPtr GetDoubleField;
            public IntPtr SetObjectField;
            public IntPtr SetBooleanField;
            public IntPtr SetByteField;
            public IntPtr SetCharField;
            public IntPtr SetShortField;
            public IntPtr SetIntField;
            public IntPtr SetLongField;
            public IntPtr SetFloatField;
            public IntPtr SetDoubleField;
            public IntPtr GetStaticMethodID;
            public IntPtr CallStaticObjectMethod;
            public IntPtr CallStaticObjectMethodV;
            public IntPtr CallStaticObjectMethodA;
            public IntPtr CallStaticBooleanMethod;
            public IntPtr CallStaticBooleanMethodV;
            public IntPtr CallStaticBooleanMethodA;
            public IntPtr CallStaticByteMethod;
            public IntPtr CallStaticByteMethodV;
            public IntPtr CallStaticByteMethodA;
            public IntPtr CallStaticCharMethod;
            public IntPtr CallStaticCharMethodV;
            public IntPtr CallStaticCharMethodA;
            public IntPtr CallStaticShortMethod;
            public IntPtr CallStaticShortMethodV;
            public IntPtr CallStaticShortMethodA;
            public IntPtr CallStaticIntMethod;
            public IntPtr CallStaticIntMethodV;
            public IntPtr CallStaticIntMethodA;
            public IntPtr CallStaticLongMethod;
            public IntPtr CallStaticLongMethodV;
            public IntPtr CallStaticLongMethodA;
            public IntPtr CallStaticFloatMethod;
            public IntPtr CallStaticFloatMethodV;
            public IntPtr CallStaticFloatMethodA;
            public IntPtr CallStaticDoubleMethod;
            public IntPtr CallStaticDoubleMethodV;
            public IntPtr CallStaticDoubleMethodA;
            public IntPtr CallStaticVoidMethod;
            public IntPtr CallStaticVoidMethodV;
            public IntPtr CallStaticVoidMethodA;
            public IntPtr GetStaticFieldID;
            public IntPtr GetStaticObjectField;
            public IntPtr GetStaticBooleanField;
            public IntPtr GetStaticByteField;
            public IntPtr GetStaticCharField;
            public IntPtr GetStaticShortField;
            public IntPtr GetStaticIntField;
            public IntPtr GetStaticLongField;
            public IntPtr GetStaticFloatField;
            public IntPtr GetStaticDoubleField;
            public IntPtr SetStaticObjectField;
            public IntPtr SetStaticBooleanField;
            public IntPtr SetStaticByteField;
            public IntPtr SetStaticCharField;
            public IntPtr SetStaticShortField;
            public IntPtr SetStaticIntField;
            public IntPtr SetStaticLongField;
            public IntPtr SetStaticFloatField;
            public IntPtr SetStaticDoubleField;
            public IntPtr NewString;
            public IntPtr GetStringLength;
            public IntPtr GetStringChars;
            public IntPtr ReleaseStringChars;
            public IntPtr NewStringUTF;
            public IntPtr GetStringUTFLength;
            public IntPtr GetStringUTFChars;
            public IntPtr ReleaseStringUTFChars;
            public IntPtr GetArrayLength;
            public IntPtr NewObjectArray;
            public IntPtr GetObjectArrayElement;
            public IntPtr SetObjectArrayElement;
            public IntPtr NewBooleanArray;
            public IntPtr NewByteArray;
            public IntPtr NewCharArray;
            public IntPtr NewShortArray;
            public IntPtr NewIntArray;
            public IntPtr NewLongArray;
            public IntPtr NewFloatArray;
            public IntPtr NewDoubleArray;
            public IntPtr GetBooleanArrayElements;
            public IntPtr GetByteArrayElements;
            public IntPtr GetCharArrayElements;
            public IntPtr GetShortArrayElements;
            public IntPtr GetIntArrayElements;
            public IntPtr GetLongArrayElements;
            public IntPtr GetFloatArrayElements;
            public IntPtr GetDoubleArrayElements;
            public IntPtr ReleaseBooleanArrayElements;
            public IntPtr ReleaseByteArrayElements;
            public IntPtr ReleaseCharArrayElements;
            public IntPtr ReleaseShortArrayElements;
            public IntPtr ReleaseIntArrayElements;
            public IntPtr ReleaseLongArrayElements;
            public IntPtr ReleaseFloatArrayElements;
            public IntPtr ReleaseDoubleArrayElements;
            public IntPtr GetBooleanArrayRegion;
            public IntPtr GetByteArrayRegion;
            public IntPtr GetCharArrayRegion;
            public IntPtr GetShortArrayRegion;
            public IntPtr GetIntArrayRegion;
            public IntPtr GetLongArrayRegion;
            public IntPtr GetFloatArrayRegion;
            public IntPtr GetDoubleArrayRegion;
            public IntPtr SetBooleanArrayRegion;
            public IntPtr SetByteArrayRegion;
            public IntPtr SetCharArrayRegion;
            public IntPtr SetShortArrayRegion;
            public IntPtr SetIntArrayRegion;
            public IntPtr SetLongArrayRegion;
            public IntPtr SetFloatArrayRegion;
            public IntPtr SetDoubleArrayRegion;
            public IntPtr RegisterNatives;
            public IntPtr UnregisterNatives;
            public IntPtr MonitorEnter;
            public IntPtr MonitorExit;
            public IntPtr GetJavaVM;
            public IntPtr GetStringRegion;
            public IntPtr GetStringUTFRegion;
            public IntPtr GetPrimitiveArrayCritical;
            public IntPtr ReleasePrimitiveArrayCritical;
            public IntPtr GetStringCritical;
            public IntPtr ReleaseStringCritical;
            public IntPtr NewWeakGlobalRef;
            public IntPtr DeleteWeakGlobalRef;
            public IntPtr ExceptionCheck;
            public IntPtr NewDirectByteBuffer;
            public IntPtr GetDirectBufferAddress;
            public IntPtr GetDirectBufferCapacity;
        }

        [StructLayout(LayoutKind.Sequential, Size = 4), NativeCppClass]
        private struct JNINativeInterfacePtr
        {
            public JNINativeInterface* Functions;
        }
    }
}