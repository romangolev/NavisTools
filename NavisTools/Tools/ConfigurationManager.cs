using NavisTools.Interfaces;
using NavisTools.Models;
using NavisTools.Services;
using System;

namespace NavisTools.UI
{
    /// <summary>
    /// Static facade for configuration management.
    /// Delegates to IConfigurationService for backwards compatibility.
    /// </summary>
    [Obsolete("Use IConfigurationService from ServiceLocator instead.")]
    public static class ConfigurationManager
    {
        private static IConfigurationService GetService()
        {
            return ServiceLocator.Resolve<IConfigurationService>();
        }

        /// <summary>
        /// Gets the current settings (for backwards compatibility).
        /// </summary>
        public static SettingsModel Settings
        {
            get
            {
                var service = GetService() as ConfigurationService;
                return service?.Settings ?? new SettingsModel();
            }
        }

        /// <summary>
        /// Opens the settings dialog.
        /// </summary>
        public static void OpenSettings()
        {
            GetService()?.OpenSettings();
        }

        /// <summary>
        /// Resets settings to defaults.
        /// </summary>
        public static void ResetToDefaults()
        {
            GetService()?.ResetToDefaults();
        }
    }
}
