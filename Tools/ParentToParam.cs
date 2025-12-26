using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Api.Plugins;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Application = Autodesk.Navisworks.Api.Application;
using Autodesk.Navisworks.Api.Interop.ComApi;
using ComApiBridge = Autodesk.Navisworks.Api.ComApi.ComApiBridge;

namespace NavisTools
{
    public class ParentToParam
    {

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
