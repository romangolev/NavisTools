using Autodesk.Navisworks.Api;
using NavisTools.Interfaces;
using System;
using Application = Autodesk.Navisworks.Api.Application;

namespace NavisTools.Services
{
    /// <summary>
    /// Implementation of ISelectionService that wraps Navisworks selection functionality.
    /// </summary>
    public class NavisworksSelectionService : ISelectionService
    {
        private readonly IDocumentProvider _documentProvider;

        public event EventHandler SelectionChanged;

        public NavisworksSelectionService(IDocumentProvider documentProvider)
        {
            _documentProvider = documentProvider ?? throw new ArgumentNullException(nameof(documentProvider));
            SubscribeToSelectionEvents();
        }

        public ModelItemCollection SelectedItems
        {
            get
            {
                var doc = _documentProvider.ActiveDocument;
                return doc?.CurrentSelection?.SelectedItems ?? new ModelItemCollection();
            }
        }

        public bool HasSelection => SelectionCount > 0;

        public int SelectionCount => SelectedItems?.Count ?? 0;

        public void ClearSelection()
        {
            _documentProvider.ActiveDocument?.CurrentSelection?.Clear();
        }

        public void AddToSelection(ModelItem item)
        {
            _documentProvider.ActiveDocument?.CurrentSelection?.Add(item);
        }

        private void SubscribeToSelectionEvents()
        {
            Application.ActiveDocumentChanged += OnActiveDocumentChanged;
            SubscribeToCurrentDocument();
        }

        private void OnActiveDocumentChanged(object sender, EventArgs e)
        {
            SubscribeToCurrentDocument();
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SubscribeToCurrentDocument()
        {
            var doc = _documentProvider.ActiveDocument;
            if (doc != null)
            {
                // Note: In production, we'd need to track and unsubscribe from old documents
                doc.CurrentSelection.Changed += OnSelectionChangedInternal;
            }
        }

        private void OnSelectionChangedInternal(object sender, EventArgs e)
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
