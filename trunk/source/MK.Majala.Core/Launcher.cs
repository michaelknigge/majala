namespace MK.Majala.Core
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// This abstract class runs majala. Application type specific functions (like
    /// output of error messages) are implemented in the derived classes.
    /// </summary>
    public abstract class Launcher
    {
        // TODO check if settings and/or jvm can be local in RunApplication....
        private Settings settings;
        private Jvm jvm;

        /// <summary>
        /// Runs the Java program.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public void RunApplication(string[] args)
        {
            try
            {
                // Each of these methods has to throw a MajalaException on errors (which holds
                // the "real" exception as a inner exception (if there is one)...
                this.LoadSettings();
                this.InitializeLogger();
                this.SetWorkingDirectory();
                this.ParseCommandLineArguments(args);
                this.LoadJavaVirtualMachine();
                this.RegisterHooks();
                this.LoadAndInvokeMainClass();
            }
            catch (MajalaException ex)
            {
                this.ShowError(ex.Message + ex.InnerException == null ? string.Empty : " " + ex.InnerException.Message);
            }

            Logger.Close();
        }

        /// <summary>
        /// This method can be used from the derived classes to initiate
        /// a shutdown of the launched Java application.
        /// </summary>
        public void ShutdownApplication()
        {
        }

        /// <summary>
        /// This abstract method is used to output error messages.
        /// </summary>
        /// <param name="message">The message.</param>
        public abstract void ShowError(string message);

        /// <summary>
        /// This abstract method is called just before the Java main class is loaded
        /// and executed. The derived class can decide to register a shutdown hook (i. e.
        /// trap CTRL-C) or just do nothing.
        /// </summary>
        public abstract void RegisterShutdownHook();

        /// <summary>
        /// Loads the settings from an external or embedded file.
        /// </summary>
        private void LoadSettings()
        {
            this.settings = SettingsLoader.LoadSettings();
        }

        /// <summary>
        /// Initializes the configured Logger.
        /// </summary>
        private void InitializeLogger()
        {
            // TODO determine real logger from settings...
            Logger.Appender = new ConsoleAppender();
        }

        /// <summary>
        /// Changes the working directory if requested by the configuration file.
        /// </summary>
        private void SetWorkingDirectory()
        {
            if (this.settings.WorkingDirectory.Length != 0)
                Directory.SetCurrentDirectory(this.settings.WorkingDirectory);
        }

        /// <summary>
        /// Parses all command line arguments.
        /// </summary>
        private void ParseCommandLineArguments(string[] args)
        {
            // TODO: implement ParseCommandLineArguments().
        }

        /// <summary>
        /// Determines the best suitable JVM and loads it.
        /// </summary>
        private void LoadJavaVirtualMachine()
        {
            if (this.settings.JavaDirectory.Length == 0)
                this.jvm = JvmChooser.LoadJvm(this.settings.JavaVersionMin, this.settings.JavaVersionMax);
            else
                this.jvm = JvmChooser.LoadJvm(this.settings.JavaDirectory, true, this.settings.JavaVersionMin, this.settings.JavaVersionMax);
        }

        /// <summary>
        /// Registers all desired hooks (currently there is just one - a shutdown hook).
        /// </summary>
        private void RegisterHooks()
        {
            // TODO: implement RegisterHooks().
        }

        /// <summary>
        /// Loads the main class and invokes it..
        /// </summary>
        private void LoadAndInvokeMainClass()
        {
            // TODO: implement LoadAndInvokeMainClass().
        }
    }
}
