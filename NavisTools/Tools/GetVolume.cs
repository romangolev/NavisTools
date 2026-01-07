using Autodesk.Navisworks.Api;
using NavisTools.Models;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Application = Autodesk.Navisworks.Api.Application;

namespace NavisTools.Tools
{
    public enum MeasurementUnit
    {
        Volume,
        Area,
        Length
    }

    public enum NavPropertyCategory
    {
        Element,
        Object,
        Material,
        RevitMaterial,
        AutodeskMaterial,
        Dimensions,
        Identity
    }

    public class GetVolume
    {
		public static void ExecuteVolumeCommand()
		{
			var totalVolume = GetParameter(MeasurementUnit.Volume);

            foreach (ModelItem selectedItem in Application.ActiveDocument.CurrentSelection.SelectedItems)
            {
				PropertyCategoryCollection categories = selectedItem.GetUserFilteredPropertyCategories();
				foreach (PropertyCategory category in categories)
				{
					string catInternal = category.Name;
                    string catDisplay = category.DisplayName;

					foreach (DataProperty prop in category.Properties)
					{
						string propInternal = prop.Name;
                        string propDisplay = prop.DisplayName;
						string combined = prop.CombinedName.ToString();

						VariantData value = prop.Value;

						System.Diagnostics.Debug.WriteLine(
							$"{catInternal} ({catDisplay}) :: " +
							$"{propInternal} ({propDisplay}) = {value}"
						);
                        if (propInternal == "lcldrevit_parameter_-1001133")
                        {
							System.Diagnostics.Debug.WriteLine(value.IsDoubleArea.ToString());
							System.Diagnostics.Debug.WriteLine(
								$"{catInternal} ({catDisplay}) :: " +
								$"{propInternal} ({propDisplay}) = {ConvertValue(prop, MeasurementUnit.Area)}"
							);
						}
                        if (propInternal == "lcldrevit_parameter_-1001129")
                        {
							System.Diagnostics.Debug.WriteLine(value.IsDoubleVolume.ToString());
							System.Diagnostics.Debug.WriteLine(
								$"{catInternal} ({catDisplay}) :: " +
								$"{propInternal} ({propDisplay}) = {ConvertValue(prop, MeasurementUnit.Volume)}"
							);
						}
					}
				}
            }

			ShowMyDialog(
				"Selected Total Volume",
				totalVolume.ToString(),
				"Volume = \n",
				"м3"
				);
		}

		public static void ExecuteAreaCommand()
		{
			var totalArea = GetParameter(MeasurementUnit.Area);
			ShowMyDialog(
				"Selected Total Area",
				totalArea.ToString(),
				"Area = \n",
				"м2"
				);
		}

		public static void ExecuteLengthCommand()
		{
			var totalLength = GetParameter(MeasurementUnit.Length);
			ShowMyDialog(
				"Selected Total Length",
				totalLength.ToString(),
					"Length = \n",
						"м"
						);
				}

				public static void ExecuteCountCommand()
				{

					int num = 0;
					ModelItemCollection modelItemCollection = new ModelItemCollection();
					foreach (ModelItem selectedItem in Application.ActiveDocument.CurrentSelection.SelectedItems)
					{
						++num;
						modelItemCollection.Add(selectedItem);
					}
					Selection selection = new Selection(modelItemCollection);
					Application.ActiveDocument.CurrentSelection.Clear();
					Application.ActiveDocument.CurrentSelection.CopyFrom(selection);

					ShowMyDialog(
						"Selected Count",
						num.ToString(),
						"Count = \n",
						"pcs."
						);
				}

				/// <summary>
				/// Executes the Total Sums command showing Volume, Area, Length and Count
				/// </summary>
				public static void ExecuteTotalSumsCommand()
				{
					var totalVolume = GetParameter(MeasurementUnit.Volume);
					var totalArea = GetParameter(MeasurementUnit.Area);
					var totalLength = GetParameter(MeasurementUnit.Length);

					int count = 0;
					foreach (ModelItem selectedItem in Application.ActiveDocument.CurrentSelection.SelectedItems)
					{
						++count;
					}

					ShowTotalSumsPanel(totalVolume, totalArea, totalLength, count);
				}

