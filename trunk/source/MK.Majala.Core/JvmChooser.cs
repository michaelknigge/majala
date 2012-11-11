namespace MK.Majala.Core
{
    using System;
    using System.IO;
    using Microsoft.Win32;
    using MK.Majala.Core.Properties;

    /// <summary>
    /// The JvmChooser chooses and decides which installed (or provided) Java Version
    /// should be used.
    /// </summary>
    public static class JvmChooser
    {
        /// <summary>
        /// Looks up the most suitable Java Virtual Machine and loads it. If no suitable JVM can be found
        /// an MajalaExeption will be thrown. Note that the version (minVersion and maxVersion) of a JVM 
        /// loaded from a user supplied path will not be checked (it is assumed that a user supplied Java
        /// is shipped with the application and that the vendor will ship the correct JVM)!
        /// </summary>
        /// <param name="userPath">User provided path (from the configuration file) to the JVM (JAVA_HOME).</param>
        /// <param name="fallback">True if a system wide instaled JVM should be loaded if the user provided JVM can not be loaded.</param>
        /// <param name="minVersion">Minimum Java Version required (or an empty String).</param>
        /// <param name="maxVersion">Maximum Java Version allowed (or an empty String).</param>
        /// <returns>Jvm object with a suitable loaded JVM.</returns>
        public static Jvm LoadJvm(string userPath, bool fallback, string minVersion, string maxVersion)
        {
            if (IsValidJavaHome(userPath))
                return new Jvm(userPath);

            if (!fallback)
                throw new MajalaException(string.Format(Resources.DirectoryContainsNoJava, userPath));

            return LocateAndLoadInstalledJvm(minVersion, maxVersion);
        }

        /// <summary>
        /// Looks up the most suitable Java Virtual Machine installed on the local system and loads it.
        /// If no suitable JVM can be found an MajalaExeption will be thrown.
        /// </summary>
        /// <param name="minVersion">Minimum Java Version required (or an empty String).</param>
        /// <param name="maxVersion">Maximum Java Version allowed (or an empty String).</param>
        /// <returns>Jvm object with a suitable loaded JVM.</returns>
        public static Jvm LoadJvm(string minVersion, string maxVersion)
        {
            return new Jvm("dummy");
        }

        /// <summary>
        /// Locates the installed Java version and loads a JVM.  If no suitable JVM can be found
        /// a MajalaExeption will be thrown.
        /// </summary>
        /// <param name="minVersion">Minimum Java Version required (or an empty String).</param>
        /// <param name="maxVersion">Maximum Java Version allowed (or an empty String).</param>
        /// <returns>Jvm object with a suitable loaded JVM.</returns>
        private static Jvm LocateAndLoadInstalledJvm(string minVersion, string maxVersion)
        {
            const string rootKey = "HKEY_LOCAL_MACHINE";
            const string jdkKey = @"SOFTWARE\JavaSoft\Java Development Kit";
            const string jreKey = @"SOFTWARE\JavaSoft\Java Runtime Environment";

            // We will use the following strategy for locating a suitable JVM:
            //
            // 1. JDK's
            // 1.1  Current version
            // 1.2  All other versions
            // 2. JRE's
            // 2.1  Current version
            // 2.2  All other versions
            //
            // or....
            //
            // just collect all JVMs available, order them and load the newest one availabe that is suitable...
            string jdkCv = (string)Registry.GetValue(rootKey, jdkKey + @"\CurrentVersion", string.Empty);
            if (jdkCv.Length != 0)
            {
                string path = (string)Registry.GetValue(rootKey, jdkKey + jdkCv + "JavaHome", string.Empty);
            }

            string jreCv = (string)Registry.GetValue("HKEY_LOCAL_MACHINE", @"SOFTWARE\JavaSoft\Java Runtime Environment\CurrentVersion", string.Empty);

            //// TODO load JVM!

            int bits = SystemHelper.ProcessBitness();
            if (minVersion.Length == 0 && maxVersion.Length == 0)
                throw new MajalaException(string.Format(Resources.NoSuitableJavaFoundV1, bits));

            if (maxVersion.Length == 0)
                throw new MajalaException(string.Format(Resources.NoSuitableJavaFoundV2, bits, minVersion));

            throw new MajalaException(string.Format(Resources.NoSuitableJavaFoundV3, bits, minVersion, maxVersion));
        }

        /// <summary>
        /// Checks if the given directory is a valid java installation (JAVA_HOME) directory.
        /// </summary>
        /// <param name="directory">Name of an existing directory</param>
        /// <returns>True if the given directory is a valid JAVA_HOME, false if not.</returns>
        private static bool IsValidJavaHome(string directory)
        {
            // Checks for an installed JDK in JAVA_HOME (Client VM)...
            string jvmDll = Path.Combine(directory, @"jre\bin\client\jvm.dll");
            if (File.Exists(jvmDll))
                return true;

            // Checks for an installed JDK in JAVA_HOME (Server VM)...
            jvmDll = Path.Combine(directory, @"jre\bin\server\jvm.dll");
            if (File.Exists(jvmDll))
                return true;

            // Checks for an installed JRE in JAVA_HOME (Client VM)...
            jvmDll = Path.Combine(directory, @"bin\client\jvm.dll");
            if (File.Exists(jvmDll))
                return true;

            // Checks for an installed JRE in JAVA_HOME (Server VM)...
            jvmDll = Path.Combine(directory, @"bin\server\jvm.dll");
            if (File.Exists(jvmDll))
                return true;

            // No Java found...
            return false;
        }
    }
}
