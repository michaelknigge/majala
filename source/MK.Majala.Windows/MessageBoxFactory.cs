namespace MK.Majala.Windows
{
    using System;

    /// <summary>
    /// Factory for the IMessageBox.
    /// </summary>
    internal static class MessageBoxFactory
    {
        /// <summary>
        /// Creates a ModernMessageBox (if running on Windows Vista or newer) or a StandardMessageBox Object.
        /// </summary>
        /// <returns>The created IMessageBox Object</returns>
        internal static IMessageBox Create()
        {
            OperatingSystem os = Environment.OSVersion;

            //// No one should ever run majala on Mono.... But who knows...
            if (os.Platform != PlatformID.Win32NT)
                return new StandardMessageBox();

            if (os.Version.CompareTo(new Version(6, 0, 5243)) >= 0)
                return new ModernMessageBox();
            else
                return new StandardMessageBox();
        }
    }
}
