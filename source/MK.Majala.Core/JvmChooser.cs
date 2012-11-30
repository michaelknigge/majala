namespace MK.Majala.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Microsoft.Win32;
    using MK.Majala.Core.Properties;

    /// <summary>
    /// The JvmChooser chooses which installed (or provided) Java Version
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
                throw new MajalaException(string.Format(CultureInfo.CurrentCulture, Resources.DirectoryContainsNoJava, userPath));

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
            return LocateAndLoadInstalledJvm(minVersion, maxVersion);
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
            SortedDictionary<string, string> virtualMachines = new SortedDictionary<string, string>(new JavaVersionComparer());
            AddVirtualMachinesFromRegistry(@"SOFTWARE\JavaSoft\Java Development Kit", virtualMachines);
            AddVirtualMachinesFromRegistry(@"SOFTWARE\JavaSoft\Java Runtime Environment", virtualMachines);

            //// TODO: most suitable Java Virtual Machine
            int bits = SystemHelper.ProcessBitness();
            if (minVersion.Length == 0 && maxVersion.Length == 0)
                throw new MajalaException(string.Format(CultureInfo.CurrentCulture, Resources.NoSuitableJavaFoundV1, bits));

            if (maxVersion.Length == 0)
                throw new MajalaException(string.Format(CultureInfo.CurrentCulture, Resources.NoSuitableJavaFoundV2, bits, minVersion));

            throw new MajalaException(string.Format(CultureInfo.CurrentCulture, Resources.NoSuitableJavaFoundV3, bits, minVersion, maxVersion));
        }

        /// <summary>
        /// Adds all JVMs registered in the given Registry key to the given Dictionary.
        /// </summary>
        /// <param name="regKey">Registry key (under HKLM) that contains a list of installed JVMs.</param>
        /// <param name="virtualMachines">Dictionary where the JVMs are added.</param>
        private static void AddVirtualMachinesFromRegistry(string regKey, SortedDictionary<string, string> virtualMachines)
        {
            RegistryKey registry = Registry.LocalMachine.OpenSubKey(regKey);
            string[] jvmVersions = registry.GetSubKeyNames();
            foreach (string version in jvmVersions)
            {
                string key = regKey + @"\" + version + @"\" + "JavaHome";
                string javaHome = (string)Registry.GetValue("HKEY_LOCAL_MACHINE", key, string.Empty);

                if (virtualMachines.ContainsKey(javaHome))
                {
                    string currentVersion;
                    virtualMachines.TryGetValue(javaHome, out currentVersion);

                    // i. e. "1.7" will get replaced by "1.7.0_10"...
                    if (version.Length > currentVersion.Length)
                    {
                        virtualMachines.Remove(javaHome);
                        virtualMachines.Add(javaHome, version);
                    }
                }
                else
                {
                    virtualMachines.Add(javaHome, version);
                }
            }
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
