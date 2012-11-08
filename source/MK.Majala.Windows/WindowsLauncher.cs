namespace MK.Majala.Core
{
    using System;
    using System.Text;

    /// <summary>
    /// The Launcher for GUI applications.
    /// </summary>
    public class WindowsLauncher : Launcher
    {
        /// <summary>
        /// Shows a Message Box (or a modern Task Dialog if available) to the
        /// user showing the given message.
        /// </summary>
        /// <param name="message">The message to be shown.</param>
        public override void ShowError(string message)
        {
            // TODO MessageBox and/or Task Dialog

            // http://www.codeproject.com/Articles/49268/Windows-7-New-Features-Explained-Using-NET
            // http://archive.msdn.microsoft.com/WindowsAPICodePack
            // http://www.codeproject.com/Tips/247358/TaskDilaog-via-Windows-API-Code-Pack-for-Microsoft
            // http://www.codeproject.com/Articles/17026/TaskDialog-for-WinForms
            // http://www.codeproject.com/Articles/21276/Vista-TaskDialog-Wrapper-and-Emulator
        }

        /// <summary>
        /// Does nothing...
        /// </summary>
        public override void RegisterShutdownHook()
        {
        }
    }
}
