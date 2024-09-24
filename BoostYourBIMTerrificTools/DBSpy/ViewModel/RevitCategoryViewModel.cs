using Autodesk.Revit.DB;

namespace BoostYourBIMTerrificTools.DBSpy
{
    public class RevitCategoryViewModel : TreeViewItemViewModel
    {
        readonly BuiltInCategory _bic;

        public RevitCategoryViewModel(BuiltInCategory bic) 
            : base(null, true)
        {
            _bic = bic;
        }

        public string Name
        {
            get { return _bic.ToString().Replace("OST_",""); }
        }

        protected override void LoadChildren()
        {
            foreach (object o in Database.GetElements(_bic))
            {
                if (o is Element e)
                    Children.Add(new ElementViewModel(e, this));
            }
        }
    }
}