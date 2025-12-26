using Autodesk.Navisworks.Api;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Application = Autodesk.Navisworks.Api.Application;

namespace NavisTools.Tools
{
    public class GetVolume
    {
        public static void ExecuteVolumeCommand()
        {
            ShowMyDialog("Объем выборки", tostring(get_parameter("Объект", "Объем", "м3")), "Объем = \n", "м3");
        }

        public static void ExecuteAreaCommand()
        {
            ShowMyDialog("Площадь выборки", tostring(get_parameter("Объект", "Площадь", "м2")), "Площадь = \n", "м2");
        }

        public static void ExecuteLengthCommand()
        {
            ShowMyDialog("Длина выборки", tostring(get_parameter("Объект", "Длина", "м")), "Длина = \n", "м");
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
            ShowMyDialog("Количество", num.ToString(), "Кол-во = \n", "шт.");
        }

        private static DataProperty FindGeneralProperty(ModelItem item, string category, string propertyName)
        {
            DataProperty prop = item.PropertyCategories.FindPropertyByDisplayName(category, propertyName);
            if (prop != (NativeHandle)null)
                return prop;

            string[] variations = new string[]
            {
                propertyName,
                "Длина стержня",
                "Количество",
                "Count",
                "Quantity"
            };

            foreach (string variation in variations)
            {
                prop = item.PropertyCategories.FindPropertyByDisplayName(category, variation);
                if (prop != (NativeHandle)null)
                    return prop;
            }

            return null;
        }

        private static DataProperty FindVolumeProperty(ModelItem item, string category, string propertyName)
        {
            // Try the exact property name in the specified category first
            DataProperty prop = item.PropertyCategories.FindPropertyByDisplayName(category, propertyName);
            if (prop != (NativeHandle)null)
                return prop;

            string[] variations = new string[]
            {
                "Объем",
                "Volume",
                propertyName + " (м3)",
                "Объем (м3)",
                "Volume (m³)"
            };

            // Search in the specified category
            foreach (string variation in variations)
            {
                prop = item.PropertyCategories.FindPropertyByDisplayName(category, variation);
                if (prop != (NativeHandle)null)
                    return prop;
            }

            // If not found, search across ALL categories
            foreach (PropertyCategory propCategory in item.PropertyCategories)
            {
                foreach (string variation in variations)
                {
                    prop = propCategory.Properties.FirstOrDefault(p => p.DisplayName == variation);
                    if (prop != null)
                        return prop;
                }
            }

            return null;
        }

        private static DataProperty FindAreaProperty(ModelItem item, string category, string propertyName)
        {
            // Try the exact property name in the specified category first
            DataProperty prop = item.PropertyCategories.FindPropertyByDisplayName(category, propertyName);
            if (prop != (NativeHandle)null)
                return prop;

            string[] variations = new string[]
            {
                "Площадь",
                "Area",
                propertyName + " (м2)",
                "Площадь (м2)",
                "Area (m²)"
            };

            // Search in the specified category
            foreach (string variation in variations)
            {
                prop = item.PropertyCategories.FindPropertyByDisplayName(category, variation);
                if (prop != (NativeHandle)null)
                    return prop;
            }

            // If not found, search across ALL categories
            foreach (PropertyCategory propCategory in item.PropertyCategories)
            {
                foreach (string variation in variations)
                {
                    prop = propCategory.Properties.FirstOrDefault(p => p.DisplayName == variation);
                    if (prop != null)
                        return prop;
                }
            }

            return null;
        }

        private static DataProperty FindLengthProperty(ModelItem item, string category, string propertyName)
        {
            // Try the exact property name in the specified category first
            DataProperty prop = item.PropertyCategories.FindPropertyByDisplayName(category, propertyName);
            if (prop != (NativeHandle)null)
                return prop;

            string[] variations = new string[]
            {
                "Длина",
                "Length",
                propertyName + " (м)",
                "Длина (м)",
                "Length (m)"
            };

            // Search in the specified category
            foreach (string variation in variations)
            {
                prop = item.PropertyCategories.FindPropertyByDisplayName(category, variation);
                if (prop != (NativeHandle)null)
                    return prop;
            }

            // If not found, search across ALL categories
            foreach (PropertyCategory propCategory in item.PropertyCategories)
            {
                foreach (string variation in variations)
                {
                    prop = propCategory.Properties.FirstOrDefault(p => p.DisplayName == variation);
                    if (prop != null)
                        return prop;
                }
            }

            return null;
        }

