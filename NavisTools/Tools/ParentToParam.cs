using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using NavisTools.Helpers;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Application = Autodesk.Navisworks.Api.Application;
using Autodesk.Navisworks.Api.Interop.ComApi;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;

namespace NavisTools.Tools
{
    public class ParentToParam
    {
        public static void ExecuteParentToParam(ModelItemCollection items, InwOpState10 cdoc)
        {
            // Check if document is available
            Document doc = Application.ActiveDocument;
            if (doc == null || doc.Models.Count == 0)
            {
                MessageBox.Show(Application.Gui.MainWindow, 
                    "Please open a document first.", 
                    "No Document", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

            // Check if any items are selected
            if (items == null || items.Count == 0)
            {
                MessageBox.Show(Application.Gui.MainWindow, 
                    "Please select at least one item.", 
                    "No Selection", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

			// Show balloon notification
			BalloonHelper.ShowBalloon("Adding Parent Name", $"Processing {items.Count} selected item(s)...");
			// ParentToParam.SelectParents();

			//Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectAll();
			// ModelItemCollection oModelColl = Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectedItems;

			foreach (ModelItem item in items)
			{
				// convert ModelItem to   COM Path
				InwOaPath citem = (InwOaPath)ComApiBridge.ToInwOaPath(item);
				// Get item's PropertyCategoryCollection
				InwGUIPropertyNode2 cpropcates = (InwGUIPropertyNode2)cdoc.GetGUIPropertyNode(citem, true);
				// create a new Category (PropertyDataCollection)
				InwOaPropertyVec newcate = (InwOaPropertyVec)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaPropertyVec, null, null);
				// create a new Property (PropertyData)
				InwOaProperty newprop = (InwOaProperty)cdoc.ObjectFactory(nwEObjectType.eObjectType_nwOaProperty, null, null);
				// set PropertyName
				newprop.name = "ParentName" + "_InternalName";
				// set PropertyDisplayName
				newprop.UserName = "ParentName";
				// set PropertyValue
				if (item.Parent != null)
				{
					newprop.value = item.Parent.DisplayName;
				}
				// add PropertyData to Category
				newcate.Properties().Add(newprop);
				// add CategoryData to item's CategoryDataCollection
				cpropcates.SetUserDefined(0, "NavisTools", "NavisTools" + "_InternalName", newcate);
			}

		}

		public static void SelectParents()
        {
            ModelItemCollection selectionModelItems = new ModelItemCollection();
            Application.ActiveDocument.CurrentSelection.SelectedItems.CopyTo(
            selectionModelItems);

            //Clear the current selection
            Application.ActiveDocument.CurrentSelection.Clear();
            //iterate through the ModelItem's in the selection
            foreach (ModelItem modelItem in selectionModelItems)
            {
                //Add parent to selection
                Application.ActiveDocument.CurrentSelection.Add(modelItem.Parent);
            }
        }

        public static void CreateParameter()
        {

        }
    }
}
