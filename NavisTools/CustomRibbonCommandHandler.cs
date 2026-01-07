using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Interop.ComApi;
using Autodesk.Navisworks.Api.Plugins;
using NavisTools.UI;
using System;
using System.Windows.Forms;
using Application = Autodesk.Navisworks.Api.Application;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;


namespace NavisTools
{
    [Plugin("NavisTools", "NavisTools", DisplayName = "NavisTools")]
    [Strings("NavisTools.name")]
    [RibbonLayout("NavisTools.xaml")]
    [RibbonTab("ID_CustomTab_1", DisplayName = "Navis Tools")]
    [Command("ID_Button_10")]
    [Command("ID_Button_11")]
    [Command("ID_Button_12")]
    [Command("ID_Button_13")]
    [Command("ID_Button_14")]
    [Command("ID_Button_15")]
    [Command("ID_Button_Config", DisplayName = "Settings", Icon = "Config_16.ico", LargeIcon = "Config_32.ico", ToolTip = "Configuration Settings", ExtendedToolTip = "Configure NavisTools settings")]
    [Command("ID_Button_Config_Settings", DisplayName = "Settings...")]
    [Command("ID_Button_Config_Reset", DisplayName = "Reset to Defaults")]
    [Command("ID_Button_SelectionInfo", DisplayName = "Selection Info", ToolTip = "Selection Info Panel", ExtendedToolTip = "Show the selection info dockable panel")]
    public class CustomRibbonCommandHandler : CommandHandlerPlugin
    {
        public CustomRibbonCommandHandler()
        {
        }

        public override int ExecuteCommand(string commandId, params string[] parameters)
        {
            Document doc = Application.ActiveDocument;
            InwOpState10 cdoc = ComApiBridge.State;
            ModelItemCollection items = doc.CurrentSelection.SelectedItems;

            switch (commandId)
            {
                case "ID_Button_10":
                    try
                    {
                        Tools.ParentToParam.ExecuteParentToParam(items, cdoc);
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        int num = (int)MessageBox.Show(ex.Message);
                        return 0;
					}
				case "ID_Button_11":
                    try
                    {
                        Tools.GetVolume.ExecuteVolumeCommand();
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
                        Tools.GetVolume.ExecuteAreaCommand();
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
                        Tools.GetVolume.ExecuteLengthCommand();
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
						Tools.GetVolume.ExecuteCountCommand();
						return 0;
					}
					catch (Exception ex)
					{
						int num = (int)MessageBox.Show(ex.Message);
						return 0;
					}
				case "ID_Button_15":
					try
					{
						Tools.GetVolume.ExecuteTotalSumsCommand();
						return 0;
					}
					catch (Exception ex)
					{
						int num = (int)MessageBox.Show(ex.Message);
						return 0;
					}
				case "ID_Button_Config":
                    {
                        ConfigurationManager.OpenSettings();
                        break;
                    }
                case "ID_Button_Config_Settings":
                    {
                        ConfigurationManager.OpenSettings();
                        break;
                    }
				case "ID_Button_Config_Reset":
					{
						ConfigurationManager.ResetToDefaults();
						break;
					}
				case "ID_Button_SelectionInfo":
					{
						try
						{
							if (!Application.IsAutomated)
							{
								var pluginRecord = Application.Plugins.FindPlugin("SelectionInfoPane.NavisTools");

								if (pluginRecord is DockPanePluginRecord && pluginRecord.IsEnabled)
								{
									var dockPane = (pluginRecord.LoadedPlugin ?? pluginRecord.LoadPlugin()) as DockPanePlugin;

									dockPane.ActivatePane();
								}
							}
						}
						catch (Exception ex)
						{
							MessageBox.Show(Application.Gui.MainWindow,
								$"Error toggling Selection Info panel: {ex.Message}",
								"Error",
								MessageBoxButtons.OK,
								MessageBoxIcon.Error);
						}
						break;
					}
				}
			return 0;
		}

		public override CommandState CanExecuteCommand(String commandId)
		{
			CommandState state = new CommandState
			{
				IsVisible = true,
				IsEnabled = true,
				IsChecked = false
			};
			return state;
		}

		public override bool TryShowCommandHelp(String commandId)
		{
			MessageBox.Show("Showing Help for command with the Id " + commandId);
			return true;
		}

		public override bool CanExecuteRibbonTab(String ribbonTabId)
		{
			return true;
		}
	}
}
