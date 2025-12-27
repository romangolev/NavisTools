using System;
using System.Windows.Forms;

namespace NavisTools.UI
{
    /// <summary>
    /// Configuration dialog for NavisTools settings
    /// </summary>
    public partial class ConfigurationForm : Form
    {
        private SettingsModel _settings;
        private TextBox _parameterNameTextBox;
        private TextBox _categoryNameTextBox;
        private Button _okButton;
        private Button _cancelButton;
        private Label _parameterNameLabel;
        private Label _categoryNameLabel;

        public ConfigurationForm(SettingsModel settings)
        {
            _settings = settings;
            InitializeComponent();
            LoadSettings();
        }

        private void InitializeComponent()
        {
            this.Text = "NavisTools Settings";
            this.Size = new System.Drawing.Size(450, 220);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // Parent settings group
            var parentGroup = new GroupBox
            {
                Text = "Parent Value Settings",
                Location = new System.Drawing.Point(15, 15),
                Size = new System.Drawing.Size(405, 100)
            };

            _parameterNameLabel = new Label
            {
                Text = "Parameter Name:",
                Location = new System.Drawing.Point(15, 30),
                Size = new System.Drawing.Size(120, 20)
            };

            _parameterNameTextBox = new TextBox
            {
                Location = new System.Drawing.Point(140, 28),
                Size = new System.Drawing.Size(245, 20)
            };

            _categoryNameLabel = new Label
            {
                Text = "Category Name:",
                Location = new System.Drawing.Point(15, 60),
                Size = new System.Drawing.Size(120, 20)
            };

            _categoryNameTextBox = new TextBox
            {
                Location = new System.Drawing.Point(140, 58),
                Size = new System.Drawing.Size(245, 20)
            };

            parentGroup.Controls.AddRange(new Control[] { 
                _parameterNameLabel, 
                _parameterNameTextBox,
                _categoryNameLabel,
                _categoryNameTextBox
            });

            // Buttons
            _okButton = new Button
            {
                Text = "OK",
                Location = new System.Drawing.Point(255, 135),
                Size = new System.Drawing.Size(75, 23),
                DialogResult = DialogResult.OK
            };
            _okButton.Click += OkButton_Click;

            _cancelButton = new Button
            {
                Text = "Cancel",
                Location = new System.Drawing.Point(345, 135),
                Size = new System.Drawing.Size(75, 23),
                DialogResult = DialogResult.Cancel
            };

            this.Controls.AddRange(new Control[] { parentGroup, _okButton, _cancelButton });
            this.AcceptButton = _okButton;
            this.CancelButton = _cancelButton;
        }

        private void LoadSettings()
        {
            _parameterNameTextBox.Text = _settings.ParentParameterName;
            _categoryNameTextBox.Text = _settings.ParentCategoryName;
        }

        private void SaveSettings()
        {
            _settings.ParentParameterName = _parameterNameTextBox.Text.Trim();
            _settings.ParentCategoryName = _categoryNameTextBox.Text.Trim();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(_parameterNameTextBox.Text))
            {
                MessageBox.Show(this, 
                    "Parameter Name cannot be empty.", 
                    "Validation Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                _parameterNameTextBox.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            if (string.IsNullOrWhiteSpace(_categoryNameTextBox.Text))
            {
                MessageBox.Show(this, 
                    "Category Name cannot be empty.", 
                    "Validation Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                _categoryNameTextBox.Focus();
                this.DialogResult = DialogResult.None;
                return;
            }

            SaveSettings();
        }
    }
}
