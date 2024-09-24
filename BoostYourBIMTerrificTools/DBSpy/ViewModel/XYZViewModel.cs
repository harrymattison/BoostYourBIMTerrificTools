using Autodesk.Revit.DB;
using System;

namespace BoostYourBIMTerrificTools.DBSpy
{
    public class XYZViewModel : TreeViewItemViewModel
    {

        public XYZViewModel(XYZ xyz, TreeViewItemViewModel parent)
            : base(parent, false)
        {
            XYZ = xyz;
            X = Math.Round(xyz.X, 4);
            Y = Math.Round(xyz.Y, 4);
            Z = Math.Round(xyz.Z, 4);
        }

        public XYZ XYZ { get; }

        public double X
        {
            get;
        }

        public double Y
        {
            get;
        }

        public double Z
        {
            get;
        }
    }
}