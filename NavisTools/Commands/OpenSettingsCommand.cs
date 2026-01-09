using NavisTools.Interfaces;

namespace NavisTools.Commands
{
    /// <summary>
    /// Command for opening configuration settings.
    /// </summary>
    public class OpenSettingsCommand : ToolCommandBase
    {
        public const string Id = "ID_Button_ConfigSettings";
        public const string SplitButtonId = "ID_SplitButton_Config";

        private readonly IConfigurationService _configurationService;

        public OpenSettingsCommand(
            IDocumentProvider documentProvider,
            ISelectionService selectionService,
            INotificationService notificationService,
            IConfigurationService configurationService)
            : base(documentProvider, selectionService, notificationService)
        {
            _configurationService = configurationService;
        }

        public override string CommandId => Id;

        public override bool CanExecute => true; // Settings always available

        public override int Execute()
        {
            _configurationService?.OpenSettings();
            return 0;
        }
    }
}