        private static double get_v2(DataProperty prop, string unit)
        {
            double v2;
            switch (unit)
            {
                case "м3":
                    try
                    {
                        v2 = Math.Round(prop.Value.ToDoubleVolume() * 0.3048 * 0.3048 * 0.3048, 3);
                    }
                    catch
                    {
                        try
                        {
                            string displayStr = prop.Value.ToDisplayString();
                            string numericStr = displayStr.Split(' ')[0].Replace(",", ".");
                            if (double.TryParse(numericStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedValue))
                            {
                                v2 = Math.Round(parsedValue, 3);
                            }
                            else
                            {
                                v2 = 0.0;
                            }
                        }
                        catch
                        {
                            v2 = 0.0;
                        }
                    }
                    break;
                case "м2":
                    try
                    {
                        v2 = Math.Round(prop.Value.ToDoubleArea() * 0.3048 * 0.3048, 2);
                    }
                    catch
                    {
                        try
                        {
                            string displayStr = prop.Value.ToDisplayString();
                            string numericStr = displayStr.Split(' ')[0].Replace(",", ".");
                            if (double.TryParse(numericStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedValue))
                            {
                                v2 = Math.Round(parsedValue, 2);
                            }
                            else
                            {
                                v2 = 0.0;
                            }
                        }
                        catch
                        {
                            v2 = 0.0;
                        }
                    }
                    break;
                case "м":
                    try
                    {
                        v2 = Math.Round(prop.Value.ToDoubleLength() * 0.3048, 2);
                    }
                    catch
                    {
                        try
                        {
                            string displayStr = prop.Value.ToDisplayString();
                            string numericStr = displayStr.Split(' ')[0].Replace(",", ".");
                            if (double.TryParse(numericStr, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsedValue))
                            {
                                v2 = Math.Round(parsedValue, 2);
                            }
                            else
                            {
                                v2 = 0.0;
                            }
                        }
                        catch
                        {
                            v2 = 0.0;
                        }
                    }
                    break;
                case "кг":
                    try
                    {
                        v2 = Math.Round(double.Parse(prop.Value.ToDisplayString().Remove(prop.Value.ToDisplayString().Length - 4).Replace(",", ".")) * 0.00785, 2);
                        break;
                    }
                    catch
                    {
                        try
                        {
                            v2 = Math.Round(double.Parse(prop.Value.ToDisplayString().Remove(prop.Value.ToDisplayString().Length - 4).Replace(".", ",")) * 0.00785, 2);
                        }
                        catch
                        {
                            v2 = 0.0;
                        }
                        break;
                    }
                case "кг2":
                    try
                    {
                        v2 = Math.Round(prop.Value.ToDouble(), 2);
                    }
                    catch
                    {
                        v2 = 0.0;
                    }
                    break;
                default:
                    v2 = 0.0;
                    break;
            }
            return v2;
        }

