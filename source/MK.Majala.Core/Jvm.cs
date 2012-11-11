namespace MK.Majala.Core
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// The Jvm class represents a loaded Java Virtual Machine.
    /// </summary>
    public class Jvm : IDisposable
    {
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
            // see http://jni4net.googlecode.com/svn/trunk/jni4net.n/src/jni/JNI.cs
            // see http://jni4net.sourceforge.net/
            if ((this.jvm = LoadLibrary(directory + @"\jvm.dll")) == IntPtr.Zero)
                throw new MajalaException();

            IntPtr function;

            if ((function = GetProcAddress(this.jvm, "JNI_CreateJavaVM")) == IntPtr.Zero)
                throw new MajalaException();
            else
                this.createJavaVm = (JNI_CreateJavaVM)Marshal.GetDelegateForFunctionPointer(function, typeof(JNI_CreateJavaVM));

            if ((function = GetProcAddress(this.jvm, "JNI_GetCreatedJavaVMs")) == IntPtr.Zero)
                throw new MajalaException();
            else
                this.getCreatedJavaVms = (JNI_GetCreatedJavaVMs)Marshal.GetDelegateForFunctionPointer(function, typeof(JNI_GetCreatedJavaVMs));

            if ((function = GetProcAddress(this.jvm, "JNI_GetDefaultJavaVMInitArgs")) == IntPtr.Zero)
                throw new MajalaException();
            else
                this.getDefaultJavaVMInitArgs = (JNI_GetDefaultJavaVMInitArgs)Marshal.GetDelegateForFunctionPointer(function, typeof(JNI_GetDefaultJavaVMInitArgs));

            //// TODO determine Version 
            this.version = string.Empty;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate JNIResult JNI_CreateJavaVM(out IntPtr pvm, out IntPtr penv, JavaVMInitArgs* args);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate JNIResult JNI_GetCreatedJavaVMs(out IntPtr pvm, int size, [Out] out int size2);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private unsafe delegate JNIResult JNI_GetDefaultJavaVMInitArgs(JavaVMInitArgs* args);

        private enum JNIResult
        {
            /// <summary>
            /// Function called successfully.
            /// </summary>
            JNI_OK = 0,

            /// <summary>
            /// Unknown error.
            /// </summary>
            JNI_ERR = (-1),

            /// <summary>
            /// Thread detached from the VM.
            /// </summary>
            JNI_EDETACHED = (-2),

            /// <summary>
            /// JNI Version error.
            /// </summary>
            JNI_EVERSION = (-3),

            /// <summary>
            /// Not enough memory.
            /// </summary>
            JNI_ENOMEM = (-4),

            /// <summary>
            /// VM has been already created.
            /// </summary>
            JNI_EEXIST = (-5),

            /// <summary>
            /// Invalid arguments
            /// </summary>
            JNI_EINVAL = (-6),
        }

        /// <summary>
        /// Gets the Version of the loaded JVM.
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

        /// <summary>
        /// Dispose this object and tell the garbage collector to not call the finalize method.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string fn);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr library, string function);

        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr library);

        /// <summary>
        /// Disposes this object.
        /// </summary>
        /// <param name="disposing">true if managed and unnamagend resources should be freed. false if only unmanages resources should be freed.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources here...
            }

            if (this.jvm != null)
                FreeLibrary(this.jvm);
        }

        [StructLayout(LayoutKind.Sequential), NativeCppClass]
        private unsafe struct JavaVMInitArgs
        {
            public int Version;
            public int OptionCount;
            public JavaVMOption* Options;
            public byte Unrecognized;
        }

        [StructLayout(LayoutKind.Sequential), NativeCppClass]
        private struct JavaVMOption
        {
            public IntPtr OptionString;
            public IntPtr ExtraInfo;
        }
    }
}
