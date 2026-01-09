using NavisTools.Interfaces;

namespace NavisTools.Commands
{
    /// <summary>
    /// Command for resetting settings to defaults.
    /// </summary>
    public class ResetSettingsCommand : ToolCommandBase
    {
        public const string Id = "ID_Button_ConfigReset";

        private readonly IConfigurationService _configurationService;

        public ResetSettingsCommand(
            IDocumentProvider documentProvider,
            ISelectionService selectionService,
            INotificationService notificationService,
            IConfigurationService configurationService)
            : base(documentProvider, selectionService, notificationService)
        {
            _configurationService = configurationService;
        }

        public override string CommandId => Id;

        public override bool CanExecute => true; // Reset always available

        public override int Execute()
        {
            _configurationService?.ResetToDefaults();
            return 0;
        }
    }
}
