namespace MK.Majala.Core
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// This class holds some native Methods.
    /// </summary>
    internal class NativeMethods
    {
        [DllImport("kernel32.dll")]
        internal static extern IntPtr LoadLibrary(string fn);

        [DllImport("kernel32.dll")]
        internal static extern IntPtr GetProcAddress(IntPtr library, string function);

        [DllImport("kernel32.dll")]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        internal static extern bool FreeLibrary(IntPtr library);
    }
}
