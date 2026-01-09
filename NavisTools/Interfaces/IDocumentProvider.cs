using Autodesk.Navisworks.Api;

namespace NavisTools.Interfaces
{
    /// <summary>
    /// Provides abstraction for accessing the Navisworks document.
    /// Enables testability by allowing mock implementations.
    /// </summary>
    public interface IDocumentProvider
    {
        /// <summary>
        /// Gets the currently active document.
        /// </summary>
        Document ActiveDocument { get; }

        /// <summary>
        /// Gets whether a document is currently open.
        /// </summary>
        bool HasActiveDocument { get; }

        /// <summary>
        /// Gets the number of models in the active document.
        /// </summary>
        int ModelCount { get; }
    }
}
