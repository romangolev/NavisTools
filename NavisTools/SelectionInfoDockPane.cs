using Autodesk.Navisworks.Api.Plugins;
using NavisTools.UI;
using System.Windows.Forms;

namespace NavisTools
{
    /// <summary>
    /// Dockable panel plugin that displays selection information dynamically.
    /// Updates automatically when selection changes in Navisworks.
    /// </summary>
    [Plugin("SelectionInfoPane", "RG", DisplayName = "Selection Info")]
    [DockPanePlugin(300, 450, AutoScroll = true, FixedSize = false, MinimumHeight = 200, MinimumWidth = 250)]
    public class SelectionInfoDockPane : DockPanePlugin
    {
        private SelectionInfoControl _selectionInfoControl;

        /// <summary>
        /// Called when the dock pane is created. Returns the control to display in the pane.
        /// </summary>
        public override Control CreateControlPane()
        {
            _selectionInfoControl = new SelectionInfoControl();
            _selectionInfoControl.Dock = DockStyle.Fill;
            return _selectionInfoControl;
        }

        /// <summary>
        /// Called when the dock pane is destroyed. Cleanup resources.
        /// </summary>
        public override void DestroyControlPane(Control pane)
        {
            _selectionInfoControl?.Cleanup();
            _selectionInfoControl = null;
            pane?.Dispose();
        }
    }
}
