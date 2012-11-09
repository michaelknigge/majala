namespace MK.Majala.Console
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using MK.Majala.Core;

    /// <summary>
    /// The Launcher for console applications.
    /// </summary>
    public class ConsoleLauncher : Launcher
    {
        /// <summary>
        /// The instance of this singleton class.
        /// </summary>
        public static readonly ConsoleLauncher Instance = new ConsoleLauncher();

        private ConsoleLauncher()
        {
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
            NativeMethods.SetConsoleCtrlHandler(new MK.Majala.Console.NativeMethods.HandlerRoutine(ConsoleCtrlHandler), true);
        }

        /// <summary>
        /// Handles the console control event (i. e. "CTRL-C" or "CTRL-Break").
        /// </summary>
        /// <param name="ctrlType">Type of even that has beed raised.</param>
        /// <returns>true if this method has handled the event, false if not. If false is returned, the next event handler in a chain of registered handlers is called.</returns>
        private static bool ConsoleCtrlHandler(NativeMethods.CtrlTypes ctrlType)
        {
            ConsoleLauncher.Instance.ShutdownApplication();
            return false; // Do not call any other CtrlHandlers....
        }
    }
}
