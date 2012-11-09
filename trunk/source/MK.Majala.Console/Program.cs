namespace MK.Majala.Console
{
    using System;
    using MK.Majala.Core;

    /// <summary>
    /// This is the main class for the console application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// This is the entry point of the console application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            ConsoleLauncher.Instance.RunApplication(args);
        }
    }
}
