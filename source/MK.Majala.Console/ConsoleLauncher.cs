namespace MK.Majala.Core
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// The Launcher for console applications.
    /// </summary>
    public class ConsoleLauncher : Launcher
    {
        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(HandlerRoutine handler, bool addOrRemove);

        private delegate bool HandlerRoutine(CtrlTypes CtrlType);

        private static readonly ConsoleLauncher instance = new ConsoleLauncher();

        private enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private ConsoleLauncher()
        {
        }

        /// <summary>
        /// Gets the singleton instance of the ConsoleLauncher.
        /// </summary>
        public static ConsoleLauncher Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Prints the given message to the standard error stream.
        /// </summary>
        /// <param name="message">The message to be printed.</param>
        public override void ShowError(string message)
        {
            Console.Error.WriteLine(message);
        }

        /// <summary>
        /// Registers a Callback that gets called if the user presses CTRL-C.
        /// </summary>
        public override void RegisterShutdownHook()
        {
            //// Due to http://www.codeproject.com/Articles/2357/Console-Event-Handling (see comments at the bottom of the page)
            //// the events CTRL_LOGOFF_EVENT and CTRL_SHUTDOWN_EVENT are no longer raised under Windows 7. We'll have to
            //// x-check this later....
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlHandler), true);
        }

        /// <summary>
        /// Handles the console control event (i. e. "CTRL-C" or "CTRL-Break").
        /// </summary>
        /// <param name="ctrlType">Type of even that has beed raised.</param>
        /// <returns>true if this method has handled the event, false if not. If false is returned, the next event handler in a chain of registered handlers is called.</returns>
        private static bool ConsoleCtrlHandler(CtrlTypes ctrlType)
        {
            ConsoleLauncher.Instance.ShutdownApplication();
            return false; // Do not call any other CtrlHandlers....
        }
    }
}
