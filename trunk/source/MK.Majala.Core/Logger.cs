namespace MK.Majala.Core
{
    using System;
    using System.Globalization;

    /// <summary>
    /// A logger for log/info/debug messages.
    /// </summary>
    public static class Logger
    {
        private static IAppender appender = new VoidAppender();

        /// <summary>
        /// Gets or sets the minimum Java Version required for the application.
        /// </summary>
        public static IAppender Appender
        {
            get { return Logger.appender; }
            set { Logger.appender = value; }
        }

        /// <summary>
        /// Appends a message to the log. The message will get prefixed with the current date and time.
        /// </summary>
        /// <param name="message">Message to be appended.</param>
        public static void Log(string message)
        {
            Logger.appender.Append(DateTime.Now.ToString("u", CultureInfo.CurrentCulture) + " : " + message);
        }

        /// <summary>
        /// Closes the IAppender.
        /// </summary>
        public static void Close()
        {
            Logger.appender.Dispose();
            Logger.Appender = new VoidAppender();
        }
    }
}
