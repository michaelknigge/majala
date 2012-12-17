namespace MK.Majala.Windows
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// This class holds some native Methods.
    /// </summary>
    internal class NativeMethods
    {
        /// <summary>
        /// The TaskDialog common button flags used to specify the builtin bottons to show in the TaskDialog.
        /// </summary>
        [Flags]
        internal enum TaskDialogCommonButtons
        {
            /// <summary>
            /// No common buttons.
            /// </summary>
            None = 0,

            /// <summary>
            /// OK common button. If selected Task Dialog will return DialogResult.OK.
            /// </summary>
            Ok = 0x0001,

            /// <summary>
            /// Yes common button. If selected Task Dialog will return DialogResult.Yes.
            /// </summary>
            Yes = 0x0002,

            /// <summary>
            /// No common button. If selected Task Dialog will return DialogResult.No.
            /// </summary>
            No = 0x0004,

            /// <summary>
            /// Cancel common button. If selected Task Dialog will return DialogResult.Cancel.
            /// If this button is specified, the dialog box will respond to typical cancel actions (Alt-F4 and Escape).
            /// </summary>
            Cancel = 0x0008,

            /// <summary>
            /// Retry common button. If selected Task Dialog will return DialogResult.Retry.
            /// </summary>
            Retry = 0x0010,

            /// <summary>
            /// Close common button. If selected Task Dialog will return this value.
            /// </summary>
            Close = 0x0020,
        }

        /// <summary>
        /// The System icons the TaskDialog supports.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32", Justification = "Needs to be unsigned.")]
        internal enum VistaTaskDialogIcon : uint
        {
            /// <summary>
            /// No Icon.
            /// </summary>
            None = 0,

            /// <summary>
            /// System warning icon.
            /// </summary>
            Warning = 0xFFFF,

            /// <summary>
            /// System Error icon.
            /// </summary>
            Error = 0xFFFE,

            /// <summary>
            /// System Information icon.
            /// </summary>
            Information = 0xFFFD,

            /// <summary>
            /// Shield icon.
            /// </summary>
            Shield = 0xFFFC,
        }

        // <summary>
        // TaskDialog, available since Windows Vista.
        // </summary>
        // <param name="parent">Parent window.</param>
        // <param name="instance">Module instance to get resources from.</param>
        // <param name="caption">Title of the Task Dialog window.</param>
        // <param name="text">The main instructions.</param>
        // <param name="commonButtons">Common push buttons to show.</param>
        // <param name="icon">The main icon.</param>
        // <param name="button">The push button pressed.</param>
        [DllImport("ComCtl32", CharSet = CharSet.Unicode, PreserveSig = false)]
        internal static extern void TaskDialog(IntPtr parent, IntPtr instance, string caption, string text, TaskDialogCommonButtons commonButtons, IntPtr icon, out int button);
    }
}
