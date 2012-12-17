namespace MK.Majala.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using Microsoft.Win32;

    /// <summary>
    /// This class holds a list of available JVMs.
    /// </summary>
    public class JvmCollection
    {
        /// <summary>
        /// List of all successfully added custom Java versions.
        /// </summary>
        private List<string> customJvms;

        /// <summary>
        /// Key-Value Collection of installed Java versions (key = JAVA_HOME directory, value = version).
        /// </summary>
        private SortedDictionary<string, string> installedJvms;

        /// <summary>
        /// Initializes a new instance of the <see cref="JvmCollection" /> class.
        /// </summary>
        public JvmCollection() :
            this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JvmCollection" /> class. This
        /// constructor is mainly for NUnit tests...
        /// </summary>
        /// <param name="addInstalled">True if all installed JVMs should be added automatically.</param>
        public JvmCollection(bool addInstalled)
        {
            this.customJvms = new List<string>();
            this.installedJvms = new SortedDictionary<string, string>(new JavaVersionComparer());

            if (addInstalled)
            {
                this.AddVirtualMachinesFromRegistry(@"SOFTWARE\JavaSoft\Java Development Kit");
                this.AddVirtualMachinesFromRegistry(@"SOFTWARE\JavaSoft\Java Runtime Environment");
            }
        }

        /// <summary>
        /// Gets a read-only collection of all custom Java versions.
        /// </summary>
        public ReadOnlyCollection<string> CustomVersions
        {
            get
            {
                return new ReadOnlyCollection<string>(this.customJvms);
            }
        }

        /// <summary>
        /// Gets a read-only collection of all installed Java versions.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "it is clean code!")]
        public ReadOnlyCollection<KeyValuePair<string, string>> InstalledVersions
        {
            get
            {
                List<KeyValuePair<string, string>> installed = new List<KeyValuePair<string, string>>();
                foreach (string key in this.installedJvms.Keys)
                    installed.Add(new KeyValuePair<string, string>(key, this.installedJvms[key]));

                return new ReadOnlyCollection<KeyValuePair<string, string>>(installed);
            }
        }

        /// <summary>
        /// Adds a custom JAVA_HOME to the list of available JVMs if the given directory is
        /// a valid JAVA_HOME.
        /// </summary>
        /// <param name="javaHome">Path to a custom JAVA_HOME.</param>
        /// <returns>true if the JVM was added, false if not.</returns>
        public bool AddCustomJvm(string javaHome)
        {
            if (!JvmCollection.IsValidJavaHome(javaHome))
                return false;

            this.customJvms.Add(javaHome);
            return true;
        }

        /// <summary>
        /// Adds an installed JAVA_HOME to the list of available JVMs if the given directory is
        /// a valid JAVA_HOME.
        /// </summary>
        /// <param name="version">Version of the Java installation.</param>
        /// <param name="javaHome">Path to a custom JAVA_HOME.</param>
        /// <returns>true if the JVM was added, false if not.</returns>
        public bool AddInstalledJvm(string version, string javaHome)
        {
            if (!JvmCollection.IsValidJavaHome(javaHome))
                return false;

            if (this.installedJvms.ContainsKey(javaHome))
            {
                string currentVersion;
                this.installedJvms.TryGetValue(javaHome, out currentVersion);

                // i. e. "1.7" will get replaced by "1.7.0_10"...
                if (version.Length > currentVersion.Length)
                {
                    this.installedJvms.Remove(javaHome);
                    this.installedJvms.Add(javaHome, version);
                }
            }
            else
            {
                this.installedJvms.Add(javaHome, version);
            }

            return true;
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

        /// <summary>
        /// Adds all JVMs registered in the given Registry key to the list of installed JVMs.
        /// </summary>
        /// <param name="regKey">Registry key (under HKLM) that contains a list of installed JVMs.</param>
        private void AddVirtualMachinesFromRegistry(string regKey)
        {
            RegistryKey registry = Registry.LocalMachine.OpenSubKey(regKey);
            string[] jvmVersions = registry.GetSubKeyNames();
            foreach (string version in jvmVersions)
            {
                string key = regKey + @"\" + version + @"\" + "JavaHome";
                string javaHome = (string)Registry.GetValue("HKEY_LOCAL_MACHINE", key, string.Empty);

                this.AddInstalledJvm(version, javaHome);
            }
        }
    }
}
