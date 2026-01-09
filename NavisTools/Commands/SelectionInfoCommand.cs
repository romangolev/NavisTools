using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using NavisTools.Interfaces;
using Application = Autodesk.Navisworks.Api.Application;

namespace NavisTools.Commands
{
    /// <summary>
    /// Command for showing the Selection Info dockable panel.
    /// </summary>
    public class SelectionInfoCommand : ToolCommandBase
    {
        public const string Id = "ID_Button_SelectionInfo";

        public SelectionInfoCommand(
            IDocumentProvider documentProvider,
            ISelectionService selectionService,
            INotificationService notificationService)
            : base(documentProvider, selectionService, notificationService)
        {
        }

        public override string CommandId => Id;

        public override bool CanExecute => !Application.IsAutomated;

        public override int Execute()
        {
            try
            {
                if (Application.IsAutomated)
                    return 0;

                var pluginRecord = Application.Plugins.FindPlugin("SelectionInfoPane.NavisTools");

                if (pluginRecord is DockPanePluginRecord && pluginRecord.IsEnabled)
                {
                    var dockPane = (pluginRecord.LoadedPlugin ?? pluginRecord.LoadPlugin()) as DockPanePlugin;
                    dockPane?.ActivatePane();
                }
            }
            catch (System.Exception ex)
            {
                NotificationService?.ShowError($"Error toggling Selection Info panel: {ex.Message}");
            }

            return 0;
        }
    }
}
