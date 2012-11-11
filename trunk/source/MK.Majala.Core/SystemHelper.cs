namespace MK.Majala.Core
{
    using System;

    /// <summary>
    /// This helper class contains some methods for getting system level information.
    /// </summary>
    public static class SystemHelper
    {
        /// <summary>
        /// Determines the bitness of the currenty running process.
        /// </summary>
        /// <returns>Bitness of the currenty running process (which is 32 or 64).</returns>
        public static int ProcessBitness()
        {
            return IntPtr.Size == 8 ? 64 : 32;
        }

        /// <summary>
        /// Determines the bitness of the currenty running operating system.
        /// </summary>
        /// <returns>Bitness of the currenty running operating system (which is 32 or 64).</returns>
        public static int SystemBitness()
        {
            return Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE").Equals("AMD64") ? 64 : 32;
        }
    }
}
