namespace NavisTools.UI
{
    partial class SelectionInfoControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Cleanup();
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Main layout - reduced padding from 8 to 4
            this.Name = "SelectionInfoControl";
            this.Size = new System.Drawing.Size(450, 430);
            this.AutoScroll = true;
            this.Padding = new System.Windows.Forms.Padding(4);

            int yPos = 4;

            // Summary Group
            this._summaryGroup = new System.Windows.Forms.GroupBox();
            this._summaryGroup.Text = "Selection Summary";
            this._summaryGroup.Location = new System.Drawing.Point(4, yPos);
            this._summaryGroup.Size = new System.Drawing.Size(442, 160);
            this._summaryGroup.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            int innerY = 20;

            // Summary label
            this._summaryLabel = new System.Windows.Forms.Label();
            this._summaryLabel.Text = "No items selected";
            this._summaryLabel.Location = new System.Drawing.Point(10, innerY);
            this._summaryLabel.Size = new System.Drawing.Size(420, 20);
            this._summaryLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this._summaryLabel.Font = new System.Drawing.Font(this._summaryLabel.Font, System.Drawing.FontStyle.Bold);
            this._summaryGroup.Controls.Add(this._summaryLabel);

            innerY += 30;

            // Count row
            var countLabel = new System.Windows.Forms.Label();
            countLabel.Text = "Count:";
            countLabel.Location = new System.Drawing.Point(10, innerY);
            countLabel.Size = new System.Drawing.Size(100, 20);
            this._summaryGroup.Controls.Add(countLabel);

            this._countValueLabel = new System.Windows.Forms.Label();
            this._countValueLabel.Text = "0";
            this._countValueLabel.Location = new System.Drawing.Point(120, innerY);
            this._countValueLabel.Size = new System.Drawing.Size(310, 20);
            this._countValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this._countValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._summaryGroup.Controls.Add(this._countValueLabel);

            innerY += 25;

            // Volume row
            var volumeLabel = new System.Windows.Forms.Label();
            volumeLabel.Text = "Total Volume:";
            volumeLabel.Location = new System.Drawing.Point(10, innerY);
            volumeLabel.Size = new System.Drawing.Size(100, 20);
            this._summaryGroup.Controls.Add(volumeLabel);

            this._volumeValueLabel = new System.Windows.Forms.Label();
            this._volumeValueLabel.Text = "0.000 m³";
            this._volumeValueLabel.Location = new System.Drawing.Point(120, innerY);
            this._volumeValueLabel.Size = new System.Drawing.Size(310, 20);
            this._volumeValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this._volumeValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._summaryGroup.Controls.Add(this._volumeValueLabel);

            innerY += 25;

            // Area row
            var areaLabel = new System.Windows.Forms.Label();
            areaLabel.Text = "Total Area:";
            areaLabel.Location = new System.Drawing.Point(10, innerY);
            areaLabel.Size = new System.Drawing.Size(100, 20);
            this._summaryGroup.Controls.Add(areaLabel);

            this._areaValueLabel = new System.Windows.Forms.Label();
            this._areaValueLabel.Text = "0.00 m²";
            this._areaValueLabel.Location = new System.Drawing.Point(120, innerY);
            this._areaValueLabel.Size = new System.Drawing.Size(310, 20);
            this._areaValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this._areaValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._summaryGroup.Controls.Add(this._areaValueLabel);

            innerY += 25;

            // Length row
            var lengthLabel = new System.Windows.Forms.Label();
            lengthLabel.Text = "Total Length:";
            lengthLabel.Location = new System.Drawing.Point(10, innerY);
            lengthLabel.Size = new System.Drawing.Size(100, 20);
            this._summaryGroup.Controls.Add(lengthLabel);

            this._lengthValueLabel = new System.Windows.Forms.Label();
            this._lengthValueLabel.Text = "0.00 m";
            this._lengthValueLabel.Location = new System.Drawing.Point(120, innerY);
            this._lengthValueLabel.Size = new System.Drawing.Size(310, 20);
            this._lengthValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this._lengthValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this._summaryGroup.Controls.Add(this._lengthValueLabel);

            this.Controls.Add(this._summaryGroup);

            yPos += 170;

            // Refresh button
            this._refreshButton = new System.Windows.Forms.Button();
            this._refreshButton.Text = "Refresh";
            this._refreshButton.Location = new System.Drawing.Point(4, yPos);
            this._refreshButton.Size = new System.Drawing.Size(75, 25);
            this._refreshButton.Click += RefreshButton_Click;
            this.Controls.Add(this._refreshButton);

            yPos += 30;

            // Items Group with ListView - adjusted to fill width
            this._itemsGroup = new System.Windows.Forms.GroupBox();
            this._itemsGroup.Text = "Selected Items";
            this._itemsGroup.Location = new System.Drawing.Point(4, yPos);
            this._itemsGroup.Size = new System.Drawing.Size(442, 200);
            this._itemsGroup.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;

            this._itemsListView = new System.Windows.Forms.ListView();
            this._itemsListView.View = System.Windows.Forms.View.Details;
            this._itemsListView.FullRowSelect = true;
                this._itemsListView.GridLines = true;
                this._itemsListView.Location = new System.Drawing.Point(6, 20);
                this._itemsListView.Size = new System.Drawing.Size(430, 174);
                this._itemsListView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;

                // Adjusted column widths to better use available space
                this._itemsListView.Columns.Add("Name", 220);
                this._itemsListView.Columns.Add("Volume", 70, System.Windows.Forms.HorizontalAlignment.Right);
                this._itemsListView.Columns.Add("Area", 70, System.Windows.Forms.HorizontalAlignment.Right);
                this._itemsListView.Columns.Add("Length", 70, System.Windows.Forms.HorizontalAlignment.Right);

                this._itemsGroup.Controls.Add(this._itemsListView);
                this.Controls.Add(this._itemsGroup);

                this.ResumeLayout(false);
            }

        #endregion

        private System.Windows.Forms.Label _summaryLabel;
        private System.Windows.Forms.Label _countValueLabel;
        private System.Windows.Forms.Label _volumeValueLabel;
        private System.Windows.Forms.Label _areaValueLabel;
        private System.Windows.Forms.Label _lengthValueLabel;
        private System.Windows.Forms.ListView _itemsListView;
        private System.Windows.Forms.Button _refreshButton;
        private System.Windows.Forms.GroupBox _summaryGroup;
        private System.Windows.Forms.GroupBox _itemsGroup;
    }
}
