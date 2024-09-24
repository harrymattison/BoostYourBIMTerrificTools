namespace BoostYourBIMTerrificTools.DBSpy
{
    public class StringViewModel : TreeViewItemViewModel
    {

        public string D { get; }
        public StringViewModel(string d, TreeViewItemViewModel parent)
            : base(parent, false)
        {
            D = d;
        }
    }
}