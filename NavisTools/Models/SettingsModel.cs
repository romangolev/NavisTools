namespace NavisTools.Models
{
    /// <summary>
    /// Model class for application settings.
    /// Pure data class with no dependencies.
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

        /// <summary>
        /// Creates a copy of this settings model.
        /// </summary>
        public SettingsModel Clone()
        {
            return new SettingsModel
            {
                ParentParameterName = this.ParentParameterName,
                ParentCategoryName = this.ParentCategoryName
            };
        }
    }
}
