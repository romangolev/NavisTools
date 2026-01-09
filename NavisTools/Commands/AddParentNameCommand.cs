using Autodesk.Navisworks.Api.Interop.ComApi;
using NavisTools.Interfaces;
using NavisTools.Services;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;

namespace NavisTools.Commands
{
    /// <summary>
    /// Command for adding parent name as a parameter to selected items.
    /// </summary>
    public class AddParentNameCommand : ToolCommandBase
    {
        public const string Id = "ID_Button_AddParentName";

        private readonly IConfigurationService _configurationService;

        public AddParentNameCommand(
            IDocumentProvider documentProvider,
            ISelectionService selectionService,
            INotificationService notificationService,
            IConfigurationService configurationService)
            : base(documentProvider, selectionService, notificationService)
        {
            _configurationService = configurationService;
        }

        public override string CommandId => Id;

        public override bool CanExecute => base.CanExecute && SelectionService.HasSelection;

        public override int Execute()
        {
            if (!ValidateDocumentOpen())
                return 0;

            if (!ValidateSelection())
                return 0;

            var items = SelectionService.SelectedItems;
            InwOpState10 cdoc = ComApiBridge.State;

            NotificationService?.ShowBalloon("Adding Parent Name", $"Processing {items.Count} selected item(s)...");

            foreach (Autodesk.Navisworks.Api.ModelItem item in items)
            {
                var citem = (InwOaPath)ComApiBridge.ToInwOaPath(item);
                var cpropcates = (InwGUIPropertyNode2)cdoc.GetGUIPropertyNode(citem, true);
                var newcate = (InwOaPropertyVec)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);
                var newprop = (InwOaProperty)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);

                string paramName = _configurationService?.ParentParameterName ?? "ParentName";
                string categoryName = _configurationService?.ParentCategoryName ?? "NavisTools";

                newprop.name = paramName + "_InternalName";
                newprop.UserName = paramName;

                if (item.Parent != null)
                {
                    newprop.value = item.Parent.DisplayName;
                }

                newcate.Properties().Add(newprop);
                cpropcates.SetUserDefined(0, categoryName, categoryName + "_InternalName", newcate);
            }

            return 0;
        }
    }
}
