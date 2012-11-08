namespace MK.Majala.Service
{
    using System;
    using MK.Majala.Core;

    /// <summary>
    /// This is the main class for the Windows service application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// This is the entry point of the Windows service application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            new ServiceLauncher().RunApplication(args);
        }
    }
}
