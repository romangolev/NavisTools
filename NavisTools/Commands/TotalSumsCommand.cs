using NavisTools.Interfaces;
using NavisTools.Tools;

namespace NavisTools.Commands
{
    /// <summary>
    /// Command for showing total sums (Volume, Area, Length, Count).
    /// </summary>
    public class TotalSumsCommand : ToolCommandBase
    {
        public const string Id = "ID_Button_Total_Sums";

        public TotalSumsCommand(
            IDocumentProvider documentProvider,
            ISelectionService selectionService,
            INotificationService notificationService)
            : base(documentProvider, selectionService, notificationService)
        {
        }

        public override string CommandId => Id;

        public override int Execute()
        {
            if (!ValidateDocumentOpen())
                return 0;

            // Delegate to existing implementation for now
            // In a full refactor, this would use IMeasurementService
            GetVolume.ExecuteTotalSumsCommand();
            return 0;
        }
    }
}
