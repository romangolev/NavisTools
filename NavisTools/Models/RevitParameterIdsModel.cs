using System.Collections.Generic;
using System.Linq;

namespace NavisTools.Models
{
    /// <summary>
    /// Semantic classification of Revit built-in parameters by their meaning.
    /// </summary>
    public enum RevitParameterType
    {
        Unknown,
        Area,
        Volume,
        Height,
        Length,
        Width
    }

    /// <summary>
    /// Model for Revit parameter identification using internal parameter IDs.
    /// Revit parameters are stored in Navisworks with internal names like "lcldrevit_parameter_-1012805".
    /// This model provides stable allow-lists for semantic classification without locale dependency.
    /// </summary>
    public class RevitParameterIdsModel
    {
        #region Constants
        /// <summary>
        /// Prefix used by Navisworks for Revit parameter internal names.
        /// </summary>
        public const string InternalNamePrefix = "lcldrevit_parameter_";
        #endregion

        #region Area Parameters
        /// <summary>
        /// HashSet of all Revit built-in parameter IDs that represent Area.
        /// </summary>
        public static readonly HashSet<int> AreaParams = new HashSet<int>
        {
            -1001133,   // DPART_AREA_COMPUTED
            -1012805,   // HOST_AREA_COMPUTED
            -1155234,   // LAYER_ELEM_AREA_COMPUTED
            -1012010,   // LEVEL_DATA_FLOOR_AREA
            -1012011,   // LEVEL_DATA_SURFACE_AREA
            -1012004,   // MASS_GROSS_AREA
            -1012006,   // MASS_GROSS_SURFACE_AREA
            -1012025,   // MASS_ZONE_FLOOR_AREA
            -1012600,   // PROPERTY_AREA
            -1012606,   // PROPERTY_AREA_OPEN
            -1140360,   // MATERIAL_AREA
            -1155139,   // STEEL_ELEM_PLATE_AREA
        };
        #endregion

        #region Volume Parameters
        /// <summary>
        /// HashSet of all Revit built-in parameter IDs that represent Volume.
        /// </summary>
        public static readonly HashSet<int> VolumeParams = new HashSet<int>
        {
            -1001129,   // DPART_VOLUME_COMPUTED
            -1012806,   // HOST_VOLUME_COMPUTED
            -1155232,   // LAYER_ELEM_VOLUME_COMPUTED
            -1012012,   // LEVEL_DATA_VOLUME
            -1012007,   // MASS_GROSS_VOLUME
            -1012021,   // MASS_ZONE_VOLUME
            -1140361,   // MATERIAL_VOLUME
            -1180306,   // EXCAVATION_VOLUME
            -1180308,   // EXCAVATION_VOLUME_ON_TOPOSOLID
            -1018502,   // REIN_EST_BAR_VOLUME
            -1018503,   // REINFORCEMENT_VOLUME
            -1155140,   // STEEL_ELEM_PLATE_VOLUME
        };
        #endregion

        #region Height Parameters
        /// <summary>
        /// HashSet of all Revit built-in parameter IDs that represent Height.
        /// </summary>
        public static readonly HashSet<int> HeightParams = new HashSet<int>
        {
            -1001135,   // DPART_HEIGHT_COMPUTED
            -1001105,   // WALL_USER_HEIGHT_PARAM
            -1001300,   // WINDOW_HEIGHT / DOOR_HEIGHT
            -1001362,   // INSTANCE_HEAD_HEIGHT_PARAM
            -1006940,   // LEVEL_ATTR_ROOM_COMPUTATION_HEIGHT
            -1006939,   // LEVEL_ROOM_COMPUTATION_HEIGHT
            -1150331,   // RAILING_SYSTEM_HANDRAILS_HEIGHT_PARAM
            -1150335,   // RAILING_SYSTEM_SECONDARY_HANDRAILS_HEIGHT_PARAM
            -1150328,   // RAILING_SYSTEM_TOP_RAIL_HEIGHT_PARAM
            -1005193,   // RENDER_PLANT_HEIGHT
        };
        #endregion

