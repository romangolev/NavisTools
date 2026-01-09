using Autodesk.Navisworks.Api;
using System;

namespace NavisTools.Interfaces
{
    /// <summary>
    /// Provides abstraction for selection-related operations.
    /// </summary>
    public interface ISelectionService
    {
        /// <summary>
        /// Gets the currently selected items.
        /// </summary>
        ModelItemCollection SelectedItems { get; }

        /// <summary>
        /// Gets whether there are any items selected.
        /// </summary>
        bool HasSelection { get; }

        /// <summary>
        /// Gets the count of selected items.
        /// </summary>
        int SelectionCount { get; }

        /// <summary>
        /// Event raised when the selection changes.
        /// </summary>
        event EventHandler SelectionChanged;

        /// <summary>
        /// Clears the current selection.
        /// </summary>
        void ClearSelection();

        /// <summary>
        /// Adds an item to the current selection.
        /// </summary>
        void AddToSelection(ModelItem item);
    }
}
