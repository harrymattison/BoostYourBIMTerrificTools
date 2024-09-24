using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace BoostYourBIMTerrificTools.SelectIntersecting
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class selectIntersecting : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;

            Document doc = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = new UIDocument(doc);

            Element eSelected = null;
            try
            {
                eSelected = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element));
            }
            catch
            { return Result.Cancelled; }

            BoundingBoxXYZ bbox = eSelected.get_BoundingBox(null);
            if (bbox == null)
            {
                TaskDialog.Show("Error", "Selected element does not have a bounding box for its 3D geometry.");
                return Result.Cancelled;
            }
            Outline outline = new Outline(bbox.Min, bbox.Max);

            IEnumerable<Element> bboxIntersect = new FilteredElementCollector(doc)
                .WherePasses(new BoundingBoxIntersectsFilter(outline));

            IList<string> badCat = new List<string>();
            foreach (Element element in bboxIntersect)
            {
                if (!ElementIntersectsElementFilter.IsElementSupported(element) || !ElementIntersectsElementFilter.IsCategorySupported(element))
                {
                    if (element.Category != null && !badCat.Contains(element.Category.Name))
                    {
                        badCat.Add(element.Category.Name);
                    }
                }
            }

            bool includeBbox = false;

            if (badCat.Count > 0)
            {
                string q = "";
                foreach (string s in badCat)
                {
                    q += s + ", ";
                }
                q = q.Remove(q.Length - 2);

                TaskDialog td = new TaskDialog("Error");
                td.MainInstruction = "Elements in the following categories have a bounding box that intersects the bounding box of the element you selected. It cannot be determined if they physically intersect with the element you selected.\n\n" + q +
                    "\n\nDo you want to select all elements whose bounding box intersects the bounding box of the element you selected?";
                td.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;
                if (td.Show() == TaskDialogResult.Yes)
                    includeBbox = true;
            }

            List<Element> selSet = new List<Element>();

            IList<string> elementsNotSupported = new List<string>();
            IList<string> catNotSupported = new List<string>();

            if (includeBbox)
            {
                foreach (Element e in bboxIntersect)
                {
                    selSet.Add(e);
                }
            }
            else
            {
                foreach (Element e in bboxIntersect)
                {
                    if (!ElementIntersectsElementFilter.IsCategorySupported(e))
                    {
                        if (!catNotSupported.Contains(e.Category.Name))
                        {
                            catNotSupported.Add(e.Category.Name);
                        }
                        continue;
                    }

                    if (!ElementIntersectsElementFilter.IsElementSupported(e))
                    {
                        if (!elementsNotSupported.Contains(e.Category.Name))
                        {
                            elementsNotSupported.Add(e.Category.Name);
                        }
                        continue;
                    }

                    IList<ElementId> myList = new List<ElementId>();
                    myList.Add(e.Id);

                    if (!ElementIntersectsElementFilter.IsCategorySupported(eSelected))
                    {
                        if (!catNotSupported.Contains(eSelected.Category.Name))
                        {
                            catNotSupported.Add(eSelected.Category.Name);
                        }
                        continue;
                    }

                    if (!ElementIntersectsElementFilter.IsElementSupported(eSelected))
                    {
                        if (!elementsNotSupported.Contains(eSelected.Category.Name))
                        {
                            elementsNotSupported.Add(eSelected.Category.Name);
                        }
                        continue;
                    }

                    IList<Element> intersections = new FilteredElementCollector(doc, myList).WherePasses(new ElementIntersectsElementFilter(eSelected)).ToList();
                    int intersectionCount = intersections.Count;
                    if (intersectionCount == 1)
                        selSet.Add(e);
                }
            }

            string badE = "";
            foreach (string s in elementsNotSupported)
            {
                badE += s + "\n";
            }

            string badC = "";
            foreach (string s in catNotSupported)
            {
                badC += s + "\n";
            }

            string errors = "";
            if (badE.Length > 0)
                errors += "These elements are not supported by Element Intersection filters in the Revit API:\n" + badE;
            if (badC.Length > 0)
                errors += "These categories are not supported by Element Intersection filters in the Revit API:\n" + badC;

            if (errors.Length > 0)
                TaskDialog.Show("Errors", errors);

            uidoc.Selection.SetElementIds(selSet.Select(q => q.Id).ToList());

            TaskDialog.Show("Select Intersecting", selSet.Count + " intersecting elements found");

            uidoc.RefreshActiveView();

            return Result.Succeeded;
        }
    }
}