        #region Length Parameters
        /// <summary>
        /// HashSet of all Revit built-in parameter IDs that represent Length.
        /// </summary>
        public static readonly HashSet<int> LengthParams = new HashSet<int>
        {
            -1001136,   // DPART_LENGTH_COMPUTED
            -1004005,   // CURVE_ELEM_LENGTH
            -1001375,   // INSTANCE_LENGTH_PARAM
            -1155247,   // LINEAR_FRAMING_LENGTH
            -1018705,   // LEADER_LENGTH
            -1133752,   // LEGEND_COMPONENT_LENGTH
            -1140132,   // RBS_CABLETRAYCONDUITRUN_LENGTH_PARAM
            -1008303,   // RAMP_MAX_RUN_LENGTH
            -1155137,   // STEEL_ELEM_PLATE_LENGTH
            -1155147,   // STEEL_ELEM_PROFILE_LENGTH
            -1017608,   // FABRIC_SHEET_LENGTH
        };
        #endregion

        #region Width Parameters
        /// <summary>
        /// HashSet of all Revit built-in parameter IDs that represent Width.
        /// </summary>
        public static readonly HashSet<int> WidthParams = new HashSet<int>
        {
            -1001301,   // WINDOW_WIDTH / DOOR_WIDTH
            -1007750,   // RASTER_SHEETWIDTH
            -1007765,   // RASTER_SYMBOL_WIDTH
            -1140122,   // RBS_CABLETRAY_WIDTH_PARAM
            -1140134,   // RBS_CABLETRAYRUN_WIDTH_PARAM
            -1005502,   // STRUCTURAL_SECTION_COMMON_WIDTH
            -1007300,   // RECT_MULLION_WIDTH1
            -1007301,   // RECT_MULLION_WIDTH2
            -1155138,   // STEEL_ELEM_PLATE_WIDTH
        };
        #endregion

        #region Generated Internal Names
        /// <summary>
        /// All Area parameter internal names for Navisworks lookup.
        /// </summary>
        public static readonly HashSet<string> AreaInternalNames = new HashSet<string>(
            AreaParams.Select(id => ToInternalName(id))
        );

        /// <summary>
        /// All Volume parameter internal names for Navisworks lookup.
        /// </summary>
        public static readonly HashSet<string> VolumeInternalNames = new HashSet<string>(
            VolumeParams.Select(id => ToInternalName(id))
        );

        /// <summary>
        /// All Height parameter internal names for Navisworks lookup.
        /// </summary>
        public static readonly HashSet<string> HeightInternalNames = new HashSet<string>(
            HeightParams.Select(id => ToInternalName(id))
        );

        /// <summary>
        /// All Length parameter internal names for Navisworks lookup.
        /// </summary>
        public static readonly HashSet<string> LengthInternalNames = new HashSet<string>(
            LengthParams.Select(id => ToInternalName(id))
        );

        /// <summary>
        /// All Width parameter internal names for Navisworks lookup.
        /// </summary>
        public static readonly HashSet<string> WidthInternalNames = new HashSet<string>(
            WidthParams.Select(id => ToInternalName(id))
        );
        #endregion

        #region Helper Methods
        /// <summary>
        /// Converts a Revit parameter ID to the Navisworks internal name format.
        /// </summary>
        /// <param name="parameterId">The Revit built-in parameter ID.</param>
        /// <returns>The internal name string used in Navisworks.</returns>
        public static string ToInternalName(int parameterId)
        {
            return $"{InternalNamePrefix}{parameterId}";
        }

        /// <summary>
        /// Tries to extract the Revit parameter ID from a Navisworks internal name.
        /// </summary>
        /// <param name="internalName">The Navisworks internal property name.</param>
        /// <param name="parameterId">The extracted parameter ID if successful.</param>
        /// <returns>True if extraction was successful, false otherwise.</returns>
        public static bool TryParseParameterId(string internalName, out int parameterId)
        {
            parameterId = 0;
            if (string.IsNullOrEmpty(internalName))
                return false;

            if (!internalName.StartsWith(InternalNamePrefix))
                return false;

            string idPart = internalName.Substring(InternalNamePrefix.Length);
            return int.TryParse(idPart, out parameterId);
        }

