using Autodesk.Navisworks.Api;
using NavisTools.Interfaces;
using Application = Autodesk.Navisworks.Api.Application;

namespace NavisTools.Services
{
    /// <summary>
    /// Implementation of IDocumentProvider that wraps the Navisworks Application.
    /// </summary>
    public class NavisworksDocumentProvider : IDocumentProvider
    {
        public Document ActiveDocument => Application.ActiveDocument;

        public bool HasActiveDocument => ActiveDocument != null && ActiveDocument.Models.Count > 0;

        public int ModelCount => ActiveDocument?.Models?.Count ?? 0;
    }
}
