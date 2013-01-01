namespace MK.Majala.Core
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// This class wraps the function pointers to the JVM service functions.
    /// </summary>
    public unsafe class JavaVM
    {
        private readonly IntPtr jvm;
        private readonly DestroyJavaVM destroyJavaVM;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaVM" /> class.
        /// </summary>
        /// <param name="jvm">Pointer to the native JavaVM structure, returned from the native function JNI_CreateJavaVM.</param>
        public JavaVM(IntPtr jvm)
        {
            this.jvm = jvm;

            JNIInvokeInterface functions = *(*(JNIInvokeInterfacePtr*)jvm.ToPointer()).Functions;
            this.destroyJavaVM = (DestroyJavaVM)Marshal.GetDelegateForFunctionPointer(functions.DestroyJavaVM, typeof(DestroyJavaVM));
        }

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private unsafe delegate int DestroyJavaVM(IntPtr jvm);

        /// <summary>
        /// Destroys the current JVM.
        /// </summary>
        /// <returns>Returns JNI_OK on success; returns a suitable JNI error code (a negative number) on failure.</returns>
        public MK.Majala.Core.Jvm.JniResult Destroy()
        {
            return (MK.Majala.Core.Jvm.JniResult)this.destroyJavaVM.Invoke(this.jvm);
        }

        [StructLayout(LayoutKind.Sequential), NativeCppClass]
        private struct JNIInvokeInterface
        {
            public IntPtr Reserved0;
            public IntPtr Reserved1;
            public IntPtr Reserved2;

            public IntPtr DestroyJavaVM;
            public IntPtr AttachCurrentThread;
            public IntPtr DetachCurrentThread;
            public IntPtr GetEnv;
            public IntPtr AttachCurrentThreadAsDaemon;
        }

        [StructLayout(LayoutKind.Sequential, Size = 4), NativeCppClass]
        private struct JNIInvokeInterfacePtr
        {
            public JNIInvokeInterface* Functions;
        }
    }
}
