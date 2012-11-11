namespace MK.Majala.Core
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// An IAppender that writes log messages to the console (standard error stream).
    /// </summary>
    public class ConsoleAppender : IAppender
    {
        /// <summary>
        /// Writes a log message to the standard error stream.
        /// </summary>
        /// <param name="message">Message to be written.</param>
        public void Append(string message)
        {
            try
            {
                Console.Error.WriteLine(message);
            }
            catch (IOException)
            {
                // We ignore any errors - an error while logging should not harm the application...
            }
        }

        /// <summary>
        /// Dispose this object and tell the garbage collector to not call the finalize method.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes this object.
        /// </summary>
        /// <param name="disposing">true if managed and unnamagend resources should be freed. false if only unmanages resources should be freed.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources here...
            }
        }
    }
}
