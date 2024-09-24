using Autodesk.Revit.DB;

namespace BoostYourBIMTerrificTools.DBSpy
{
    public class ElementArrayViewModel : TreeViewItemViewModel
    {

        public string Name { get; }
        public ElementArray Array { get; }
        public ElementArrayViewModel(string name, ElementArray array, TreeViewItemViewModel parent)
            : base(parent, true)
        {
            Name = name;
            Array = array;
        }
        protected override void LoadChildren()
        {
            foreach (Element e in Array)
                Children.Add(new ElementViewModel(e, this));
        }
    }
}