				/// <summary>
				/// Shows a panel with all totals
				/// </summary>
				private static void ShowTotalSumsPanel(double volume, double area, double length, int count)
				{
					Form panel = new Form();
					panel.Text = "Total Sums";
					panel.Size = new Size(320, 220);
					panel.FormBorderStyle = FormBorderStyle.FixedDialog;
					panel.MaximizeBox = false;
					panel.MinimizeBox = false;
					panel.StartPosition = FormStartPosition.CenterParent;

					int yPos = 20;
					int labelWidth = 100;
					int valueWidth = 180;

					// Volume
					Label volumeLabel = new Label();
					volumeLabel.Text = "Volume:";
					volumeLabel.Location = new Point(15, yPos);
					volumeLabel.Size = new Size(labelWidth, 20);
					volumeLabel.Font = new Font(volumeLabel.Font, FontStyle.Bold);
					panel.Controls.Add(volumeLabel);

					Label volumeValue = new Label();
					volumeValue.Text = $"{volume:N3} м³";
					volumeValue.Location = new Point(120, yPos);
					volumeValue.Size = new Size(valueWidth, 20);
					panel.Controls.Add(volumeValue);

					yPos += 30;

					// Area
					Label areaLabel = new Label();
					areaLabel.Text = "Area:";
					areaLabel.Location = new Point(15, yPos);
					areaLabel.Size = new Size(labelWidth, 20);
					areaLabel.Font = new Font(areaLabel.Font, FontStyle.Bold);
					panel.Controls.Add(areaLabel);

					Label areaValue = new Label();
					areaValue.Text = $"{area:N2} м²";
					areaValue.Location = new Point(120, yPos);
					areaValue.Size = new Size(valueWidth, 20);
					panel.Controls.Add(areaValue);

					yPos += 30;

					// Length
					Label lengthLabel = new Label();
					lengthLabel.Text = "Length:";
					lengthLabel.Location = new Point(15, yPos);
					lengthLabel.Size = new Size(labelWidth, 20);
					lengthLabel.Font = new Font(lengthLabel.Font, FontStyle.Bold);
					panel.Controls.Add(lengthLabel);

					Label lengthValue = new Label();
					lengthValue.Text = $"{length:N2} м";
					lengthValue.Location = new Point(120, yPos);
					lengthValue.Size = new Size(valueWidth, 20);
					panel.Controls.Add(lengthValue);

					yPos += 30;

					// Count
					Label countLabel = new Label();
					countLabel.Text = "Count:";
					countLabel.Location = new Point(15, yPos);
					countLabel.Size = new Size(labelWidth, 20);
					countLabel.Font = new Font(countLabel.Font, FontStyle.Bold);
					panel.Controls.Add(countLabel);

					Label countValue = new Label();
					countValue.Text = $"{count} pcs.";
					countValue.Location = new Point(120, yPos);
					countValue.Size = new Size(valueWidth, 20);
					panel.Controls.Add(countValue);

					yPos += 40;

					// OK Button
					Button okButton = new Button();
					okButton.Text = "OK";
					okButton.Location = new Point(115, yPos);
					okButton.Size = new Size(75, 25);
					okButton.DialogResult = DialogResult.OK;
					panel.Controls.Add(okButton);
					panel.AcceptButton = okButton;

					panel.ShowDialog(Application.Gui.MainWindow);
				}

