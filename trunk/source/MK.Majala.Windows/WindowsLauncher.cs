namespace MK.Majala.Windows
{
    using System;
    using System.Text;
    using System.Windows.Forms;
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
            MessageBox.Show(message, Resources.ErrorBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
