namespace MK.Majala.Core
{
    using System;

    /// <summary>
    /// Simple static class that loads the settings. It determines from where the
    /// settings have to be loaded (external or embeded file) and is also able
    /// to load the settings in various formats.
    /// </summary>
    public static class SettingsLoader
    {
        /// <summary>
        /// Factory method that determines the format and the location of the
        /// configuration and returns a Settings object that holds this
        /// configuration.
        /// </summary>
        /// <returns>A Settings object with the loaded configration.</returns>
        public static Settings LoadSettings()
        {
            //// TODO: Find the location of the configuration file (external file overrides the embedded file!)
            //// TODO: Determine the format of the file and load it (XML or text).
            return new Settings();
        }
    }
}
