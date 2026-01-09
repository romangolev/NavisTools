using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using NavisTools.Commands;
using NavisTools.Services;
using System;
using System.Windows.Forms;
using Application = Autodesk.Navisworks.Api.Application;


namespace NavisTools
{
    [Plugin("NavisTools", "NavisTools", DisplayName = "NavisTools")]
    [Strings("NavisTools.name")]
    [RibbonLayout("NavisTools.xaml")]
    [RibbonTab("ID_NavisToolsRibbonTab", DisplayName = "Navis Tools")]
    [Command("ID_Button_AddParentName", DisplayName = "Add Parent Name", ToolTip = "Add Parent Name", ExtendedToolTip = "Add parent name to selected items as a parameter")]
    [Command("ID_Button_Total_Sums", DisplayName = "Total Sums", ToolTip = "Total Sums", ExtendedToolTip = "Calculate total sums for selected items")]
    [Command("ID_SplitButton_Config", DisplayName = "Settings", ToolTip = "Configuration Settings", ExtendedToolTip = "Configure NavisTools settings")]
    [Command("ID_Button_ConfigSettings", DisplayName = "Settings...")]
    [Command("ID_Button_ConfigReset", DisplayName = "Reset to Defaults")]
	[Command("ID_Button_SelectionInfo", DisplayName = "Selection Info", ToolTip = "Selection Info Panel", ExtendedToolTip = "Show the selection info dockable panel")]
	public class NavisToolsCommandHandler : CommandHandlerPlugin
	{
		public NavisToolsCommandHandler()
		{
			// Initialize services on first use
			ServiceLocator.Initialize();
		}

        public override int ExecuteCommand(string commandId, params string[] parameters)
        {
			try
			{
				// Use command registry for command execution (Open/Closed Principle)
				var command = CommandRegistry.Instance.GetCommand(commandId);
				if (command != null)
				{
					return command.Execute();
				}

				// Handle split button that shares ID with settings
				if (commandId == "ID_SplitButton_Config")
				{
					var settingsCommand = CommandRegistry.Instance.GetCommand(OpenSettingsCommand.Id);
					return settingsCommand?.Execute() ?? 0;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(Application.Gui.MainWindow, 
					ex.Message, 
					"Error", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error);
			}

			return 0;
		}

		public override CommandState CanExecuteCommand(String commandId)
		{
			var state = new CommandState
			{
				IsVisible = true,
				IsEnabled = true,
				IsChecked = false
			};

			// Use command registry to check if command can execute
			var command = CommandRegistry.Instance.GetCommand(commandId);
			if (command != null)
			{
				state.IsEnabled = command.CanExecute;
			}
			else if (commandId == "ID_SplitButton_Config")
			{
				// Split button is always enabled
				state.IsEnabled = true;
			}

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
