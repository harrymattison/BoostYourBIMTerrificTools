namespace BoostYourBIMTerrificTools.DBSpy
{
    public class DoubleViewModel : TreeViewItemViewModel
    {

        public double D { get; }
        public DoubleViewModel(double d, TreeViewItemViewModel parent)
            : base(parent, false)
        {
            D = d;
        }

    }
}