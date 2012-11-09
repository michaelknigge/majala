namespace MK.Majala.Core
{
    using System;

    /// <summary>
    /// An IAppender that does nothing.
    /// </summary>
    public class VoidAppender : IAppender
    {
        /// <summary>
        /// Writes a log message to the standard error stream.
        /// </summary>
        /// <param name="message">Message to be written.</param>
        public void Append(string message)
        {
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
            // This methon does only exist to make FxCop happy (rule CA1063)...
        }
    }
}
