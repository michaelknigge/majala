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

        private enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
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
            //// http://www.codeproject.com/Articles/2357/Console-Event-Handling
            //// kommentar eines users: tut nicht mehr unter Windows 7, lieber
            //// die Klasse Microsoft.Win32.SystemEvents verwenden!!!!
            //// ---> ggf ist aber "nur" Logoff / Shutdown gemeint...


            //SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlHandler), true);
            Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            {
                // TEST   wird der bei allen benötigne Events aufgerufen ?!?!?!?
                //  -> CTRL-C
                //  -> CTRL-BREAK
                //  -> CLOSE
                e.Cancel = true;
                this.ShutdownApplication();
            };
        }

        private static bool ConsoleCtrlHandler(CtrlTypes ctrlType)
        {
            // Singleton!!!! this = private static!!!
            this.ShutdownApplication();
        }
    }
}
