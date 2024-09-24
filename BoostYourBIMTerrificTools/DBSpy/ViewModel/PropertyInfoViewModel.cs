#if !RELEASE2013 && !RELEASE2014
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BoostYourBIMTerrificTools.DBSpy
{
    public class PropertyInfoViewModel : TreeViewItemViewModel
    {
        readonly PropertyInfo _pi;
        readonly ElementId _id;
        readonly object _o;

        public PropertyInfoViewModel(PropertyInfo pi, ElementViewModel parent)
            : base(parent, true)
        {
            _pi = pi;
            _id = parent.Id;
        }

        public PropertyInfoViewModel(PropertyInfo pi, SolidViewModel parent)
    : base(parent, true)
        {
            _pi = pi;
        }

        public PropertyInfoViewModel(PropertyInfo pi, ElementId id, object o, PropertyInfoViewModel parent)
            : base(parent, true)
        {
            _pi = pi;
            _id = id;
            _o = o;
        }

        public string Name
        {
            get { return _pi.Name; }
        }

        protected override void LoadChildren()
        {
            if (Parent is ElementViewModel evm)
            {
                Element e = BoostYourBIMTerrificTools.Utils.doc.GetElement(evm.Id);
                object propertyValue = GetPropertyValue(e);
                if (propertyValue == null)
                {
                    Children.Add(new StringViewModel("null", evm));
                    return;
                }
                else if (propertyValue is double d)
                {
                    Children.Add(new DoubleViewModel(d, evm));
                    return;
                }
                else if (propertyValue is string s)
                {
                    Children.Add(new StringViewModel(s, evm));
                    return;
                }
                else if (propertyValue is int i)
                {
                    Children.Add(new IntViewModel(i, evm));
                    return;
                }
                else if (propertyValue is ElementId id)
                {
                    Children.Add(new IdViewModel(id, evm));
                    return;
                }
                else if (propertyValue is WorksetId wid)
                {
                    Children.Add(new WorksetIdViewModel(wid, evm));
                    return;
                }
                else if (propertyValue is bool b)
                {
                    Children.Add(new BoolViewModel(b, evm));
                    return;
                }
                else if (propertyValue is Color color)
                {
                    Children.Add(new ColorViewModel(color, this));
                }
                else if (propertyValue is GeometryElement ge)
                {
                    foreach (GeometryObject geomobj in ge)
                    {
                        if (geomobj is Line line)
                        {
                            Children.Add(new LineViewModel(line, this));
                        }
                        else if (geomobj is Solid solid)
                        {
                            SolidViewModel svm = new SolidViewModel(solid, this);
                            Children.Add(svm);
                            //List<PropertyInfo> solidList = solid.GetType().GetProperties().ToList().OrderBy(q => q.Name).ToList();
                            //if (solidList.Any())
                            //{
                            //    foreach (PropertyInfo pi in solidList)
                            //    {
                            //        Children.Add(new PropertyInfoViewModel(pi, svm));
                            //    }
                            //}
                        }
                    }
                }

                List<PropertyInfo> piList = propertyValue.GetType().GetProperties().ToList().OrderBy(q => q.Name).ToList();
                if (piList.Any())
                {
                    foreach (PropertyInfo pi in piList)
                    {
                        Children.Add(new PropertyInfoViewModel(pi, evm.Id, propertyValue, this));
                    }
                }
                List<MethodInfo> methodList = propertyValue.GetType().GetMethods().ToList().OrderBy(q => q.Name).ToList();
                List<string> methodNames = methodList.Select(q => q.Name).ToList();

                Children.Add(new StringViewModel(propertyValue.ToString(), evm));
            }
            else if (this is PropertyInfoViewModel pivm)
            {
                if (pivm._o == null)
                    return;

                object value = null;
                try
                {
                    if (_pi.Name == "JoinType")
                    {
                        object value0 = _pi.GetValue(pivm._o, new object[1] { 0 });
                        object value1 = _pi.GetValue(pivm._o, new object[1] { 1 });
                        Children.Add(new StringViewModel("0: " + value0.ToString(), this));
                        Children.Add(new StringViewModel("1: " + value1.ToString(), this));
                        return;
                    }
                    if (_pi.Name == "ElementsAtJoin")
                    {
                        ElementArray array0 = (ElementArray)_pi.GetValue(pivm._o, new object[1] { 0 });
                        ElementArray array1 = (ElementArray)_pi.GetValue(pivm._o, new object[1] { 1 });
                        Children.Add(new ElementArrayViewModel("End 0", array0, this));
                        Children.Add(new ElementArrayViewModel("End 1", array1, this));
                        return;
                    }
                    value = pivm._pi.GetValue(pivm._o);
                    if (value == null)
                        return;

                    if (value is XYZ xyz)
                    {
                        Children.Add(new XYZViewModel(xyz, this));
                        return;
                    }
                    if (value is Line line)
                    {
                        List<PropertyInfo> lineList = line.GetType().GetProperties().ToList().OrderBy(q => q.Name).ToList();
                        foreach (PropertyInfo pi in lineList)
                            Children.Add(new PropertyInfoViewModel(pi, pivm._id, line, pivm));
                        Children.Add(new XYZNamedViewModel("End 0", line.GetEndPoint(0), Parent));
                        Children.Add(new XYZNamedViewModel("End 1", line.GetEndPoint(1), Parent));
                        return;
                    }
                    else if (value is Arc arc)
                    {
                        List<PropertyInfo> lineList = arc.GetType().GetProperties().ToList().OrderBy(q => q.Name).ToList();
                        foreach (PropertyInfo pi in lineList)
                            Children.Add(new PropertyInfoViewModel(pi, pivm._id, arc, pivm));
                        Children.Add(new XYZNamedViewModel("End 0", arc.GetEndPoint(0), Parent));
                        Children.Add(new XYZNamedViewModel("Midpoint", arc.Evaluate(0.5, true), Parent));
                        Children.Add(new XYZNamedViewModel("End 1", arc.GetEndPoint(1), Parent));
                        return;
                    }
                    else if (value is byte byt)
                    {
                        Children.Add(new IntViewModel(byt, this));
                        return;
                    }
                    else
                    {
                        Children.Add(new StringViewModel(value.ToString(), this));
                    }    
                }
                catch
                { }

                object propertyValue = GetPropertyValue(value);
                if (propertyValue != null)
                {
                    if (propertyValue is double d)
                    {
                        Children.Add(new DoubleViewModel(d, pivm));
                        return;
                    }
                    else if (propertyValue is string s)
                    {
                        Children.Add(new StringViewModel(s, pivm));
                        return;
                    }
                    else if (propertyValue is int i)
                    {
                        Children.Add(new IntViewModel(i, pivm));
                        return;
                    }
                    else if (propertyValue is ElementId id)
                    {
                        Children.Add(new IdViewModel(id, pivm));
                        return;
                    }
                    else if (propertyValue is bool b)
                    {
                        Children.Add(new BoolViewModel(b, pivm));
                        return;
                    }
                }

                List<PropertyInfo> piList = value.GetType().GetProperties().ToList().OrderBy(q => q.Name).ToList();
                foreach (PropertyInfo pi in piList)
                    Children.Add(new PropertyInfoViewModel(pi, pivm._id, propertyValue, pivm));
            }
        }

        private object GetPropertyValue(object e)
        {
            if (e is double d)
                return d;

            if (e is int i)
                return i;

            if (e is bool b)
                return b;

            object propertyValue = null;
            try
            {
                if (_pi.Name == "Geometry")
                    propertyValue = _pi.GetValue(e, new object[1] { new Options() });
                else if (_pi.Name == "BoundingBox")
                    propertyValue = _pi.GetValue(e, new object[1] { new UIDocument(BoostYourBIMTerrificTools.Utils.doc).ActiveView });
                else if (_pi.Name == "Item")
                    propertyValue = _pi.GetValue(e, new object[1] { 0 });
                else if (_pi.Name == "Location")
                    propertyValue = _pi.GetValue(e);
                else
                    propertyValue = _pi.GetValue(e);
            }
            catch (Exception ex)
            {
            }
            return propertyValue;
        }


    }
}
#endif