using System.Windows.Media;

namespace BoostYourBIMTerrificTools.DBSpy
{
    public class ColorViewModel : TreeViewItemViewModel
    {
        public SolidColorBrush BrushColor { get; }
        public ColorViewModel(Autodesk.Revit.DB.Color revitColor, TreeViewItemViewModel parent)
            : base(parent, false)
        {
            BrushColor = new SolidColorBrush(new Color
            {
                R = revitColor.Red,
                G = revitColor.Green,
                B = revitColor.Blue,
                A = 255
            });
        }

    }
}