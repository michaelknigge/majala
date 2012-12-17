namespace MK.Majala.Windows
{
    using System;
    using System.Text;

    /// <summary>
    /// The ModernMessageBox uses the new Task Dialog (available since Windows Vista) as the Message Box.
    /// </summary>
    internal class ModernMessageBox : IMessageBox
    {
        /// <summary>
        /// Displays a Message Box Window with an OK-Button and an Error Icon.
        /// </summary>
        /// <param name="caption">Title of the Message Box Window.</param>
        /// <param name="text">Text of the Message Box Window.</param>
        public void ShowError(string caption, string text)
        {
            int pressedButton;
            NativeMethods.TaskDialog(IntPtr.Zero, IntPtr.Zero, caption, text, NativeMethods.TaskDialogCommonButtons.Ok, (IntPtr)NativeMethods.VistaTaskDialogIcon.Error, out pressedButton);
        }
    }
}
