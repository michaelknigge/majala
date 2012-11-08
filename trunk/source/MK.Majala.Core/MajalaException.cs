namespace MK.Majala.Core
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// The MajalaException is thrown for all errors that are determined
    /// by Majala itself.
    /// </summary>
    [Serializable]
    public class MajalaException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MajalaException" /> class.
        /// </summary>
        public MajalaException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MajalaException" /> class.
        /// </summary>
        /// <param name="message">Error message describing the exception.</param>
        public MajalaException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MajalaException" /> class.
        /// </summary>
        /// <param name="message">Error message describing the exception.</param>
        /// <param name="ex">The exception that is to be nested in this exception.</param>
        public MajalaException(string message, Exception ex)
            : base(message, ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MajalaException" /> class.
        /// </summary>
        /// <param name="si">SerializationInfo object needed for deserialization.</param>
        /// <param name="sc">StreamingContext object needed for deserialization.</param>
        protected MajalaException(SerializationInfo si, StreamingContext sc)
            : base(si, sc)
        {
        }
    }
}
