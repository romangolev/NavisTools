using System;

namespace NavisTools.Interfaces
{
    /// <summary>
    /// Provides configuration and settings management.
    /// </summary>
    public interface IConfigurationService
    {
        /// <summary>
        /// Gets the configured parent parameter name.
        /// </summary>
        string ParentParameterName { get; }

        /// <summary>
        /// Gets the configured parent category name.
        /// </summary>
        string ParentCategoryName { get; }

        /// <summary>
        /// Opens the settings dialog.
        /// </summary>
        void OpenSettings();

        /// <summary>
        /// Resets all settings to their default values.
        /// </summary>
        void ResetToDefaults();

        /// <summary>
        /// Event raised when settings are changed.
        /// </summary>
        event EventHandler SettingsChanged;
    }
}
