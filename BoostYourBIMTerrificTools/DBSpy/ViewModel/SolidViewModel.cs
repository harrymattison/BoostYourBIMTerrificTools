using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BoostYourBIMTerrificTools.DBSpy
{
    public class SolidViewModel : TreeViewItemViewModel
    {
        public Solid _solid { get; }
        public SolidViewModel(Solid solid, TreeViewItemViewModel parent)
            : base(parent, true)
        {
            _solid = solid;
        }
        protected override void LoadChildren()
        {
            List<PropertyInfo> lineList = _solid.GetType().GetProperties().ToList().OrderBy(q => q.Name).ToList();
            foreach (PropertyInfo pi in lineList)
                Children.Add(new PropertyInfoViewModel(pi, this));
        }
    }
}