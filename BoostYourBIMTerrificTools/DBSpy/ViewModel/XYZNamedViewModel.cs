using Autodesk.Revit.DB;

namespace BoostYourBIMTerrificTools.DBSpy
{
    public class XYZNamedViewModel : XYZViewModel
    {
        public XYZNamedViewModel(string name, XYZ xyz, TreeViewItemViewModel parent) : base(xyz, parent)
        {
            Name = name;
        }

        public string Name
        {
            get;
        }

    }
}