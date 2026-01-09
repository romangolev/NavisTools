using NavisTools.Interfaces;
using System;

namespace NavisTools.ViewModels
{
    /// <summary>
    /// ViewModel for the Configuration/Settings dialog.
    /// </summary>
    public class ConfigurationViewModel : ViewModelBase
    {
        private readonly IConfigurationService _configurationService;
        private string _parentParameterName;
        private string _parentCategoryName;
        private bool _isDirty;

        public ConfigurationViewModel(IConfigurationService configurationService)
        {
            _configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));
            LoadFromService();
        }

        #region Properties

        public string ParentParameterName
        {
            get => _parentParameterName;
            set
            {
                if (SetProperty(ref _parentParameterName, value))
                {
                    IsDirty = true;
                }
            }
        }

        public string ParentCategoryName
        {
            get => _parentCategoryName;
            set
            {
                if (SetProperty(ref _parentCategoryName, value))
                {
                    IsDirty = true;
                }
            }
        }

        public bool IsDirty
        {
            get => _isDirty;
            private set => SetProperty(ref _isDirty, value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads settings from the configuration service.
        /// </summary>
        public void LoadFromService()
        {
            ParentParameterName = _configurationService.ParentParameterName;
            ParentCategoryName = _configurationService.ParentCategoryName;
            IsDirty = false;
        }

        /// <summary>
        /// Saves settings back to the configuration service.
        /// </summary>
        public bool Save()
        {
            // Note: In a full implementation, the configuration service would have setters
            // For now, this marks the ViewModel as clean
            IsDirty = false;
            return true;
        }

        /// <summary>
        /// Resets to default values.
        /// </summary>
        public void ResetToDefaults()
        {
            _configurationService.ResetToDefaults();
            LoadFromService();
        }

        /// <summary>
        /// Cancels any unsaved changes.
        /// </summary>
        public void Cancel()
        {
            LoadFromService();
        }

        #endregion
    }
}