        private static double get_parameter(string cat, string par, string unit)
        {
            double parameter = 0.0;
            double num1 = 1.7;
            if (unit == "кг")
            {
                string str = "";
                if (InputBox("Введите длину нахлеста", "Длина нахлеста, мм", ref str) == DialogResult.OK)
                    num1 = Convert.ToDouble(str) / 1000.0;
            }
            ModelItemCollection modelItemCollection = new ModelItemCollection();
            foreach (ModelItem selectedItem in Application.ActiveDocument.CurrentSelection.SelectedItems)
            {
                if (unit == "кг")
                {
                    DataProperty propertyByDisplayName1 = FindGeneralProperty(selectedItem, "Объект", "Длина стержня");
                    DataProperty propertyByDisplayName2 = FindGeneralProperty(selectedItem, "Объект", "Количество");
                    if ((propertyByDisplayName1 != null) && (propertyByDisplayName2 != null))
                    {
                        double num2 = Convert.ToDouble(propertyByDisplayName1.Value.ToDisplayString()) / 1000.0;
                        int int32 = propertyByDisplayName2.Value.ToInt32();
                        if (num2 > 11.7)
                            parameter += num2 / 11.7 * num1 * (double)int32;
                    }
                }
                DataProperty propertyByDisplayName3 = null;
                if (unit == "м3")
                {
                    propertyByDisplayName3 = FindVolumeProperty(selectedItem, cat, par);
                }
                else if (unit == "м2")
                {
                    propertyByDisplayName3 = FindAreaProperty(selectedItem, cat, par);
                }
                else if (unit == "м")
                {
                    propertyByDisplayName3 = FindLengthProperty(selectedItem, cat, par);
                }
                else
                {
                    propertyByDisplayName3 = FindGeneralProperty(selectedItem, cat, par);
                }

                if (propertyByDisplayName3 != (NativeHandle)null && propertyByDisplayName3 != null)
                {
                    double value = get_v2(propertyByDisplayName3, unit);
                    if (value > 0)
                    {
                        parameter += value;
                        modelItemCollection.Add(selectedItem);
                    }
                }
                else
                {
                    int num3 = 0;
                    foreach (ModelItem child1 in selectedItem.Children)
                    {
                        DataProperty propertyByDisplayName4 = null;
                        if (unit == "м3")
                        {
                            propertyByDisplayName4 = FindVolumeProperty(child1, cat, par);
                        }
                        else if (unit == "м2")
                        {
                            propertyByDisplayName4 = FindAreaProperty(child1, cat, par);
                        }
                        else if (unit == "м")
                        {
                            propertyByDisplayName4 = FindLengthProperty(child1, cat, par);
                        }
                        else
                        {
                            propertyByDisplayName4 = FindGeneralProperty(child1, cat, par);
                        }

                        if (propertyByDisplayName4 != (NativeHandle)null && propertyByDisplayName4 != null)
                        {
                            double value = get_v2(propertyByDisplayName4, unit);
                            if (value > 0)
                            {
                                parameter += value;
                                modelItemCollection.Add(child1);
                                num3 = 1;
                            }
                        }
                        else
                        {
                            foreach (ModelItem child2 in child1.Children)
                            {
                                DataProperty propertyByDisplayName5 = null;
                                if (unit == "м3")
                                {
                                    propertyByDisplayName5 = FindVolumeProperty(child2, cat, par);
                                }
                                else if (unit == "м2")
                                {
                                    propertyByDisplayName5 = FindAreaProperty(child2, cat, par);
                                }
                                else if (unit == "м")
                                {
                                    propertyByDisplayName5 = FindLengthProperty(child2, cat, par);
                                }
                                else
                                {
                                    propertyByDisplayName5 = FindGeneralProperty(child2, cat, par);
                                }

                                if (propertyByDisplayName5 != (NativeHandle)null && propertyByDisplayName5 != null)
                                {
                                    double value = get_v2(propertyByDisplayName5, unit);
                                    if (value > 0)
                                    {
                                        parameter += value;
                                        modelItemCollection.Add(child2);
                                        num3 = 1;
                                    }
                                }
                                else
                                {
                                    foreach (ModelItem child3 in child2.Children)
                                    {
                                        DataProperty propertyByDisplayName6 = null;
                                        if (unit == "м3")
                                        {
                                            propertyByDisplayName6 = FindVolumeProperty(child3, cat, par);
                                        }
                                        else if (unit == "м2")
                                        {
                                            propertyByDisplayName6 = FindAreaProperty(child3, cat, par);
                                        }
                                        else if (unit == "м")
                                        {
                                            propertyByDisplayName6 = FindLengthProperty(child3, cat, par);
                                        }
                                        else
                                        {
                                            propertyByDisplayName6 = FindGeneralProperty(child3, cat, par);
                                        }

                                        if (propertyByDisplayName6 != (NativeHandle)null && propertyByDisplayName6 != null)
                                        {
                                            double value = get_v2(propertyByDisplayName6, unit);
                                            if (value > 0)
                                            {
                                                parameter += value;
                                                modelItemCollection.Add(child3);
                                                num3 = 1;
                                            }
                                        }
                                        else
                                        {
                                            foreach (ModelItem child4 in child3.Children)
                                            {
                                                DataProperty propertyByDisplayName7 = null;
                                                if (unit == "м3")
                                                {
                                                    propertyByDisplayName7 = FindVolumeProperty(child4, cat, par);
                                                }
                                                else if (unit == "м2")
                                                {
                                                    propertyByDisplayName7 = FindAreaProperty(child4, cat, par);
                                                }
                                                else if (unit == "м")
                                                {
                                                    propertyByDisplayName7 = FindLengthProperty(child4, cat, par);
                                                }
                                                else
                                                {
                                                    propertyByDisplayName7 = FindGeneralProperty(child4, cat, par);
                                                }

                                                if (propertyByDisplayName7 != (NativeHandle)null && propertyByDisplayName7 != null)
                                                {
                                                    double value = get_v2(propertyByDisplayName7, unit);
                                                    if (value > 0)
                                                    {
                                                        parameter += value;
                                                        modelItemCollection.Add(child4);
                                                        num3 = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (num3 == 0)
                    {
                        ModelItem parent = selectedItem.Parent;
                        DataProperty propertyByDisplayName8 = null;
                        if (unit == "м3")
                        {
                            propertyByDisplayName8 = FindVolumeProperty(parent, cat, par);
                        }
                        else if (unit == "м2")
                        {
                            propertyByDisplayName8 = FindAreaProperty(parent, cat, par);
                        }
                        else if (unit == "м")
                        {
                            propertyByDisplayName8 = FindLengthProperty(parent, cat, par);
                        }
                        else
                        {
                            propertyByDisplayName8 = FindGeneralProperty(parent, cat, par);
                        }

                        if (propertyByDisplayName8 != (NativeHandle)null && propertyByDisplayName8 != null)
                        {
                            double value = get_v2(propertyByDisplayName8, unit);
                            if (value > 0)
                            {
                                parameter += value;
                                modelItemCollection.Add(parent);
                            }
                        }
                    }
                }
            }
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

        private static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button button1 = new Button();
            Button button2 = new Button();
            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;
            button1.Text = "OK";
            button2.Text = "Cancel";
            button1.DialogResult = DialogResult.OK;
            button2.DialogResult = DialogResult.Cancel;
            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            button1.SetBounds(228, 72, 75, 23);
            button2.SetBounds(309, 72, 75, 23);
            label.AutoSize = true;
            textBox.Anchor |= AnchorStyles.Right;
            button1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            button2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[4]
            {
                (Control) label,
                (Control) textBox,
                (Control) button1,
                (Control) button2
            });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = (IButtonControl)button1;
            form.CancelButton = (IButtonControl)button2;
            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private static string tostring(double num)
        {
            return !(new NumberFormatInfo().CurrencyDecimalSeparator == ".") ? num.ToString().Replace(".", ",") : num.ToString().Replace(",", ".");
        }
    }
}
