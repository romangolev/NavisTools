using System;
using System.Windows.Forms;
using Application = Autodesk.Navisworks.Api.Application;

namespace NavisTools.Helpers
{
    /// <summary>
    /// Helper class for showing balloon notifications in Navisworks
    /// </summary>
    public static class BalloonHelper
    {
        /// <summary>
        /// Shows a balloon notification in the Navisworks status bar area
        /// </summary>
        /// <param name="title">The title of the balloon notification</param>
        /// <param name="message">The message to display</param>
        /// <param name="icon">The icon to display (default: Info)</param>
        /// <param name="durationMs">Duration in milliseconds (default: 3000)</param>
        public static void ShowBalloon(string title, string message, ToolTipIcon icon = ToolTipIcon.Info, int durationMs = 3000)
        {
            try
            {
                var balloon = new ToolTip
                {
                    IsBalloon = true,
                    ToolTipTitle = title,
                    ToolTipIcon = icon
                };

                // Get the main window handle
                var mainWindow = Application.Gui.MainWindow;
                if (mainWindow != null)
                {
                    // Show balloon near the bottom right of the window
                    var control = Control.FromHandle(mainWindow.Handle);
                    if (control != null)
                    {
                        balloon.Show(message, control, control.Width - 250, control.Height - 100, durationMs);
                    }
                }
            }
            catch
            {
                // Fallback - silently continue if balloon fails
            }
        }
    }
}
