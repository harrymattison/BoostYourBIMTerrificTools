using Autodesk.Revit.DB;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace BoostYourBIMTerrificTools.DBSpy
{ 
    public class ElementViewModel : TreeViewItemViewModel
    {
        readonly Element _e;

        public ElementViewModel(Element e, TreeViewItemViewModel parent)
            : base(parent, true)
        {
            _e = e;
        }

        public ElementId Id
        {            
            get {
                if (!_e.IsValidObject)
                    return ElementId.InvalidElementId;

                return _e.Id; 
            }
        }

        public string Name
        {
            get {  return Utils.GetName(_e); } 
        }

        protected override void LoadChildren()
        {
            List<PropertyInfo> piList = _e.GetType().GetProperties().ToList().OrderBy(q => q.Name).ToList();
            List<string> propertyNames = piList.Select(q => q.Name).ToList();
            foreach (PropertyInfo pi in piList)
                Children.Add(new PropertyInfoViewModel(pi, this));

            //List<MethodInfo> methods = _e.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            //    .OrderBy(x => x.Name)
            //    .ToList();

        }
    }
}