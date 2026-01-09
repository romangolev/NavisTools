namespace NavisTools.Models
{
    /// <summary>
    /// Defines how property values are looked up in model items.
    /// </summary>
    public enum PropertyLookupMode
    {
        /// <summary>
        /// Search by display name (e.g., "Volume", "Объем", "Area").
        /// Works with any source but depends on locale.
        /// </summary>
        ByDisplayName = 0,

        /// <summary>
        /// Search by Revit built-in parameter IDs (e.g., lcldrevit_parameter_-1012806).
        /// Locale-independent but only works with Revit models.
        /// </summary>
        ByRevitParameterId = 1
    }

    /// <summary>
    /// Model class for application settings.
    /// Pure data class with no dependencies.
    /// </summary>
    public class SettingsModel
    {
        public string ParentParameterName { get; set; }
        public string ParentCategoryName { get; set; }

        /// <summary>
        /// Defines how properties (Volume, Area, Length, etc.) are looked up.
        /// </summary>
        public PropertyLookupMode LookupMode { get; set; }

        public SettingsModel()
        {
            ResetToDefaults();
        }

        public void ResetToDefaults()
        {
            ParentParameterName = "ParentName";
            ParentCategoryName = "Item";
            LookupMode = PropertyLookupMode.ByDisplayName;
        }

        /// <summary>
        /// Creates a copy of this settings model.
        /// </summary>
        public SettingsModel Clone()
        {
            return new SettingsModel
            {
                ParentParameterName = this.ParentParameterName,
                ParentCategoryName = this.ParentCategoryName,
                LookupMode = this.LookupMode
            };
        }
    }
}
