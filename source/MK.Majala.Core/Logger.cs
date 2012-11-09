namespace MK.Majala.Core
{
    using System;

    /// <summary>
    /// A logger for log/info/debug messages.
    /// </summary>
    public class Logger
    {
        private IAppender appender;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger" /> class.
        /// </summary>
        /// <param name="appender">An IAppender for the log messages.</param>
        public Logger(IAppender appender)
        {
            this.appender = appender;
        }

        /// <summary>
        /// Appends a message to the log. The message will get prefixed with the current date and time.
        /// </summary>
        /// <param name="message">Message to be appended.</param>
        public void Log(string message)
        {
            this.appender.Append(DateTime.Now.ToString("u") + " : " + message);
        }

        /// <summary>
        /// Closes the IAppender.
        /// </summary>
        public void Close()
        {
            this.appender.Dispose();
        }
    }
}
