using System;
using System.Windows.Forms;
using NwApplication = Autodesk.Navisworks.Api.Application;

namespace NavisTools.UI
{
    /// <summary>
    /// Manages configuration settings for NavisTools
    /// </summary>
    public static class ConfigurationManager
    {
        private static SettingsModel _settings;

        static ConfigurationManager()
        {
            _settings = new SettingsModel();
        }

        /// <summary>
        /// Gets the current settings
        /// </summary>
        public static SettingsModel Settings => _settings;

        /// <summary>
        /// Opens the settings dialog
        /// </summary>
        public static void OpenSettings()
        {
            using (var form = new ConfigurationForm(_settings))
            {
                if (form.ShowDialog(NwApplication.Gui.MainWindow) == DialogResult.OK)
                {
                    // Settings were saved, apply them
                    ApplySettings();
                }
            }
        }

        /// <summary>
        /// Resets settings to defaults
        /// </summary>
        public static void ResetToDefaults()
        {
            var result = MessageBox.Show(
                NwApplication.Gui.MainWindow,
                "Are you sure you want to reset all settings to their default values?",
                "Reset Settings",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _settings.ResetToDefaults();
                ApplySettings();
                MessageBox.Show(
                    NwApplication.Gui.MainWindow,
                    "Settings have been reset to defaults.",
                    "Settings Reset",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private static void ApplySettings()
        {
            // TODO: Apply settings to the application
            // This would refresh any UI or behavior based on settings
        }
    }

    /// <summary>
    /// Model class for application settings
    /// </summary>
    public class SettingsModel
    {
        public string ParentParameterName { get; set; }
        public string ParentCategoryName { get; set; }

        public SettingsModel()
        {
            ResetToDefaults();
        }

        public void ResetToDefaults()
        {
            ParentParameterName = "ParentName";
            ParentCategoryName = "Item";
        }
    }
}
