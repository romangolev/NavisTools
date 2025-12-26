using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Interop.ComApi;
using Autodesk.Navisworks.Api.Plugins;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Application = Autodesk.Navisworks.Api.Application;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;


namespace NavisTools

{
    /// <summary>
    /// The plugin attribute defines this class as a plugin.
    /// 
    /// Plugin Id: used to uniquely identify the plugin when combined with the Developer Id.
    /// 
    /// Developer Id: used in combination with the plugin Id to uniquely identify the plugin.
    /// 
    /// DisplayName: 
    /// </summary>
    [Plugin("CustomRibbonCommandHandler", "RG", DisplayName = "NavisTools")]
    /// <summary>
    /// An attribute identifying the name file that defines the localised text for 
    /// ribbon tabs and commands (see following attributes). Strings defined in the 
    /// name file will override any properties defined on the attributes. Text defined
    /// in the xaml layout file overrides strings defined in the name file.
    /// </summary>
    [Strings("NavisTools.name")]
    /// <summary>
    /// An attribute identifying a xaml file that defines the layout of associated
    /// ribbon tabs and commands (see following attributes).
    /// </summary>
    [RibbonLayout("NavisTools.xaml")]
    /// <summary>
    /// An attribute that defines a ribbon tab - must be accomnpanied by a 
    /// RibbonLayout attribute in order for the tab to appear in the GUI.
    /// 
    /// Tab Id: used to uniquely the ribbon tab within the plugin, and must 
    /// correspond to the tab Id used in the xaml layout file.
    /// 
    /// DisplayName: provides text for the ribbon tab, unless overriddden in 
    /// the name file (localised) or the localised xaml layout file.
    /// 
    /// LoadForCanExecute: if True, the plugin is fully loaded to ensure that the
    /// CanExecuteRibbonTab method is called. The CanExecuteRibbonTab method can 
    /// be used to make a ribbon tab contextual i.e. only visible when
    /// specified conditions are met. Otherwise the tab is visible by default.
    /// </summary>
    [RibbonTab("ID_CustomTab_1", DisplayName = "Custom Tab 1 - non-localised")]
    /// <summary>
    /// Defines a command that will perform an action within the application.
    /// 
    /// Command Id: used to uniquely identify the command within the plugin,
    /// and must correspond to the button Id, defined in the xaml layout definition.
    /// 
    /// DisplayName: Provides text for the command wherever it appears in the GUI,
    /// unless overridden by the button Text, defined in the xaml ribbon definition.
    /// The advantage of defining button Text in the xaml is that the plugin can be 
    /// localized by providing language-specific xaml files.
    ///
    /// Icon: defines the standard image used for the command wherever it appears 
    /// in the GUI, unless overridden by the button Image, defined in the xaml ribbon
    /// definition. This must be a 16x16 pixel image and it should be located next to 
    /// the plugin dll, or in an "Images" subdirectory next to the plugin dll.
    /// 
    /// LargeIcon: defines the large image used for the command when the command is 
    /// displayed as a Large button, as defined in the xaml ribbon definition. It must
    /// be a 32x32 pixel image. It must be located as per Icon.
    /// 
    /// CanToggle: defines whether the button as it appears in the ribbon can toggle 
    /// on and off. Specify True to make the button toggle.
    /// 
    /// ToolTip: defines text that will appear when the user hovers over the command
    /// button in the ribbon. This is not localised. Localised tooltips can be provided
    /// if they are defined in a name file.
    /// 
    /// ExtendedToolTip: defines additonal text that describes the purpose of the command
    /// in more detail than ToolTip. This is not localised. Localised tooltips can be provided
    /// if they are defined in a name file.
    /// 
    /// Shortcut: defines a keyboard shortcut that can be used to activate the command 
    /// e.g. Ctrl+B. If the shortcut conflicts with a shortcut already defined in the 
    /// application it will not be set. 
    /// 
    /// ShortcutWindowTypes: If you have defined a DockPanePlugin, you can specify that
    /// the Shortcut only applies when that window is active. Use the Id for the 
    /// DockPanePlugin.
    /// 
    /// CallCanExecute: Determines the conditions in which CanExecuteCommand should be 
    /// called. If the CanExecuteCommand is not called the default command state is disabled. 
    /// Options are: 
    /// Always (i.e. the CanExecuteCommand is always called, regardless of model state)  
    /// DocumentNotClear (i.e. the CanExecuteCommand is called only when a model is open)
    /// CurrentSelectionSingle (i.e. the CanExecuteCommand is called only when a single item
    /// is selected in the open model)
    /// CurrentSelectionMultiple (the CanExecuteCommand is called only when multiple items 
    /// are selected in the open model)
    /// 
    /// LoadForCanExecute: Commands are enabled by default, but if this is False the
    /// plugin will not be fully loaded until the first time a command is executed. If
    /// this is True the plugin will be fully loaded at application startup in order to
    /// call the CanExecuteCommand method defined by the plugin author.
    /// </summary>
    [Command("ID_Button_1", DisplayName = "Button 1 non-localized", Icon = "One_16.ico", LargeIcon = "One_32.ico", ToolTip = "Button 1 - non-localized", ExtendedToolTip = "Extended Button 1 tooltip - non-localized")]
    [Command("ID_Button_1A", DisplayName = "Button 1A non-localized", Icon = "One_16.ico", LargeIcon = "One_32.ico", ToolTip = "Button 1A - non-localized", ExtendedToolTip = "Extended Button 1A tooltip - non-localized")]
    [Command("ID_Button_2", CanToggle = true, Shortcut = "Shift+X")]
    [Command("ID_Button_3", CanToggle = true, Shortcut = "Shift+Z")]
    [Command("ID_Button_4", LoadForCanExecute = true)]
    [Command("ID_Button_5")]
    [Command("ID_Button_6")]
    [Command("ID_Button_7")]
    [Command("ID_Button_A", CanToggle = true)]
    [Command("ID_Button_B", CanToggle = true)]
    [Command("ID_Button_C", CanToggle = true)]
    [Command("ID_Button_D", CanToggle = true)]
    [Command("ID_Button_E", CanToggle = true)]

