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
        }

        private void InitializeComponent()
        {
            // Suspend layout for faster initialization
            this.SuspendLayout();

            this.Text = "NavisTools Settings";
            this.ClientSize = new System.Drawing.Size(420, 180);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.AutoScaleMode = AutoScaleMode.Font;

            // Parent settings group
            var parentGroup = new GroupBox();
            parentGroup.Text = "Parent Value Settings";
            parentGroup.Location = new System.Drawing.Point(15, 15);
            parentGroup.Size = new System.Drawing.Size(390, 100);
            parentGroup.SuspendLayout();

            _parameterNameLabel = new Label();
            _parameterNameLabel.Text = "Parameter Name:";
            _parameterNameLabel.Location = new System.Drawing.Point(15, 30);
            _parameterNameLabel.Size = new System.Drawing.Size(120, 20);
            _parameterNameLabel.AutoSize = false;

            _parameterNameTextBox = new TextBox();
            _parameterNameTextBox.Location = new System.Drawing.Point(140, 28);
            _parameterNameTextBox.Size = new System.Drawing.Size(230, 20);
            _parameterNameTextBox.Text = _settings.ParentParameterName;

            _categoryNameLabel = new Label();
            _categoryNameLabel.Text = "Category Name:";
            _categoryNameLabel.Location = new System.Drawing.Point(15, 60);
            _categoryNameLabel.Size = new System.Drawing.Size(120, 20);
            _categoryNameLabel.AutoSize = false;

            _categoryNameTextBox = new TextBox();
            _categoryNameTextBox.Location = new System.Drawing.Point(140, 58);
            _categoryNameTextBox.Size = new System.Drawing.Size(230, 20);
            _categoryNameTextBox.Text = _settings.ParentCategoryName;

            parentGroup.Controls.Add(_parameterNameLabel);
            parentGroup.Controls.Add(_parameterNameTextBox);
            parentGroup.Controls.Add(_categoryNameLabel);
            parentGroup.Controls.Add(_categoryNameTextBox);
            parentGroup.ResumeLayout(false);

            // Buttons
            _okButton = new Button();
            _okButton.Text = "OK";
            _okButton.Location = new System.Drawing.Point(240, 130);
            _okButton.Size = new System.Drawing.Size(75, 23);
            _okButton.DialogResult = DialogResult.OK;
            _okButton.Click += OkButton_Click;

            _cancelButton = new Button();
            _cancelButton.Text = "Cancel";
            _cancelButton.Location = new System.Drawing.Point(330, 130);
            _cancelButton.Size = new System.Drawing.Size(75, 23);
            _cancelButton.DialogResult = DialogResult.Cancel;

            this.Controls.Add(parentGroup);
            this.Controls.Add(_okButton);
            this.Controls.Add(_cancelButton);
            this.AcceptButton = _okButton;
            this.CancelButton = _cancelButton;

            this.ResumeLayout(false);
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
