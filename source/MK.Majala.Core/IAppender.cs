namespace MK.Majala.Core
{
    using System;
    using System.Text;

    /// <summary>
    /// An Appender writes (or appends) a log message to a target (like a file).
    /// </summary>
    public interface IAppender : IDisposable
    {
        /// <summary>
        /// Appends a log message to the log.
        /// </summary>
        /// <param name="message">Message to be appended.</param>
        void Append(string message);
    }
}