    [Command("ID_Button_11")]
    [Command("ID_Button_12")]
    [Command("ID_Button_13")]
    [Command("ID_Button_14")]

    public class CustomRibbonCommandHandler : CommandHandlerPlugin
    {
        /// <summary>
        /// Constructor, just initialises variables.
        /// </summary>
        public CustomRibbonCommandHandler()
        {
            m_button2_on = false;
            m_button3_on = false;
            m_radio_group_DEF = "ID_Button_D";
            m_buttonG_on = false;
            m_buttonH_on = false;
        }

        /// <summary>
        /// Executes a command when a button in the ribbon is pressed.
        /// </summary>
        /// <param name="commandId">Identifies the command associated with the button 
        /// that was pressed, by the Id defined in the command attribute.</param>
        /// <param name="parameters">Not currently used by Navisworks. If command is
        /// invoked programmatically by plugin author it can be used to pass additional
        /// information.</param>
        /// <returns>Not used by Navisworks. If command is invoked programmatically by 
        /// plugin author then it can be used to return additional information.</returns>
        public override int ExecuteCommand(string commandId, params string[] parameters)
        {
            // current document (.NET)
            Document doc = Autodesk.Navisworks.Api.Application.ActiveDocument;
            // current document (COM)
            InwOpState10 cdoc = ComApiBridge.State;
            // current selected items
            ModelItemCollection items = doc.CurrentSelection.SelectedItems;



            switch (commandId)
            {
                case "ID_Button_1":
                    try
                    {
                        Helper.ShowMyDialog("Объем выборки", Helper.tostring(Helper.get_parameter("Объект", "Объем", "м3")), "Объем = \n", "м3");
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        int num = (int)MessageBox.Show(ex.Message);
                        return 0;
                    }
                case "ID_Button_1A":
                    try
                    {
                        MessageBox.Show(Autodesk.Navisworks.Api.Application.Gui.MainWindow, "Changing selection to parents");
                        //ParentToParam.SelectParents();

                        //Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectAll();
                        // ModelItemCollection oModelColl = Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectedItems;

                        foreach (ModelItem item in items)
                        {
                            // convert ModelItem to COM Path
                            InwOaPath citem = (InwOaPath)ComApiBridge.ToInwOaPath(item);
                            // Get item's PropertyCategoryCollection
                            InwGUIPropertyNode2 cpropcates = (InwGUIPropertyNode2)cdoc.GetGUIPropertyNode(citem, true);
                            // create a new Category (PropertyDataCollection)
                            InwOaPropertyVec newcate = (InwOaPropertyVec)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);
                            // create a new Property (PropertyData)
                            InwOaProperty newprop = (InwOaProperty)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
                            // set PropertyName
                            newprop.name = "ParentName" + "_InternalName";
                            // set PropertyDisplayName
                            newprop.UserName = "ParentName";
                            // set PropertyValue
                            if (item.Parent != null)
                            {
                                newprop.value = item.Parent.DisplayName;
                            }
                            // add PropertyData to Category
                            newcate.Properties().Add(newprop);
                            // add CategoryData to item's CategoryDataCollection
                            cpropcates.SetUserDefined(0, "RHI", "RHI" + "_InternalName", newcate);
                        }

                        return 0;
                    }
                    catch (Exception ex)
                    {
                        int num = (int)MessageBox.Show(ex.Message);
                        return 0;
                    }
                case "ID_Button_2":
                    {
                        m_button2_on = !m_button2_on;
                        MessageBox.Show("Custom Tab 2 visibility changing!");
                        break;
                    }
                case "ID_Button_3":
                    {
                        m_button3_on = !m_button3_on;
                        MessageBox.Show("Button 4 visibility changing!");
                        break;
                    }
                case "ID_Button_D":
                case "ID_Button_E":
                case "ID_Button_F":
                    {
                        m_radio_group_DEF = commandId;
                        break;
                    }
                case "ID_Button_11":
                    try
                    {
                        this.ShowMyDialog("Объем выборки", this.tostring(this.get_parameter("Объект", "Объем", "м3")), "Объем = \n", "м3");
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        int num = (int)MessageBox.Show(ex.Message);
                        return 0;
                    }
                case "ID_Button_12":
                    try
                    {
                        this.ShowMyDialog("Площадь выборки", this.tostring(this.get_parameter("Объект", "Площадь", "м2")), "Площадь = \n", "м2");
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        int num = (int)MessageBox.Show(ex.Message);
                        return 0;
                    }
                case "ID_Button_13":
                    try
                    {
                        this.ShowMyDialog("Длина выборки", this.tostring(this.get_parameter("Объект", "Длина", "м")), "Длина = \n", "м");
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        int num = (int)MessageBox.Show(ex.Message);
                        return 0;
                    }
                case "ID_Button_14":
                    try
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
                        this.ShowMyDialog("Количество", num.ToString(), "Кол-во = \n", "шт.");
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        int num = (int)MessageBox.Show(ex.Message);
                        return 0;
                    }

