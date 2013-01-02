namespace MK.Majala.Core
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using MK.Majala.Core.Properties;

    /// <summary>
    /// The Jvm class represents a loaded Java Virtual Machine.
    /// </summary>
    public class Jvm : IDisposable
    {
        private JavaVM jvm;
        private JniEnv env;
        private IntPtr jvmDll;
        private JNI_CreateJavaVM createJavaVm;
        private JNI_GetCreatedJavaVMs getCreatedJavaVms;
        private JNI_GetDefaultJavaVMInitArgs getDefaultJavaVMInitArgs;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Jvm" /> class.
        /// The contructor takes the name of the directory containing the JVM and
        /// loads the neccessary DLLs from this installation directory.
        /// </summary>
        /// <param name="directory">Base directory of the Java Virtual Machine (that is the installation directory of the JRE or JDK).</param>
        public Jvm(string directory)
        {
            Logger.Log(string.Format(CultureInfo.CurrentCulture, Resources.LogLoadJvm, directory));

            // see http://jni4net.googlecode.com/svn/trunk/jni4net.n/src/jni/JNI.cs
            // see http://jni4net.sourceforge.net/
            string dllName = directory + @"\jvm.dll";
            if ((this.jvmDll = NativeMethods.LoadLibrary(dllName)) == IntPtr.Zero)
                throw new MajalaException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorLoadingLibrary, dllName));

            IntPtr function;

            if ((function = NativeMethods.GetProcAddress(this.jvmDll, "JNI_CreateJavaVM")) == IntPtr.Zero)
                throw new MajalaException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorGettingProcAddress, dllName, "JNI_CreateJavaVM"));
            else
                this.createJavaVm = (JNI_CreateJavaVM)Marshal.GetDelegateForFunctionPointer(function, typeof(JNI_CreateJavaVM));

            if ((function = NativeMethods.GetProcAddress(this.jvmDll, "JNI_GetCreatedJavaVMs")) == IntPtr.Zero)
                throw new MajalaException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorGettingProcAddress, dllName, "JNI_GetCreatedJavaVMs"));
            else
                this.getCreatedJavaVms = (JNI_GetCreatedJavaVMs)Marshal.GetDelegateForFunctionPointer(function, typeof(JNI_GetCreatedJavaVMs));

            if ((function = NativeMethods.GetProcAddress(this.jvmDll, "JNI_GetDefaultJavaVMInitArgs")) == IntPtr.Zero)
                throw new MajalaException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorGettingProcAddress, dllName, "JNI_GetDefaultJavaVMInitArgs"));
            else
                this.getDefaultJavaVMInitArgs = (JNI_GetDefaultJavaVMInitArgs)Marshal.GetDelegateForFunctionPointer(function, typeof(JNI_GetDefaultJavaVMInitArgs));
        }

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private unsafe delegate JniResult JNI_CreateJavaVM(out IntPtr pvm, out IntPtr penv, JavaVMInitArgs* args);

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private delegate JniResult JNI_GetCreatedJavaVMs(out IntPtr pvm, int size, [Out] out int size2);

        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        private unsafe delegate JniResult JNI_GetDefaultJavaVMInitArgs(JavaVMInitArgs* args);

        /// <summary>
        /// Result-Codes for JNI calls).
        /// </summary>
        public enum JniResult
        {
            /// <summary>
            /// Function called successfully.
            /// </summary>
            Ok = 0,

            /// <summary>
            /// Unknown error.
            /// </summary>
            Error = -1,

            /// <summary>
            /// Thread detached from the VM.
            /// </summary>
            Detached = -2,

            /// <summary>
            /// JNI Version error.
            /// </summary>
            Version = -3,

            /// <summary>
            /// Not enough memory.
            /// </summary>
            NoMem = -4,

            /// <summary>
            /// VM has been already created.
            /// </summary>
            Exist = -5,

            /// <summary>
            /// Invalid arguments
            /// </summary>
            Inval = -6,
        }

        private enum JNIVersion
        {
            /// <summary>
            /// JNI Version 1.1.
            /// </summary>
            JNI_VERSION_1_1 = 0x00010001,

            /// <summary>
            /// JNI Version 1.2.
            /// </summary>
            JNI_VERSION_1_2 = 0x00010002,

            /// <summary>
            /// JNI Version 1.4.
            /// </summary>
            JNI_VERSION_1_4 = 0x00010004,

            /// <summary>
            /// JNI Version 1.6.
            /// </summary>
            JNI_VERSION_1_6 = 0x00010006,
        }

        /// <summary>
        /// Loads the specified class and invokes the main method.
        /// </summary>
        /// <param name="className">Full qualified name of the class containing the main method.</param>
        public void InvokeMain(string className)
        {
            this.CreateJavaVirtualMachine(this.CreateInitializationArguments());

            IntPtr javaClass = this.FindClass(className);
            IntPtr javaMethod = this.GetStaticMethodID(javaClass, "main", "([Ljava/lang/String;)V");

            //// TODO: create argument array and pass it to CallStaticVoidMethod()...
            this.CallStaticVoidMethod(javaClass, javaMethod, null);
            this.DestroyJavaVirtualMachine();
        }

        /// <summary>
        /// Loads the method named initiateExit(int) and invokes this method
        /// if the method is found. If the method is not found an exception
        /// is thrown.
        /// </summary>
        /// <param name="reasonCode">User supplied reason code that will be passed to the initiateExit method.</param>
        public void InvokeInitiateExit(int reasonCode)
        {
            //// TODO: Invoke the InitiateExit method
        }

        /// <summary>
        /// Dispose this object and tell the garbage collector not to call the finalize method.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes this object.
        /// </summary>
        /// <param name="disposing">true if managed and unnamagend resources should be freed. false if only unmanages resources should be freed.</param>
        private void Dispose(bool disposing)
        {
            if (this.isDisposed)
                return;
            else
                this.isDisposed = true;

            if (disposing)
            {
                //// TODO dispose managed resources here...
            }

            if (this.jvmDll != null)
                NativeMethods.FreeLibrary(this.jvmDll);
        }

        /// <summary>
        /// Creates a JVM.
        /// </summary>
        /// <param name="initArgs">Initialization arguments for the JVM.</param>
        private unsafe void CreateJavaVirtualMachine(JavaVMInitArgs initArgs)
        {
            Logger.Log(Resources.LogCreateJavaVirtualMachine);

            IntPtr env;
            IntPtr jvm;

            JniResult result = this.createJavaVm(out jvm, out env, &initArgs);
            if (result != JniResult.Ok)
                throw new MajalaException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorCreatingJVM, result));

            this.env = new JniEnv(env);
            this.jvm = new JavaVM(jvm);
        }

        /// <summary>
        /// Destroys a previously created JVM.
        /// </summary>
        private unsafe void DestroyJavaVirtualMachine()
        {
            Logger.Log(Resources.LogDestroyJavaVirtualMachine);

            JniResult result = this.jvm.Destroy();
            if (result != JniResult.Ok)
                throw new MajalaException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorDestroyingJVM, result));
        }

        /// <summary>
        /// Sets up all required JVM initialization arguments.
        /// </summary>
        /// <returns>A filled JavaVMInitArgs that can be used to create a JVM.</returns>
        private JavaVMInitArgs CreateInitializationArguments()
        {
            Logger.Log(Resources.LogCreateInitializationArguments);

            JavaVMInitArgs args = new JavaVMInitArgs();

            args.Version = (int)JNIVersion.JNI_VERSION_1_4;
            args.IgnoreUnrecognized = 0x01;
            args.OptionCount = 0;

            //// TODO: set OptionCount and Options!

            //// args.Options = 0;

            return args;
        }

        /// <summary>
        /// Looks up the given class.
        /// </summary>
        /// <param name="className">Name of the class to be looked up.</param>
        /// <returns>A pointer to the class.</returns>
        private IntPtr FindClass(string className)
        {
            Logger.Log(string.Format(CultureInfo.CurrentCulture, Resources.LogFindClass, className));
            return this.env.FindClass(className);
        }

        /// <summary>
        /// Gets the given method.
        /// </summary>
        /// <param name="javaClass">Class containing the method.</param>
        /// <param name="methodName">Name of the class to be looked up.</param>
        /// <param name="methodSignature">Signarure of the method.</param>
        /// <returns>A pointer to the static method.</returns>
        private IntPtr GetStaticMethodID(IntPtr javaClass, string methodName, string methodSignature)
        {
            Logger.Log(string.Format(CultureInfo.CurrentCulture, Resources.LogGetStaticMethodID, methodName, methodSignature));
            return this.env.GetStaticMethodId(javaClass, methodName, methodSignature);
        }

        /// <summary>
        /// Class the given method.
        /// </summary>
        /// <param name="javaClass">Class containing the method.</param>
        /// <param name="javaMethod">Method to be called.</param>
        /// <param name="arguments">Command line arguments.</param>
        private void CallStaticVoidMethod(IntPtr javaClass, IntPtr javaMethod, IntPtr[] arguments)
        {
            Logger.Log(Resources.LogCallStaticVoidMethod);
            this.env.CallStaticVoidMethod(javaClass, javaMethod, arguments);
        }

        [StructLayout(LayoutKind.Sequential), NativeCppClass]
        private unsafe struct JavaVMInitArgs
        {
            public int Version;
            public int OptionCount;
            public JavaVMOption* Options;
            public byte IgnoreUnrecognized;
        }

        [StructLayout(LayoutKind.Sequential), NativeCppClass]
        private struct JavaVMOption
        {
            public IntPtr OptionString;
            public IntPtr ExtraInfo;
        }
    }
}
