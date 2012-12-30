namespace MK.Majala.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
        /// <param name="minVersion">Minimum Java Version required.</param>
        /// <param name="maxVersion">Maximum Java Version allowed (or an empty String).</param>
        /// <returns>Jvm object with a suitable loaded JVM.</returns>
        public static Jvm LoadJvm(string userPath, bool fallback, string minVersion, string maxVersion)
        {
            Logger.Log(string.Format(CultureInfo.CurrentCulture, Resources.LogLoadCustomJvm, userPath));

            if (JvmCollection.IsValidJavaHome(userPath))
                return new Jvm(userPath);

            if (!fallback)
                throw new MajalaException(string.Format(CultureInfo.CurrentCulture, Resources.DirectoryContainsNoJava, userPath));

            return LocateAndLoadInstalledJvm(minVersion, maxVersion);
        }

        /// <summary>
        /// Looks up the most suitable Java Virtual Machine installed on the local system and loads it.
        /// If no suitable JVM can be found an MajalaExeption will be thrown.
        /// </summary>
        /// <param name="minVersion">Minimum Java Version required.</param>
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
        /// <param name="minVersion">Minimum Java Version required.</param>
        /// <param name="maxVersion">Maximum Java Version allowed (or an empty String).</param>
        /// <returns>Jvm object with a suitable loaded JVM.</returns>
        private static Jvm LocateAndLoadInstalledJvm(string minVersion, string maxVersion)
        {
            JvmCollection jvmCollection = new JvmCollection();
            Version minRequiredVersion = new Version(minVersion);
            Version maxRequiredVersion = maxVersion.Length > 0 ? new Version(maxVersion) : null;

            if (maxVersion.Length == 0)
                Logger.Log(string.Format(CultureInfo.CurrentCulture, Resources.LogRequiredJvmV1, minVersion));
            else
                Logger.Log(string.Format(CultureInfo.CurrentCulture, Resources.LogRequiredJvmV2, maxVersion));

            int bits = SystemHelper.ProcessBitness();
            if (jvmCollection.InstalledVersions.Count == 0)
                throw new MajalaException(string.Format(CultureInfo.CurrentCulture, Resources.NoSuitableJavaFoundV1, bits));

            foreach (KeyValuePair<string, string> p in jvmCollection.InstalledVersions)
            {
                string path = p.Key;
                string version = p.Value;
                Version currentVersion = new Version(version);

                if (currentVersion.CompareTo(minRequiredVersion) >= 0)
                {
                    if (maxVersion.Length == 0 || currentVersion.CompareTo(maxRequiredVersion) <= 0)
                    {
                        Logger.Log(string.Format(CultureInfo.CurrentCulture, Resources.LogJvmIsOk, currentVersion));
                        return new Jvm(path);
                    }
                    else
                    {
                        Logger.Log(string.Format(CultureInfo.CurrentCulture, Resources.LogJvmIsTooYoung, currentVersion));
                    }
                }
                else
                {
                    Logger.Log(string.Format(CultureInfo.CurrentCulture, Resources.LogJvmIsTooOld, currentVersion));
                }
            }

            if (maxVersion.Length == 0)
                throw new MajalaException(string.Format(CultureInfo.CurrentCulture, Resources.NoSuitableJavaFoundV2, bits, minVersion));
            else
                throw new MajalaException(string.Format(CultureInfo.CurrentCulture, Resources.NoSuitableJavaFoundV3, bits, minVersion, maxVersion));
        }
    }
}