                default:
                    {
                        MessageBox.Show("You have clicked on the command with ID = '" + commandId + "'");
                        break;
                    }
            }

            return 0;
        }

        /// <summary>
        /// Updates the command specified by the command Id.
        /// </summary>
        /// <param name="commandId">The Id of the command being updated, as defined in 
        /// the command attribute.</param>
        /// <returns>The CommandState indicates if the command is enabled, checked (if a 
        /// toggle command) and visible. The associated button in the ribbon will be 
        /// displayed accordingly.</returns>
        public override CommandState CanExecuteCommand(String commandId)
        {
            CommandState state = new CommandState();
            switch (commandId)
            {
                // Button 1 is only enabled when Button 2 is toggled on.
                case "ID_Button_1":
                    {
                        state.IsEnabled = m_button2_on;
                        break;
                    }
                // Button 2 is always enabled. It's toggle state is set according to m_button2_on.
                case "ID_Button_2":
                    {
                        state.IsEnabled = true;
                        state.IsChecked = m_button2_on;
                        // when m_button2_on is true, Custom Tab 2 should be visible, so we can update
                        // our button text if we wish
                        if (m_button2_on)
                            state.OverrideDisplayName = "Hide Tab 2";
                        else
                            state.OverrideDisplayName = "Show Tab 2";
                        break;
                    }
                // Button 3 is always enabled. It's toggle state is set according to m_button3_on.
                case "ID_Button_3":
                    {
                        state.IsEnabled = true;
                        state.IsChecked = m_button3_on;
                        // when m_button3_on is true, Button 4 should be visible, so we can update
                        // our button text if we wish
                        if (m_button3_on)
                            state.OverrideDisplayName = "Hide Button 4";
                        else
                            state.OverrideDisplayName = "Show Button 4";
                        break;
                    }
                // Button 4 is only visible and enabled when Button 3 is toggled on (i.e. IsChecked=True).
                case "ID_Button_4":
                    {
                        state.IsVisible = m_button3_on;
                        state.IsEnabled = m_button3_on;
                        break;
                    }
                // Toggle Buttons D, E and F are mutually exclusive in their toggle state.
                case "ID_Button_D":
                case "ID_Button_E":
                case "ID_Button_F":
                    {
                        state.IsEnabled = true;
                        state.IsChecked = (m_radio_group_DEF == commandId);
                        break;
                    }
                default:
                    {
                        state.IsVisible = true;
                        state.IsEnabled = true;
                        state.IsChecked = false;
                        break;
                    }
            }

            return state;
        }

        /// <summary>
        /// Override this method to display assistance to the user for the command with the specified Id.
        /// </summary>
        public override bool TryShowCommandHelp(String commandId)
        {
            MessageBox.Show("Showing Help for command with the Id " + commandId);
            return true;
        }

        /// <summary>
        /// Indicates if a ribbon should be visible or not. This is used for contextual
        /// tabs that should only be visibile under conditions defined by the plugin author.
        /// </summary>
        /// <param name="ribbonTabId">The Id of the ribbon tab, as defined in the attribute.</param>
        /// <returns>True indicates that ribbon tab is visible, otherwise false.</returns>
        public override bool CanExecuteRibbonTab(String ribbonTabId)
        {
            // The second ribbon tab is only visible when Button 2 is toggled on (i.e. m_button2_on is true).
            if (ribbonTabId.Equals("ID_CustomTab_2"))
            {
                return m_button2_on;
            }

            return true;
        }

        private bool m_button2_on;
        private bool m_button3_on;
        private bool m_buttonG_on;
        private bool m_buttonH_on;
        private String m_radio_group_DEF;

        private DataProperty FindGeneralProperty(ModelItem item, string category, string propertyName)
        {
            // Try the exact property name first
            DataProperty prop = item.PropertyCategories.FindPropertyByDisplayName(category, propertyName);
            if (prop != (NativeHandle)null)
                return prop;

            // Define general variations for non-geometric properties
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

        private DataProperty FindVolumeProperty(ModelItem item, string category, string propertyName)
        {
            // Try the exact property name first
            DataProperty prop = item.PropertyCategories.FindPropertyByDisplayName(category, propertyName);
            if (prop != (NativeHandle)null)
                return prop;

            // Define volume-specific variations
            string[] variations = new string[]
            {
                "Объем",
                "Volume",
                propertyName + " (м3)",
                "Объем (м3)",
                "Volume (m³)"
            };

            foreach (string variation in variations)
            {
                prop = item.PropertyCategories.FindPropertyByDisplayName(category, variation);
                if (prop != (NativeHandle)null)
                    return prop;
            }

            return null;
        }

        private DataProperty FindAreaProperty(ModelItem item, string category, string propertyName)
        {
            // Try the exact property name first
            DataProperty prop = item.PropertyCategories.FindPropertyByDisplayName(category, propertyName);
            if (prop != (NativeHandle)null)
                return prop;

            // Define area-specific variations
            string[] variations = new string[]
            {
                "Площадь",
                "Area",
                propertyName + " (м2)",
                "Площадь (м2)",
                "Area (m²)"
            };

            foreach (string variation in variations)
            {
                prop = item.PropertyCategories.FindPropertyByDisplayName(category, variation);
                if (prop != (NativeHandle)null)
                    return prop;
            }

            return null;
        }

        private DataProperty FindLengthProperty(ModelItem item, string category, string propertyName)
        {
            // Try the exact property name first
            DataProperty prop = item.PropertyCategories.FindPropertyByDisplayName(category, propertyName);
            if (prop != (NativeHandle)null)
                return prop;

            // Define length-specific variations
            string[] variations = new string[]
            {
                "Длина",
                "Length",
                propertyName + " (м)",
                "Длина (м)",
                "Length (m)"
            };

            foreach (string variation in variations)
            {
                prop = item.PropertyCategories.FindPropertyByDisplayName(category, variation);
                if (prop != (NativeHandle)null)
                    return prop;
            }

            return null;
        }

        private double get_v2(DataProperty prop, string unit)
        {
            double v2;
            switch (unit)
            {
                case "м3":
                    try
                    {
                        // Volume is stored in cubic feet, convert to cubic meters
                        v2 = Math.Round(prop.Value.ToDoubleVolume() * 0.3048 * 0.3048 * 0.3048, 3);
                    }
                    catch
                    {
                        try
                        {
                            // Fallback: try parsing display string (e.g., "4.652 m³")
                            string displayStr = prop.Value.ToDisplayString();
                            string numericStr = displayStr.Split(' ')[0].Replace(",", ".");
                            if (double.TryParse(numericStr, NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double parsedValue))
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
                        // Area is stored in square feet, convert to square meters
                        Console.WriteLine(prop.Value);
                        v2 = Math.Round(prop.Value.ToDoubleArea() * 0.3048 * 0.3048, 2);
                    }
                    catch
                    {
                        try
                        {
                            // Fallback: try parsing display string (e.g., "46.520 m²")
                            string displayStr = prop.Value.ToDisplayString();
                            string numericStr = displayStr.Split(' ')[0].Replace(",", ".");
                            if (double.TryParse(numericStr, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double parsedValue))
                            {
                                // Value is already in m², no conversion needed
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
                        // Length is stored in feet, convert to meters
                        v2 = Math.Round(prop.Value.ToDoubleLength() * 0.3048, 2);
                    }
                    catch
                    {
                        try
                        {
                            // Fallback: try parsing display string (e.g., "21.700 m")
                            string displayStr = prop.Value.ToDisplayString();
                            string numericStr = displayStr.Split(' ')[0].Replace(",", ".");
                            if (double.TryParse(numericStr, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double parsedValue))
                            {
                                // Value is already in m, no conversion needed
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

        private double get_parameter(string cat, string par, string unit)
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
                    double value = this.get_v2(propertyByDisplayName3, unit);
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
                            double value = this.get_v2(propertyByDisplayName4, unit);
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
                                    double value = this.get_v2(propertyByDisplayName5, unit);
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
                                            double value = this.get_v2(propertyByDisplayName6, unit);
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
                                                    double value = this.get_v2(propertyByDisplayName7, unit);
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
                            double value = this.get_v2(propertyByDisplayName8, unit);
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

        private void ShowMyDialog(string title, string text, string text1, string text2)
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

        public static DialogResult InputBox(string title, string promptText, ref string value)
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

        public string tostring(double num)
        {
            return !(new NumberFormatInfo().CurrencyDecimalSeparator == ".") ? num.ToString().Replace(".", ",") : num.ToString().Replace(",", ".");
        }
    }
}
