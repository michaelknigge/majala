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
        private Logger logger;

        /// <summary>
        /// Runs the Java program.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public void RunApplication(string[] args)
        {
            try
            {
                Settings settings = SettingsLoader.LoadSettings();

                // TODO determine real logger from settings...
                this.logger = new Logger(new ConsoleAppender());

                if (settings.WorkingDirectory.Length != 0)
                    Directory.SetCurrentDirectory(settings.WorkingDirectory);

                ////this.ParseCommandLineArguments(args);

                Jvm jvm;
                if (settings.JavaDirectory.Length == 0)
                    jvm = JvmChooser.LoadJvm(settings.JavaVersionMin, settings.JavaVersionMax);
                else
                    jvm = JvmChooser.LoadJvm(settings.JavaDirectory, true, settings.JavaVersionMin, settings.JavaVersionMax);

                ////this.RegisterShutdownHook(settings);
                ////this.LoadAndRunMainClass();
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }

            this.logger.Close();
        }

        /// <summary>
        /// This method can be used from the derived classes to force
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
    }
}
