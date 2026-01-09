using NavisTools.Interfaces;

namespace NavisTools.Commands
{
    /// <summary>
    /// Base class for tool commands providing common functionality.
    /// </summary>
    public abstract class ToolCommandBase : IToolCommand
    {
        protected readonly IDocumentProvider DocumentProvider;
        protected readonly ISelectionService SelectionService;
        protected readonly INotificationService NotificationService;

        protected ToolCommandBase(
            IDocumentProvider documentProvider,
            ISelectionService selectionService,
            INotificationService notificationService)
        {
            DocumentProvider = documentProvider;
            SelectionService = selectionService;
            NotificationService = notificationService;
        }

        public abstract string CommandId { get; }

        public virtual bool CanExecute => DocumentProvider?.HasActiveDocument ?? false;

        public abstract int Execute();

        /// <summary>
        /// Validates that a document is open before execution.
        /// </summary>
        protected bool ValidateDocumentOpen()
        {
            if (!DocumentProvider.HasActiveDocument)
            {
                NotificationService?.ShowWarning("Please open a document first.", "No Document");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates that items are selected before execution.
        /// </summary>
        protected bool ValidateSelection()
        {
            if (!SelectionService.HasSelection)
            {
                NotificationService?.ShowWarning("Please select at least one item.", "No Selection");
                return false;
            }
            return true;
        }
    }
}
