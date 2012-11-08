namespace MK.Majala.Core
{
    using System;

    /// <summary>
    /// The JvmChooser chooses and decides which installed (or provided) Java version
    /// should be used.
    /// </summary>
    public static class JvmChooser
    {
        /// <summary>
        /// Looks up the most suitable Java Virtual Machine and loads it.
        /// </summary>
        /// <param name="userPath">User provided path (from the configuration file) to the JVM.</param>
        /// <param name="fallback">True if a system wide instaled JVM should be loaded if the user provided JVM can not be loaded.</param>
        /// <param name="minVersion">Minimum Java version required.</param>
        /// <param name="maxVersion">Maximum Java version allowed.</param>
        /// <returns>Jvm object with a suitable loaded JVM.</returns>
        public static Jvm LoadJvm(string userPath, bool fallback, string minVersion, string maxVersion)
        {
            //// TODO: if userPath points to a suitable JVM (bitness, version), loads it and return!
            return new Jvm(userPath);
        }

        /// <summary>
        /// Looks up the most suitable Java Virtual Machine installed on the local system and loads it.
        /// </summary>
        /// <param name="minVersion">Minimum Java version required.</param>
        /// <param name="maxVersion">Maximum Java version allowed.</param>
        /// <returns>Jvm object with a suitable loaded JVM.</returns>
        public static Jvm LoadJvm(string minVersion, string maxVersion)
        {
            return new Jvm("dummy");
        }
    }
}
