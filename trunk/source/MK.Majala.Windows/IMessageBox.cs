namespace MK.Majala.Windows
{
    using System;
    using System.Text;

    /// <summary>
    /// Interface for the ModernMessageBox and StandardMessageBox..
    /// </summary>
    internal interface IMessageBox
    {
        /// <summary>
        /// Displays a Message Box Window with an OK-Button and an Error Icon.
        /// </summary>
        /// <param name="caption">Title of the Message Box Window.</param>
        /// <param name="text">Text of the Message Box Window.</param>
        void ShowError(string caption, string text);
    }
}
