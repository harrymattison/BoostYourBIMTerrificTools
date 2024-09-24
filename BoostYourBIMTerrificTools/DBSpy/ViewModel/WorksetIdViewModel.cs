using Autodesk.Revit.DB;

namespace BoostYourBIMTerrificTools.DBSpy
{
    public class WorksetIdViewModel : TreeViewItemViewModel
    {

        public int IdInt { get; }
        public string Name { get; }
        public WorksetIdViewModel(WorksetId d, TreeViewItemViewModel parent)
            : base(parent, false)
        {
            Document doc = BoostYourBIMTerrificTools.Utils.doc;
            if (!doc.IsWorkshared)
            {
                Name = "<Worksharing Not Enabled>";
                return;
            }

            IdInt = d.IntegerValue;
            if (IdInt == -1)
            {
                Name = "<None>";
                return;
            }
            Workset w = doc.GetWorksetTable().GetWorkset(d);
            Name = w.Name;
        }

    }
}