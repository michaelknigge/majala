namespace MK.Majala.Core
{
    using System;

    /// <summary>
    /// This class holds all settings from the configration file. Since we use .NET 2.0
    /// we can not use autoatic properties (introduced with .NET 3.0)....
    /// </summary>
    public class Settings
    {
        private string javaVersionMin;
        private string javaVersionMax;
        private string workingDirectory;
        private string javaDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings" /> class.
        /// This constructor initializes all settings with default values. A setting that
        /// is not set is initialized with an empty string.
        /// </summary>
        public Settings()
        {
            this.JavaVersionMin = string.Empty;
            this.JavaVersionMax = string.Empty;
            this.WorkingDirectory = string.Empty;
            this.JavaDirectory = string.Empty;
        }

        /// <summary>
        /// Gets the minimum Java Version required for the application.
        /// </summary>
        public string JavaVersionMin
        {
            get { return this.javaVersionMin; }
            internal set { this.javaVersionMin = value; }
        }

        /// <summary>
        /// Gets the maximum Java Version allowed for the application.
        /// </summary>
        public string JavaVersionMax
        {
            get { return this.javaVersionMax; }
            internal set { this.javaVersionMax = value; }
        }

        /// <summary>
        /// Gets the working directory to be changed to.
        /// </summary>
        public string WorkingDirectory
        {
            get { return this.workingDirectory; }
            internal set { this.workingDirectory = value; }
        }

        /// <summary>
        /// Gets the directory containing the JDK or JRE.
        /// </summary>
        public string JavaDirectory
        {
            get { return this.javaDirectory; }
            internal set { this.javaDirectory = value; }
        }
    }
}
