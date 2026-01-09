using Autodesk.Navisworks.Api;
using NavisTools.Interfaces;
using NavisTools.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace NavisTools.Services
{
    /// <summary>
    /// Implementation of IMeasurementService for calculating measurements from Navisworks items.
    /// Extracted from GetVolume to follow Single Responsibility Principle.
    /// </summary>
    public class MeasurementService : IMeasurementService
    {
        private readonly IDocumentProvider _documentProvider;

        public MeasurementService(IDocumentProvider documentProvider)
        {
            _documentProvider = documentProvider ?? throw new ArgumentNullException(nameof(documentProvider));
        }

        public double CalculateTotalVolume(IEnumerable<object> items)
        {
            return CalculateTotal(items, MeasurementType.Volume);
        }

        public double CalculateTotalArea(IEnumerable<object> items)
        {
            return CalculateTotal(items, MeasurementType.Area);
        }

        public double CalculateTotalLength(IEnumerable<object> items)
        {
            return CalculateTotal(items, MeasurementType.Length);
        }

        public ItemMeasurement GetItemMeasurement(object item)
        {
            if (!(item is ModelItem modelItem))
                return new ItemMeasurement { DisplayName = "Unknown" };

            return new ItemMeasurement
            {
                DisplayName = modelItem.DisplayName ?? "Unnamed",
                Volume = GetMeasurementValue(modelItem, MeasurementType.Volume),
                Area = GetMeasurementValue(modelItem, MeasurementType.Area),
                Length = GetMeasurementValue(modelItem, MeasurementType.Length)
            };
        }

        private double CalculateTotal(IEnumerable<object> items, MeasurementType type)
        {
            double total = 0;
            foreach (var item in items)
            {
                if (item is ModelItem modelItem)
                {
                    total += GetMeasurementValue(modelItem, type);
                }
            }
            return total;
        }

        private double GetMeasurementValue(ModelItem item, MeasurementType type)
        {
            var property = FindPropertyByType(item, type);
            if (property == null)
                return 0;

            return ConvertValue(property, type);
        }

        private DataProperty FindPropertyByType(ModelItem item, MeasurementType type)
        {
            string[] propertyNames;
            switch (type)
            {
                case MeasurementType.Volume:
                    propertyNames = PropertyNamesModel.Instance.VolumeNames;
                    break;
                case MeasurementType.Area:
                    propertyNames = PropertyNamesModel.Instance.AreaNames;
                    break;
                case MeasurementType.Length:
                    propertyNames = PropertyNamesModel.Instance.LengthNames;
                    break;
                default:
                    return null;
            }

            return FindProperty(item, propertyNames);
        }

        private DataProperty FindProperty(ModelItem item, string[] propertyVariations)
        {
            foreach (PropertyCategory category in item.PropertyCategories)
            {
                var propDict = new Dictionary<string, DataProperty>();
                foreach (DataProperty p in category.Properties)
                {
                    propDict[p.DisplayName] = p;
                }

                foreach (string variation in propertyVariations)
                {
                    if (propDict.TryGetValue(variation, out DataProperty prop))
                    {
                        return prop;
                    }
                }
            }

            return null;
        }

        private double ConvertValue(DataProperty prop, MeasurementType type)
        {
            var doc = _documentProvider.ActiveDocument;
            if (doc == null)
                return 0;

            double result;

            switch (type)
            {
                case MeasurementType.Volume:
                    try
                    {
                        double volumeInDocUnits = prop.Value.ToDoubleVolume();
                        double k = UnitConversion.ScaleFactor(doc.Units, Units.Meters);
                        result = Math.Round(volumeInDocUnits * k * k * k, 3);
                    }
                    catch
                    {
                        TryParseDisplayString(prop.Value, 3, out result);
                    }
                    break;

                case MeasurementType.Area:
                    try
                    {
                        double areaInDocUnits = prop.Value.ToDoubleArea();
                        double k = UnitConversion.ScaleFactor(doc.Units, Units.Meters);
                        result = Math.Round(areaInDocUnits * k * k, 2);
                    }
                    catch
                    {
                        TryParseDisplayString(prop.Value, 2, out result);
                    }
                    break;

                case MeasurementType.Length:
                    try
                    {
                        double lengthInDocUnits = prop.Value.ToDoubleLength();
                        double k = UnitConversion.ScaleFactor(doc.Units, Units.Meters);
                        result = Math.Round(lengthInDocUnits * k, 2);
                    }
                    catch
                    {
                        TryParseDisplayString(prop.Value, 2, out result);
                    }
                    break;

                default:
                    result = 0;
                    break;
            }

            return result;
        }

        private bool TryParseDisplayString(VariantData value, int decimalPlaces, out double result)
        {
            try
            {
                string displayStr = value.ToDisplayString();
                string numericStr = displayStr.Split(' ')[0].Replace(",", ".");
                if (double.TryParse(numericStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedValue))
                {
                    result = Math.Round(parsedValue, decimalPlaces);
                    return true;
                }
            }
            catch
            {
                // Ignore parsing errors
            }

            result = 0.0;
            return false;
        }

        private enum MeasurementType
        {
            Volume,
            Area,
            Length
        }
    }
}
