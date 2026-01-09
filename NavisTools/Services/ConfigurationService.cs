using NavisTools.Interfaces;
using NavisTools.Models;
using NavisTools.UI;
using System;
using System.Windows.Forms;
using NwApplication = Autodesk.Navisworks.Api.Application;

namespace NavisTools.Services
{
    /// <summary>
    /// Implementation of IConfigurationService that manages application settings.
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        private SettingsModel _settings;

        public event EventHandler SettingsChanged;

        public ConfigurationService()
        {
            _settings = new SettingsModel();
        }

        public string ParentParameterName => _settings.ParentParameterName;

        public string ParentCategoryName => _settings.ParentCategoryName;

        /// <summary>
        /// Gets the current settings model (for internal use).
        /// </summary>
        internal SettingsModel Settings => _settings;

        public void OpenSettings()
        {
            using (var form = new ConfigurationForm(_settings))
            {
                if (form.ShowDialog(GetMainWindow()) == DialogResult.OK)
                {
                    OnSettingsChanged();
                }
            }
        }

        public void ResetToDefaults()
        {
            var result = MessageBox.Show(
                GetMainWindow(),
                "Are you sure you want to reset all settings to their default values?",
                "Reset Settings",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _settings.ResetToDefaults();
                OnSettingsChanged();
                MessageBox.Show(
                    GetMainWindow(),
                    "Settings have been reset to defaults.",
                    "Settings Reset",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Updates the settings with new values.
        /// </summary>
        public void UpdateSettings(string parentParameterName, string parentCategoryName)
        {
            _settings.ParentParameterName = parentParameterName;
            _settings.ParentCategoryName = parentCategoryName;
            OnSettingsChanged();
        }

        protected virtual void OnSettingsChanged()
        {
            SettingsChanged?.Invoke(this, EventArgs.Empty);
        }

        private IWin32Window GetMainWindow()
        {
            try
            {
                return NwApplication.Gui?.MainWindow;
            }
            catch
            {
                return null;
            }
        }
    }
}
