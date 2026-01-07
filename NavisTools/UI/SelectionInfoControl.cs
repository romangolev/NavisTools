using Autodesk.Navisworks.Api;
using NavisTools.Tools;
using System;
using System.Windows.Forms;
using Application = Autodesk.Navisworks.Api.Application;

namespace NavisTools.UI
{
	/// <summary>
	/// Windows Forms UserControl for displaying selection information dynamically
	/// </summary>
	public partial class SelectionInfoControl : UserControl
	{
		#region Properties
		/// <summary>
		/// The ActiveDocument from Autodesk.Navisworks.Api.Application.ActiveDocument
		/// </summary>
		private Document ActiveDocument { get => Application.ActiveDocument; }
		#endregion

		#region Constructor
		public SelectionInfoControl()
		{
			InitializeComponent();

			// Only subscribe to events if not in designer mode
			if (!this.DesignMode)
			{
				SubscribeToEvents();
				UpdateSelectionInfo();
			}
		}
		#endregion

		#region Event Subscriptions
		private void SubscribeToEvents()
		{
			// Subscribe to selection changed event
			if (Application.ActiveDocument != null)
			{
				Application.ActiveDocument.CurrentSelection.Changed += OnSelectionChanged;
			}

			// Subscribe to document changed to handle when documents are opened/closed
			Application.ActiveDocumentChanged += OnActiveDocumentChanged;
		}

		private void OnActiveDocumentChanged(object sender, EventArgs e)
		{
			// Unsubscribe from old document if exists, then subscribe to new
			if (Application.ActiveDocument != null)
			{
				Application.ActiveDocument.CurrentSelection.Changed -= OnSelectionChanged;
				Application.ActiveDocument.CurrentSelection.Changed += OnSelectionChanged;
			}

			// Update on UI thread
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new Action(UpdateSelectionInfo));
			}
			else
			{
				UpdateSelectionInfo();
			}
		}

		private void OnSelectionChanged(object sender, EventArgs e)
		{
			// Pause or no active document check
			if (ActiveDocument == null)
			{
				return;
			}

			// Update on UI thread
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new Action(UpdateSelectionInfo));
			}
			else
			{
				UpdateSelectionInfo();
			}
		}

		private void RefreshButton_Click(object sender, EventArgs e)
		{
			UpdateSelectionInfo();
		}
		#endregion

		#region UI Update Methods
		private void UpdateSelectionInfo()
		{
			_itemsListView.Items.Clear();

			// Get selection info from the tool
			var selectionInfo = SelectionInfoTool.GetSelectionInfo();

			// Update summary
			_summaryLabel.Text = selectionInfo.Summary;
			_countValueLabel.Text = selectionInfo.Count.ToString();
			_volumeValueLabel.Text = $"{selectionInfo.TotalVolume:N3} m³";
			_areaValueLabel.Text = $"{selectionInfo.TotalArea:N2} m²";
			_lengthValueLabel.Text = $"{selectionInfo.TotalLength:N2} m";

			// Populate list view with items
			foreach (var item in selectionInfo.Items)
			{
				var listItem = new ListViewItem(item.DisplayName);
				listItem.SubItems.Add(item.Volume > 0 ? item.Volume.ToString("N3") : "-");
				listItem.SubItems.Add(item.Area > 0 ? item.Area.ToString("N2") : "-");
				listItem.SubItems.Add(item.Length > 0 ? item.Length.ToString("N2") : "-");
				_itemsListView.Items.Add(listItem);
			}
		}
		#endregion

		#region Cleanup
		/// <summary>
		/// Cleanup event subscriptions when control is disposed
		/// </summary>
		public void Cleanup()
		{
			Application.ActiveDocumentChanged -= OnActiveDocumentChanged;
			if (Application.ActiveDocument != null)
			{
				Application.ActiveDocument.CurrentSelection.Changed -= OnSelectionChanged;
			}
		}

		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			this.Dock = DockStyle.Fill;
		}
		#endregion
	}
}
