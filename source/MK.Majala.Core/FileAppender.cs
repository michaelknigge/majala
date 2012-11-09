namespace MK.Majala.Core
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// The FileAppender appends/writes log messages to a file.
    /// </summary>
    public class FileAppender : IAppender
    {
        private TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileAppender" /> class.
        /// </summary>
        /// <param name="filename">Filename of the log file.</param>
        public FileAppender(string filename)
        {
            try
            {
                this.writer = new StreamWriter(filename, true);
            }
            catch
            {
                this.writer = null;
            }
        }

        /// <summary>
        /// Writes a log message to the log file.
        /// </summary>
        /// <param name="message">Message to be written.</param>
        public void Append(string message)
        {
            try
            {
                if (this.writer != null)
                    this.writer.WriteLine(message);
            }
            catch
            {
                // We ignore errors on appending to the log file - we do not want to
                // exit the application due to a permissing denied error or a full disk...
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
            try
            {
                if (disposing && this.writer != null)
                    this.writer.Close();
            }
            catch
            {
                // We ignore errors on closing the log file... There is nothing we could do...
            }
        }
    }
}
