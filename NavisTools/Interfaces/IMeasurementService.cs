using NavisTools.Models;
using System.Collections.Generic;

namespace NavisTools.Interfaces
{
    /// <summary>
    /// Provides measurement calculations for model items.
    /// </summary>
    public interface IMeasurementService
    {
        /// <summary>
        /// Calculates the total volume of the given items.
        /// </summary>
        double CalculateTotalVolume(IEnumerable<object> items);

        /// <summary>
        /// Calculates the total area of the given items.
        /// </summary>
        double CalculateTotalArea(IEnumerable<object> items);

        /// <summary>
        /// Calculates the total length of the given items.
        /// </summary>
        double CalculateTotalLength(IEnumerable<object> items);

        /// <summary>
        /// Gets measurement info for a single item.
        /// </summary>
        ItemMeasurement GetItemMeasurement(object item);
    }

    /// <summary>
    /// Represents measurement data for a single item.
    /// </summary>
    public class ItemMeasurement
    {
        public string DisplayName { get; set; }
        public double Volume { get; set; }
        public double Area { get; set; }
        public double Length { get; set; }
    }
}
