namespace MK.Majala.Core
{
    using System;
    using System.Text;

    /// <summary>
    /// The Launcher for Windows services.
    /// </summary>
    public class ServiceLauncher : Launcher
    {
        /// <summary>
        /// Appends the given message to the Windows event log.
        /// </summary>
        /// <param name="message">The message to be appended to the windows event log.</param>
        public override void ShowError(string message)
        {
            // TODO: append the message to the windows event log...

            // http://www.codeproject.com/Articles/39218/How-To-Create-a-Windows-Event-Log-and-Write-your-C
            // http://www.codeproject.com/Articles/29052/Writing-to-System-Event-Log
        }

        /// <summary>
        /// Registers a Callback funktion that gets called if the Windows servics
        /// has to be shutdown...
        /// </summary>
        public override void RegisterShutdownHook()
        {
        }
    }
}
