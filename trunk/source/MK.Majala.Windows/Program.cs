namespace MK.Majala.Windows
{
    using System;
    using MK.Majala.Core;

    /// <summary>
    /// This is the main class for the Windows application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// This is the entry point of the Windows application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            new WindowsLauncher().RunApplication(Environment.GetCommandLineArgs());
        }
    }
}
