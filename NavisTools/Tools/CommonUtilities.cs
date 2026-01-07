using Autodesk.Navisworks.Api;

namespace NavisTools.Tools
{
    public class CommonUtilities
    {
        public static int CountSelectedItems()
        {
            int num = 0;
            ModelItemCollection modelItemCollection = new ModelItemCollection();
            foreach (ModelItem selectedItem in Application.ActiveDocument.CurrentSelection.SelectedItems)
            {
                ++num;
                modelItemCollection.Add(selectedItem);
            }
            Selection selection = new Selection(modelItemCollection);
            Application.ActiveDocument.CurrentSelection.Clear();
            Application.ActiveDocument.CurrentSelection.CopyFrom(selection);
            return num;
        }
    }
}