				/// <summary>
				/// Efficiently find property by searching across all relevant categories with O(1) lookups
				/// </summary>
				private static DataProperty FindProperty(ModelItem item, string[] propertyVariations)
        {
            // Search across all categories
            foreach (PropertyCategory category in item.PropertyCategories)
            {
                // Convert properties to dictionary for O(1) lookup
                var propDict = new System.Collections.Generic.Dictionary<string, DataProperty>();
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

        private static DataProperty FindVolumeProperty(ModelItem item)
        {
            return FindProperty(item, PropertyNamesModel.Instance.VolumeNames);
        }

        private static DataProperty FindAreaProperty(ModelItem item)
        {
            return FindProperty(item, PropertyNamesModel.Instance.AreaNames);
        }

        private static DataProperty FindLengthProperty(ModelItem item)
        {
            return FindProperty(item, PropertyNamesModel.Instance.LengthNames);
        }

        /// <summary>
        /// Fallback method to parse numeric value from display string
        /// </summary>
        private static bool TryParseDisplayString(VariantData value, int decimalPlaces, out double result)
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

        private static double ConvertValue(DataProperty prop, MeasurementUnit unit)
        {
            double result;
            Document doc = Application.ActiveDocument;

            switch (unit)
            {
                case MeasurementUnit.Volume:
                    try
                    {
                        // Get volume in document units and convert to cubic meters
                        double volumeInDocUnits = prop.Value.ToDoubleVolume();
                        double k = UnitConversion.ScaleFactor(doc.Units, Units.Meters);
                        result = Math.Round(volumeInDocUnits * k * k * k, 3);
                    }
                    catch
                    {
                        // Fallback: try parsing display string (e.g., "4.652 m³")
                        TryParseDisplayString(prop.Value, 3, out result);
                    }
                    break;
                case MeasurementUnit.Area:
                    try
                    {
                        // Get area in document units and convert to square meters
                        double areaInDocUnits = prop.Value.ToDoubleArea();
                        double k = UnitConversion.ScaleFactor(doc.Units, Units.Meters);
                        result = Math.Round(areaInDocUnits * k * k, 2);
                    }
                    catch
                    {
                        // Fallback: try parsing display string (e.g., "46.520 m²")
                        TryParseDisplayString(prop.Value, 2, out result);
                    }
                    break;
                case MeasurementUnit.Length:
                    try
                    {
                        // Get length in document units and convert to meters
                        double lengthInDocUnits = prop.Value.ToDoubleLength();
                        double k = UnitConversion.ScaleFactor(doc.Units, Units.Meters);
                        result = Math.Round(lengthInDocUnits * k, 2);
                    }
                    catch
                    {
                        // Fallback: try parsing display string (e.g., "21.700 m")
                        TryParseDisplayString(prop.Value, 2, out result);
                    }
                    break;
                default:
                    result = 0.0;
                    break;
            }
            return result;
        }

        /// <summary>
        /// Helper method to find property based on unit type
        /// </summary>
        private static DataProperty FindPropertyByUnit(ModelItem item, MeasurementUnit unit)
        {
            switch (unit)
            {
                case MeasurementUnit.Volume:
                    return FindVolumeProperty(item);
                case MeasurementUnit.Area:
                    return FindAreaProperty(item);
                case MeasurementUnit.Length:
                    return FindLengthProperty(item);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Recursively search for properties in item and its descendants
        /// </summary>
        private static bool TryCollectParameterFromHierarchy(ModelItem item, MeasurementUnit unit, 
            ref double parameter, ModelItemCollection resultCollection)
        {
            // Try to find property on current item
            DataProperty prop = FindPropertyByUnit(item, unit);

            if (prop != null)
            {
                double value = ConvertValue(prop, unit);
                if (value > 0)
                {
                    parameter += value;
                    resultCollection.Add(item);
                    return true;
                }
            }

            // If not found on current item, search children recursively
            bool foundInChildren = false;
            foreach (ModelItem child in item.Children)
            {
                if (TryCollectParameterFromHierarchy(child, unit, ref parameter, resultCollection))
                {
                    foundInChildren = true;
                }
            }

            return foundInChildren;
        }

        private static double GetParameter(MeasurementUnit unit)
        {
            double parameter = 0.0;
            ModelItemCollection modelItemCollection = new ModelItemCollection();

            foreach (ModelItem selectedItem in Application.ActiveDocument.CurrentSelection.SelectedItems)
            {
                // Try current item and its descendants
                bool foundInHierarchy = TryCollectParameterFromHierarchy(
                    selectedItem, unit, ref parameter, modelItemCollection);

                // If not found in item or children, try parent
                if (!foundInHierarchy && selectedItem.Parent != null)
                {
                    DataProperty parentProp = FindPropertyByUnit(selectedItem.Parent, unit);

                    if (parentProp != null)
                    {
                        double value = ConvertValue(parentProp, unit);
                        if (value > 0)
                        {
                            parameter += value;
                            modelItemCollection.Add(selectedItem.Parent);
                        }
                    }
                }
            }

            // Update selection with items that had valid properties
            Selection selection = new Selection(modelItemCollection);
            Application.ActiveDocument.CurrentSelection.Clear();
            Application.ActiveDocument.CurrentSelection.CopyFrom(selection);

            return parameter;
        }

        private static void ShowMyDialog(string title, string text, string text1, string text2)
        {
            Form form1 = new Form();
            form1.Text = title;
            form1.Size = new Size(400, 200);
            Form form2 = form1;
            Control.ControlCollection controls1 = form2.Controls;
            Label label1 = new Label();
            label1.Text = text1;
            label1.Size = new Size(80, 30);
            label1.Location = new Point(50, 10);
            controls1.Add((Control)label1);
            Control.ControlCollection controls2 = form2.Controls;
            TextBox textBox = new TextBox();
            textBox.Text = text;
            textBox.Size = new Size(120, 30);
            textBox.Location = new Point(140, 10);
            controls2.Add((Control)textBox);
            Control.ControlCollection controls3 = form2.Controls;
            Label label2 = new Label();
            label2.Text = text2;
            label2.Size = new Size(60, 30);
            label2.Location = new Point(270, 10);
            controls3.Add((Control)label2);
            int num = (int)form2.ShowDialog();
            form2.Controls.OfType<TextBox>().First<TextBox>().Dispose();
            form2.Dispose();
        }
    }
}
