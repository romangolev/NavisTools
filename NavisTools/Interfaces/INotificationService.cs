using System.Windows.Forms;

namespace NavisTools.Interfaces
{
    /// <summary>
    /// Provides user notification services (balloons, message boxes, etc.)
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Shows a balloon notification.
        /// </summary>
        /// <param name="title">The notification title.</param>
        /// <param name="message">The notification message.</param>
        /// <param name="icon">The icon type.</param>
        /// <param name="durationMs">Duration in milliseconds.</param>
        void ShowBalloon(string title, string message, ToolTipIcon icon = ToolTipIcon.Info, int durationMs = 3000);

        /// <summary>
        /// Shows a warning message to the user.
        /// </summary>
        /// <param name="message">The warning message.</param>
        /// <param name="title">The dialog title.</param>
        void ShowWarning(string message, string title = "Warning");

        /// <summary>
        /// Shows an error message to the user.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="title">The dialog title.</param>
        void ShowError(string message, string title = "Error");

        /// <summary>
        /// Shows an information message to the user.
        /// </summary>
        /// <param name="message">The information message.</param>
        /// <param name="title">The dialog title.</param>
        void ShowInfo(string message, string title = "Information");
    }
}
