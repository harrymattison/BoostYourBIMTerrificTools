using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using static BoostYourBIMTerrificTools.Utils;

namespace BoostYourBIMTerrificTools.CurtainGridFromLine
{
    [Transaction(TransactionMode.Manual)]
    public class CurtainGridFromLine : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (!(doc.ActiveView is ViewSection))
            {
                TaskDialog.Show("Error", "Run this tool in a section or elevation view");
                return Result.Cancelled;
            }
            List<Line> lines = uidoc.Selection.PickObjects(ObjectType.Element, new GenericSelectionFilter(BuiltInCategory.OST_Lines))
                .Select(q => doc.GetElement(q))
                .Cast<CurveElement>()
                .Select(q => q.GeometryCurve)
                .Where(q => q is Line)
                .Cast<Line>()
                .ToList();

            Wall wall = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element,
                new GenericSelectionFilter(BuiltInCategory.OST_Walls))) as Wall;

            CurtainGrid grid = wall.CurtainGrid;
            if (grid == null)
            {
                TaskDialog.Show("Error", "Wall is not a curtain wall");
                return Result.Cancelled;
            }

            foreach (Line line in lines)
            {
                impl(grid, line);
            }

            return Result.Succeeded;
        }

        private void impl(CurtainGrid grid, Line line)
        {
            bool isU = isParallelTo(doc, line, grid.GetUGridLineIds());
            bool isV = isParallelTo(doc, line, grid.GetVGridLineIds());

            if (!isU && !isV)
            {
                return;
            }

            using (var t = new Transaction(doc, "Create Curtain Grid"))
            {
                t.Start();
                CurtainGridLine gridLine = null;
                var lineParameter = 0.01;
                while (true) // find a point on the line that is on the wall
                // handles the case when the endpoints are off the wall
                {
                    try
                    {
                        gridLine = grid.AddGridLine(isU, line.Evaluate(lineParameter, true), true);
                        break;
                    }
                    catch
                    {
                        lineParameter += 0.1;
                    }
                    if (lineParameter > 1)
                    { break; }
                }
                XYZ previousPoint = line.GetEndPoint(0);
                for (var d = 0.02; d <= 1; d += 0.05)
                {
                    var thisPoint = line.Evaluate(d, true);
                    var segment = Line.CreateBound(previousPoint, thisPoint);
                    try 
                    {
                        gridLine.AddSegment(segment);
                    }
                    catch { } // for situations where the line extends past the wall
                    previousPoint = thisPoint;
                }
                t.Commit();
            }
        }

        private bool isParallelTo(Document doc, Line line, ICollection<ElementId> ids)
        {
            var uline = (doc.GetElement(ids.First()) as CurtainGridLine).FullCurve as Line;
            if (line.Direction.IsAlmostEqualTo(uline.Direction) ||
                line.Direction.IsAlmostEqualTo(uline.Direction.Negate()))
                return true;
            return false;
        }
    }
}