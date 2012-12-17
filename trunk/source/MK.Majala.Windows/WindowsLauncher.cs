namespace MK.Majala.Windows
{
    using System;
    using System.Text;
    using MK.Majala.Core;
    using MK.Majala.Windows.Properties;

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
            MessageBoxFactory.Create().ShowError(Resources.ErrorBoxCaption, message);
        }

        /// <summary>
        /// Does nothing...
        /// </summary>
        public override void RegisterShutdownHook()
        {
            // TODO: Should we react on a windows shutdown event?
        }
    }
}
