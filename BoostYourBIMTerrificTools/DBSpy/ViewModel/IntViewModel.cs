namespace BoostYourBIMTerrificTools.DBSpy
{
    public class IntViewModel : TreeViewItemViewModel
    {

        public int D { get; }
        public IntViewModel(int d, TreeViewItemViewModel parent)
            : base(parent, false)
        {
            D = d;
        }

    }
}