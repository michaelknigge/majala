namespace MK.Majala.Core
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Helper class with static methods that invoke native functions of the Windows API.
    /// </summary>
    public static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string fn);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr library, string function);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr library);
    }
}
