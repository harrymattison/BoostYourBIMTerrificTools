using Autodesk.Revit.DB;
using System;

namespace BoostYourBIMTerrificTools.DBSpy
{
    public class LineViewModel : TreeViewItemViewModel
    {

        public Line _line { get; }
        public LineViewModel(Line line, TreeViewItemViewModel parent)
            : base(parent, true)
        {
            _line = line;
            XYZ0 = line.GetEndPoint(0);
            XYZ1 = line.GetEndPoint(1);
        }
        public XYZ XYZ0 { get; }

        public double X0
        {
            get { return Math.Round(XYZ0.X, 4); }
        }

        public double Y0
        {
            get { return Math.Round(XYZ0.Y, 4); }
        }

        public double Z0
        {
            get { return Math.Round(XYZ0.Z, 4); }
        }

        public XYZ XYZ1 { get; }

        public double X1
        {
            get { return Math.Round(XYZ1.X, 4); }
        }

        public double Y1
        {
            get { return Math.Round(XYZ1.Y, 4); }
        }

        public double Z1
        {
            get { return Math.Round(XYZ1.Z, 4); }
        }

    }
}