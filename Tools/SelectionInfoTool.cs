using Autodesk.Navisworks.Api;
using NavisTools.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace NavisTools.Tools
{
    /// <summary>
    /// Tool for extracting and calculating selection information from Navisworks documents
    /// </summary>
    public static class SelectionInfoTool
    {
        /// <summary>
        /// Gets comprehensive selection information from the active document
        /// </summary>
        /// <returns>SelectionInfo object containing all calculated data</returns>
        public static SelectionInfo GetSelectionInfo()
        {
            var info = new SelectionInfo();
            var doc = Autodesk.Navisworks.Api.Application.ActiveDocument;

            if (doc == null)
            {
                info.Summary = "No document open";
                return info;
            }

            var selection = doc.CurrentSelection.SelectedItems;
            info.Count = selection.Count;

            if (info.Count == 0)
            {
                info.Summary = "No items selected";
                return info;
            }

            foreach (ModelItem item in selection)
            {
                var itemInfo = new SelectionItemInfo
                {
                    DisplayName = item.DisplayName ?? "Unnamed"
                };

                // Find and convert volume
                var volumeProp = FindProperty(item, PropertyNamesModel.Instance.VolumeNames);
                if (volumeProp != null)
                {
                    itemInfo.Volume = ConvertVolume(volumeProp, doc);
                    info.TotalVolume += itemInfo.Volume;
                }

                // Find and convert area
                var areaProp = FindProperty(item, PropertyNamesModel.Instance.AreaNames);
                if (areaProp != null)
                {
                    itemInfo.Area = ConvertArea(areaProp, doc);
                    info.TotalArea += itemInfo.Area;
                }

                // Find and convert length
                var lengthProp = FindProperty(item, PropertyNamesModel.Instance.LengthNames);
                if (lengthProp != null)
                {
                    itemInfo.Length = ConvertLength(lengthProp, doc);
                    info.TotalLength += itemInfo.Length;
                }

                info.Items.Add(itemInfo);
            }

            info.Summary = $"{info.Count} item(s) selected";
            return info;
        }

        /// <summary>
        /// Finds a property by checking multiple name variations
        /// </summary>
        private static DataProperty FindProperty(ModelItem item, string[] propertyVariations)
        {
            foreach (PropertyCategory category in item.PropertyCategories)
            {
                foreach (DataProperty prop in category.Properties)
                {
                    foreach (string variation in propertyVariations)
                    {
                        if (string.Equals(prop.DisplayName, variation, StringComparison.OrdinalIgnoreCase))
                        {
                            return prop;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Converts volume property to cubic meters
        /// </summary>
        private static double ConvertVolume(DataProperty prop, Document doc)
        {
            try
            {
                double volumeInDocUnits = prop.Value.ToDoubleVolume();
                double k = UnitConversion.ScaleFactor(doc.Units, Units.Meters);
                return Math.Round(volumeInDocUnits * k * k * k, 3);
            }
            catch
            {
                return TryParseDisplayString(prop.Value, 3);
            }
        }

        /// <summary>
        /// Converts area property to square meters
        /// </summary>
        private static double ConvertArea(DataProperty prop, Document doc)
        {
            try
            {
                double areaInDocUnits = prop.Value.ToDoubleArea();
                double k = UnitConversion.ScaleFactor(doc.Units, Units.Meters);
                return Math.Round(areaInDocUnits * k * k, 2);
            }
            catch
            {
                return TryParseDisplayString(prop.Value, 2);
            }
        }

        /// <summary>
        /// Converts length property to meters
        /// </summary>
        private static double ConvertLength(DataProperty prop, Document doc)
        {
            try
            {
                double lengthInDocUnits = prop.Value.ToDoubleLength();
                double k = UnitConversion.ScaleFactor(doc.Units, Units.Meters);
                return Math.Round(lengthInDocUnits * k, 2);
            }
            catch
            {
                return TryParseDisplayString(prop.Value, 2);
            }
        }

        /// <summary>
        /// Fallback method to parse numeric value from display string
        /// </summary>
        private static double TryParseDisplayString(VariantData value, int decimalPlaces)
        {
            try
            {
                string displayStr = value.ToDisplayString();
                string numericStr = displayStr.Split(' ')[0].Replace(",", ".");
                if (double.TryParse(numericStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedValue))
                {
                    return Math.Round(parsedValue, decimalPlaces);
                }
            }
            catch
            {
                // Ignore parsing errors
            }
            return 0.0;
        }
    }

    /// <summary>
    /// Container for selection information
    /// </summary>
    public class SelectionInfo
    {
        public string Summary { get; set; }
        public int Count { get; set; }
        public double TotalVolume { get; set; }
        public double TotalArea { get; set; }
        public double TotalLength { get; set; }
        public List<SelectionItemInfo> Items { get; set; }

        public SelectionInfo()
        {
            Summary = string.Empty;
            Count = 0;
            TotalVolume = 0;
            TotalArea = 0;
            TotalLength = 0;
            Items = new List<SelectionItemInfo>();
        }
    }

    /// <summary>
    /// Information about a single selected item
    /// </summary>
    public class SelectionItemInfo
    {
        public string DisplayName { get; set; }
        public double Volume { get; set; }
        public double Area { get; set; }
        public double Length { get; set; }
    }
}
