namespace MK.Majala.Console
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// This class holds some native Methods.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// The HandlerRoutine gets called if a console control handler has been installed
        /// with the method SetConsoleCtrlHandler.
        /// </summary>
        /// <param name="ctrlType">The event that occured.</param>
        /// <returns>true if the method handled the event, false if not. If false is returned, the next event handler in a chain of registered handlers gets called.</returns>
        internal delegate bool HandlerRoutine(CtrlTypes ctrlType);

        /// <summary>
        /// The varoius events can be occur and handled by the HandlerRoutine.
        /// </summary>
        internal enum CtrlTypes
        {
            /// <summary>
            /// Conotrl-C was pressed (CTRL_C_EVENT).
            /// </summary>
            ControlC = 0,

            /// <summary>
            /// Conotrl-Break was pressed (CTRL_BREAK_EVENT).
            /// </summary>
            ControlBreak = 1,

            /// <summary>
            /// The console windows is to be closed (CTRL_CLOSE_EVENT).
            /// </summary>
            Close = 2,

            /// <summary>
            /// The user loggs off (CTRL_LOGOFF_EVENT).
            /// </summary>
            LogOff = 5,

            /// <summary>
            /// The system is being shut down (CTRL_SHUTDOWN_EVENT).
            /// </summary>
            Shutdown = 6
        }

        /// <summary>
        /// Adds of removes a console control handler.
        /// </summary>
        /// <param name="handler">Delegate to the method that handles the event.</param>
        /// <param name="addOrRemove">true if the handler shall be installed, false if the handler is to be removed.</param>
        /// <returns>True if the handler was added/removed successfully.</returns>
        [DllImport("kernel32.dll")]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        internal static extern bool SetConsoleCtrlHandler(HandlerRoutine handler, [MarshalAs(UnmanagedType.Bool)] bool addOrRemove);
    }
}