        /// <summary>
        /// Checks if the internal name represents a Revit parameter.
        /// </summary>
        public static bool IsRevitParameter(string internalName)
        {
            return !string.IsNullOrEmpty(internalName) && 
                   internalName.StartsWith(InternalNamePrefix);
        }

        /// <summary>
        /// Classifies a Revit parameter ID by its semantic meaning.
        /// </summary>
        /// <param name="parameterId">The Revit built-in parameter ID.</param>
        /// <returns>The semantic type of the parameter.</returns>
        public static RevitParameterType ClassifyParameter(int parameterId)
        {
            if (AreaParams.Contains(parameterId))
                return RevitParameterType.Area;
            if (VolumeParams.Contains(parameterId))
                return RevitParameterType.Volume;
            if (HeightParams.Contains(parameterId))
                return RevitParameterType.Height;
            if (LengthParams.Contains(parameterId))
                return RevitParameterType.Length;
            if (WidthParams.Contains(parameterId))
                return RevitParameterType.Width;

            return RevitParameterType.Unknown;
        }

        /// <summary>
        /// Classifies a Navisworks internal name by its semantic meaning.
        /// </summary>
        /// <param name="internalName">The Navisworks internal property name.</param>
        /// <returns>The semantic type of the parameter.</returns>
        public static RevitParameterType ClassifyInternalName(string internalName)
        {
            if (TryParseParameterId(internalName, out int parameterId))
            {
                return ClassifyParameter(parameterId);
            }
            return RevitParameterType.Unknown;
        }

        /// <summary>
        /// Checks if a parameter ID represents an Area parameter.
        /// </summary>
        public static bool IsAreaParameter(int parameterId) => AreaParams.Contains(parameterId);

        /// <summary>
        /// Checks if a parameter ID represents a Volume parameter.
        /// </summary>
        public static bool IsVolumeParameter(int parameterId) => VolumeParams.Contains(parameterId);

        /// <summary>
        /// Checks if a parameter ID represents a Height parameter.
        /// </summary>
        public static bool IsHeightParameter(int parameterId) => HeightParams.Contains(parameterId);

        /// <summary>
        /// Checks if a parameter ID represents a Length parameter.
        /// </summary>
        public static bool IsLengthParameter(int parameterId) => LengthParams.Contains(parameterId);

        /// <summary>
        /// Checks if a parameter ID represents a Width parameter.
        /// </summary>
        public static bool IsWidthParameter(int parameterId) => WidthParams.Contains(parameterId);

        /// <summary>
        /// Checks if an internal name represents an Area parameter.
        /// </summary>
        public static bool IsAreaInternalName(string internalName) => AreaInternalNames.Contains(internalName);

        /// <summary>
        /// Checks if an internal name represents a Volume parameter.
        /// </summary>
        public static bool IsVolumeInternalName(string internalName) => VolumeInternalNames.Contains(internalName);

        /// <summary>
        /// Checks if an internal name represents a Height parameter.
        /// </summary>
        public static bool IsHeightInternalName(string internalName) => HeightInternalNames.Contains(internalName);

        /// <summary>
        /// Checks if an internal name represents a Length parameter.
        /// </summary>
        public static bool IsLengthInternalName(string internalName) => LengthInternalNames.Contains(internalName);

        /// <summary>
        /// Checks if an internal name represents a Width parameter.
        /// </summary>
        public static bool IsWidthInternalName(string internalName) => WidthInternalNames.Contains(internalName);
        #endregion

        #region Singleton
        private static RevitParameterIdsModel _instance;

        /// <summary>
        /// Gets the singleton instance of the model.
        /// </summary>
        public static RevitParameterIdsModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RevitParameterIdsModel();
                }
                return _instance;
            }
        }

        private RevitParameterIdsModel() { }
        #endregion
    }
}
