using Autodesk.Revit.DB;

namespace BoostYourBIMTerrificTools.DBSpy
{
    public class IdViewModel : TreeViewItemViewModel
    {

        public int IdInt { get; }
        public string CategoryName { get; }
        public string Name { get; }
        public IdViewModel(Autodesk.Revit.DB.ElementId id, TreeViewItemViewModel parent)
            : base(parent, false)
        {
            IdInt = id.IntegerValue;
            if (IdInt == -1)
            {
                Name = "<None>";
                return;
            }
            Element e = BoostYourBIMTerrificTools.Utils.doc.GetElement(id);
            if (e.Category == null)               
            {
                CategoryName = "";
            }
            else
            {
                CategoryName = e.Category.Name;
            }
            Name = e.Name;
        }

    }
}