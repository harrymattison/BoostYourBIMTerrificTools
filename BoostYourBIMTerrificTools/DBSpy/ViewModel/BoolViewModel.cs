namespace BoostYourBIMTerrificTools.DBSpy
{
    public class BoolViewModel : TreeViewItemViewModel
    {

        public string D { get; }
        public BoolViewModel(bool d, TreeViewItemViewModel parent)
            : base(parent, false)
        {
            D = d.ToString();
        }

    }
}