namespace MK.Majala.Windows
{
    using System;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// The StandardMessageBox uses the well known Message Box Dialog.
    /// </summary>
    internal class StandardMessageBox : IMessageBox
    {
        /// <summary>
        /// Displays a Message Box Window with an OK-Button and an Error Icon.
        /// </summary>
        /// <param name="caption">Title of the Message Box Window.</param>
        /// <param name="text">Text of the Message Box Window.</param>
        public void ShowError(string caption, string text)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
