namespace MK.Majala.Core
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// The Jvm class represents a loaded Java Virtual Machine.
    /// </summary>
    public class Jvm : IDisposable
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate JNIResult JNI_CreateJavaVM(out IntPtr pvm, out IntPtr penv, JavaVMInitArgs* args);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate JNIResult JNI_GetCreatedJavaVMs(out IntPtr pvm, int size, [Out] out int size2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate JNIResult JNI_GetDefaultJavaVMInitArgs(JavaVMInitArgs* args);

        private IntPtr jvm;
        private JNI_CreateJavaVM createJavaVm;
        private JNI_GetCreatedJavaVMs getCreatedJavaVms;
        private JNI_GetDefaultJavaVMInitArgs getDefaultJavaVMInitArgs;
        private string version;


        /// <summary>
        /// Initializes a new instance of the <see cref="Jvm" /> class.
        /// The contructor takes the name of the directory containing the jvm and
        /// loads the neccessary DLLs from this installation directory.
        /// </summary>
        /// <param name="directory">Base directory of the Java Virtual Machine (that is the installation directory of the JRE or JDK).</param>
        public Jvm(string directory)
        {
            // Code siehe http://jni4net.googlecode.com/svn/trunk/jni4net.n/src/jni/JNI.cs
            // siehe http://jni4net.sourceforge.net/
            if ((this.jvm = NativeMethods.LoadLibrary(directory + @"\jvm.dll")) == IntPtr.Zero)
                throw new MajalaException();

            IntPtr function;

            if ((function = NativeMethods.GetProcAddress(this.jvm, "JNI_CreateJavaVM")) == IntPtr.Zero)
                throw new MajalaException();
            else
                this.createJavaVm = (JNI_CreateJavaVM)Marshal.GetDelegateForFunctionPointer(function, typeof(JNI_CreateJavaVM));

            if ((function = NativeMethods.GetProcAddress(this.jvm, "JNI_GetCreatedJavaVMs")) == IntPtr.Zero)
                throw new MajalaException();
            else
                this.getCreatedJavaVms = (JNI_GetCreatedJavaVMs)Marshal.GetDelegateForFunctionPointer(function, typeof(JNI_GetCreatedJavaVMs));

            if ((function = NativeMethods.GetProcAddress(this.jvm, "JNI_GetDefaultJavaVMInitArgs")) == IntPtr.Zero)
                throw new MajalaException();
            else
                this.getDefaultJavaVMInitArgs = (JNI_GetDefaultJavaVMInitArgs)Marshal.GetDelegateForFunctionPointer(function, typeof(JNI_GetDefaultJavaVMInitArgs));

            //// TODO determine version 
            this.version = string.Empty;
        }

        /// <summary>
        /// Gets the version of the loaded JVM.
        /// </summary>
        public string Version
        {
            get { return this.version; }
        }

        /// <summary>
        /// Loads and invokes the main method.
        /// </summary>
        /// <param name="method">Full qualified name of the main method.</param>
        public void InvokeMain(string method)
        {
        }

        /// <summary>
        /// Loads the method named initiateExit(int) and invokes this method
        /// if the method is found. If the method is not found an exception
        /// is thrown.
        /// </summary>
        /// <param name="reasonCode">User supplied reason code that will be passed thru the initiateExit method.</param>
        public void InvokeInitiateExit(int reasonCode)
        {
        }
    }
}
