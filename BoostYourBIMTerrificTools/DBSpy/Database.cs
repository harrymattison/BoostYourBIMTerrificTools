using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BoostYourBIMTerrificTools.DBSpy
{
    public static class Database
    {

        public static List<BuiltInCategory> GetCategories()
        {
            Document doc = BoostYourBIMTerrificTools.Utils.doc;
            if (doc == null)
                return new List<BuiltInCategory>();

            List<BuiltInCategory> types = new List<BuiltInCategory>();

            List<ElementId> selected = new UIDocument(BoostYourBIMTerrificTools.Utils.doc).Selection.GetElementIds().ToList();

            try
            {
                foreach (BuiltInCategory bic in Enum.GetValues(typeof(BuiltInCategory)))
                {
                    FilteredElementCollector coll;

                    if (selected.Any())
                        coll = new FilteredElementCollector(doc, selected).OfCategory(bic);
                    else
                        coll = new FilteredElementCollector(doc).OfCategory(bic);

                    if (coll.Any())
                    {
                        if (!types.Any(q => q.ToString() == bic.ToString()))
                        {
                            types.Add(bic);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return types.OrderBy(q => q.ToString()).ToList();
        }

        public static List<Element> GetElements(BuiltInCategory bic)
        {
            Document doc = BoostYourBIMTerrificTools.Utils.doc;
            List<ElementId> selected = new UIDocument(doc).Selection.GetElementIds().ToList();

            List<Element> ret = new List<Element>();
            FilteredElementCollector coll;
            if (selected.Any())
            {
                coll = new FilteredElementCollector(doc, selected).OfCategory(bic);
            }
            else
            {
                coll = new FilteredElementCollector(doc).OfCategory(bic);
            }

            foreach (Element e in coll.OrderBy(q => Utils.GetName(q)))
            {
                ret.Add(e);
            }

            return ret;
        }

    }
}