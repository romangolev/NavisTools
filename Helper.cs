using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Application = Autodesk.Navisworks.Api.Application;

namespace NavisTools
{
    public class Helper
    {
        public static double get_v2(DataProperty prop, string unit)
        {
            double v2;
            if (unit == "м3")
                v2 = Math.Round(prop.Value.ToDoubleVolume() * 0.3048 * 0.3048 * 0.3048, 3);
            else if (unit == "m³")
                v2 = Math.Round(prop.Value.ToDoubleVolume() * 0.3048 * 0.3048 * 0.3048, 3);
            else if (unit == "м2")
                v2 = Math.Round(prop.Value.ToDoubleArea() * 0.3048 * 0.3048, 2);
            else if (unit == "м")
                v2 = Math.Round(prop.Value.ToDoubleLength() * 0.3048, 2);
            else if (unit == "кг")
            {
                try
                {
                    v2 = Math.Round(double.Parse(prop.Value.ToDisplayString().Remove(prop.Value.ToDisplayString().Length - 4).Replace(",", ".")) * 0.00785, 2);
                }
                catch
                {
                    v2 = Math.Round(double.Parse(prop.Value.ToDisplayString().Remove(prop.Value.ToDisplayString().Length - 4).Replace(".", ",")) * 0.00785, 2);
                }
            }
            else
                v2 = !(unit == "кг2") ? 1.0 : Math.Round(prop.Value.ToDouble(), 2);
            return v2;
        }

        public static double get_parameter(string cat, string par, string unit)
        {
            double parameter = 0.0;
            double num1 = 1.7;
            if (unit == "кг")
            {
                string str = "";
                if (NavisTools.Helper.InputBox("Введите длину нахлеста", "Длина нахлеста, мм", ref str) == DialogResult.OK)
                    num1 = Convert.ToDouble(str) / 1000.0;
            }
            ModelItemCollection modelItemCollection = new ModelItemCollection();
            foreach (ModelItem selectedItem in Application.ActiveDocument.CurrentSelection.SelectedItems)
            {
                if (unit == "кг")
                {
                    DataProperty propertyByDisplayName1 = selectedItem.PropertyCategories.FindPropertyByDisplayName("Объект", "Длина стержня");
                    DataProperty propertyByDisplayName2 = selectedItem.PropertyCategories.FindPropertyByDisplayName("Объект", "Количество");
                    if (!NativeHandle.Equals((NativeHandle)propertyByDisplayName1, (NativeHandle)null) && NativeHandle.Equals((NativeHandle)propertyByDisplayName2, (NativeHandle)null))
                    {
                        double num2 = Convert.ToDouble(propertyByDisplayName1.Value.ToDisplayString()) / 1000.0;
                        int int32 = propertyByDisplayName2.Value.ToInt32();
                        if (num2 > 11.7)
                            parameter += num2 / 11.7 * num1 * (double)int32;
                    }
                }
                DataProperty propertyByDisplayName3 = selectedItem.PropertyCategories.FindPropertyByDisplayName(cat, par);
                if (!NativeHandle.Equals(propertyByDisplayName3, null))
                {
                    parameter += get_v2(propertyByDisplayName3, unit);
                    modelItemCollection.Add(selectedItem);
                }
                else
                {
                    int num3 = 0;
                    foreach (ModelItem child1 in selectedItem.Children)
                    {
                        DataProperty propertyByDisplayName4 = child1.PropertyCategories.FindPropertyByDisplayName(cat, par);
                        if (!NativeHandle.Equals((NativeHandle)propertyByDisplayName4, (NativeHandle)null))
                        {
                            parameter += get_v2(propertyByDisplayName4, unit);
                            modelItemCollection.Add(child1);
                            num3 = 1;
                        }
                        else
                        {
                            foreach (ModelItem child2 in child1.Children)
                            {
                                DataProperty propertyByDisplayName5 = child2.PropertyCategories.FindPropertyByDisplayName(cat, par);
                                if (!NativeHandle.Equals((NativeHandle)propertyByDisplayName5, (NativeHandle)null))
                                {
                                    parameter += get_v2(propertyByDisplayName5, unit);
                                    modelItemCollection.Add(child2);
                                    num3 = 1;
                                }
                                else
                                {
                                    foreach (ModelItem child3 in child2.Children)
                                    {
                                        DataProperty propertyByDisplayName6 = child3.PropertyCategories.FindPropertyByDisplayName(cat, par);
                                        if (!NativeHandle.Equals((NativeHandle)propertyByDisplayName6, (NativeHandle)null))
                                        {
                                            parameter += get_v2(propertyByDisplayName6, unit);
                                            modelItemCollection.Add(child3);
                                            num3 = 1;
                                        }
                                        else
                                        {
                                            foreach (ModelItem child4 in child3.Children)
                                            {
                                                DataProperty propertyByDisplayName7 = child4.PropertyCategories.FindPropertyByDisplayName(cat, par);
                                                if (!NativeHandle.Equals((NativeHandle)propertyByDisplayName7, (NativeHandle)null))
                                                {
                                                    parameter += get_v2(propertyByDisplayName7, unit);
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
                    if (num3 == 0)
                    {
                        ModelItem parent = selectedItem.Parent;
                        DataProperty propertyByDisplayName8 = parent.PropertyCategories.FindPropertyByDisplayName(cat, par);
                        if (!NativeHandle.Equals((NativeHandle)propertyByDisplayName8, (NativeHandle)null))
                        {
                            parameter += get_v2(propertyByDisplayName8, unit);
                            modelItemCollection.Add(parent);
                        }
                    }
                }
            }
            Selection selection = new Selection(modelItemCollection);
            Application.ActiveDocument.CurrentSelection.Clear();
            Application.ActiveDocument.CurrentSelection.CopyFrom(selection);
            return parameter;
        }
        public static void ShowMyDialog(string title, string text, string text1, string text2)
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

        public static DialogResult InputBox(
          string title,
          string promptText,
          ref string value)
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
            form.Controls
                .AddRange(new Control[4]
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

        public static string tostring(double num) => !(new NumberFormatInfo().CurrencyDecimalSeparator == ".") ? num.ToString().Replace(".", ",") : num.ToString().Replace(",", ".");

    }
}
