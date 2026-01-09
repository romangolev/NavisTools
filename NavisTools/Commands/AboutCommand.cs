using NavisTools.Interfaces;
using System.Windows.Forms;
using System.Reflection;
using Application = Autodesk.Navisworks.Api.Application;

namespace NavisTools.Commands
{
    /// <summary>
    /// Command to show the About dialog.
    /// </summary>
    public class AboutCommand : ToolCommandBase
    {
        public const string Id = "ID_Button_About";

        public AboutCommand(IDocumentProvider documentProvider, ISelectionService selectionService, INotificationService notificationService) 
            : base(documentProvider, selectionService, notificationService)
        {
        }

        public override string CommandId => Id;

        public override bool CanExecute => true; // Always available

        public override int Execute()
        {
            ShowAboutDialog();
            return 0;
        }

        private void ShowAboutDialog()
        {
            // Get version from assembly
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            var versionString = $"Version {version.Major}.{version.Minor}.{version.Build}";

            var form = new Form
            {
                Text = "About NavisTools",
                ClientSize = new System.Drawing.Size(450, 310),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                StartPosition = FormStartPosition.CenterParent
            };

            // Title Label
            var titleLabel = new Label
            {
                Text = "NavisTools",
                Location = new System.Drawing.Point(20, 20),
                Size = new System.Drawing.Size(410, 35),
                Font = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            form.Controls.Add(titleLabel);

            // Version Label
            var versionLabel = new Label
            {
                Text = versionString,
                Location = new System.Drawing.Point(20, 60),
                Size = new System.Drawing.Size(410, 20),
                Font = new System.Drawing.Font("Segoe UI", 10),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            form.Controls.Add(versionLabel);

            // Description Label
            var descriptionLabel = new Label
            {
                Text = "Navisworks productivity tools for working with\nmodel properties and measurements.",
                Location = new System.Drawing.Point(20, 95),
                Size = new System.Drawing.Size(410, 45),
                Font = new System.Drawing.Font("Segoe UI", 9),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            form.Controls.Add(descriptionLabel);

            // Features Label
            var featuresLabel = new Label
            {
                Text = "Features:\n• Calculate Volume, Area, Length totals\n• Add parent names to selected items\n• Selection info panel\n• Revit parameter ID support (locale-independent)",
                Location = new System.Drawing.Point(50, 155),
                Size = new System.Drawing.Size(350, 85),
                Font = new System.Drawing.Font("Segoe UI", 9),
                AutoSize = false
            };
            form.Controls.Add(featuresLabel);

            // GitHub Link
            var githubLink = new LinkLabel
            {
                Text = "github.com/romangolev",
                Location = new System.Drawing.Point(20, 245),
                Size = new System.Drawing.Size(410, 20),
                Font = new System.Drawing.Font("Segoe UI", 9),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                LinkColor = System.Drawing.Color.FromArgb(0, 120, 215)
            };
            githubLink.LinkClicked += (s, e) =>
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = "https://github.com/romangolev",
                        UseShellExecute = true
                    });
                }
                catch
                {
                    // Ignore errors opening browser
                }
            };
            form.Controls.Add(githubLink);

            // OK Button
            var okButton = new Button
            {
                Text = "OK",
                Location = new System.Drawing.Point(185, 270),
                Size = new System.Drawing.Size(80, 28),
                DialogResult = DialogResult.OK
            };
            form.Controls.Add(okButton);
            form.AcceptButton = okButton;

            form.ShowDialog(Application.Gui.MainWindow);
        }
    }
}
