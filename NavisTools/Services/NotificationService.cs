using NavisTools.Interfaces;
using System;
using System.Windows.Forms;
using Application = Autodesk.Navisworks.Api.Application;

namespace NavisTools.Services
{
    /// <summary>
    /// Implementation of INotificationService for Navisworks notifications.
    /// </summary>
    public class NotificationService : INotificationService
    {
        public void ShowBalloon(string title, string message, ToolTipIcon icon = ToolTipIcon.Info, int durationMs = 3000)
        {
            try
            {
                var balloon = new ToolTip
                {
                    IsBalloon = true,
                    ToolTipTitle = title,
                    ToolTipIcon = icon
                };

                var mainWindow = Application.Gui?.MainWindow;
                if (mainWindow != null)
                {
                    var control = Control.FromHandle(mainWindow.Handle);
                    if (control != null)
                    {
                        balloon.Show(message, control, control.Width - 250, control.Height - 100, durationMs);
                    }
                }
            }
            catch
            {
                // Silently fail if balloon cannot be shown
            }
        }

        public void ShowWarning(string message, string title = "Warning")
        {
            ShowMessage(message, title, MessageBoxIcon.Warning);
        }

        public void ShowError(string message, string title = "Error")
        {
            ShowMessage(message, title, MessageBoxIcon.Error);
        }

        public void ShowInfo(string message, string title = "Information")
        {
            ShowMessage(message, title, MessageBoxIcon.Information);
        }

        private void ShowMessage(string message, string title, MessageBoxIcon icon)
        {
            var owner = GetMainWindowOwner();
            if (owner != null)
            {
                MessageBox.Show(owner, message, title, MessageBoxButtons.OK, icon);
            }
            else
            {
                MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
            }
        }

        private IWin32Window GetMainWindowOwner()
        {
            try
            {
                return Application.Gui?.MainWindow;
            }
            catch
            {
                return null;
            }
        }
    }
}
