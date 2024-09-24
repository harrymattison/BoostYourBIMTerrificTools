using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace BoostYourBIMTerrificTools.RoomAreaBoundaries
{
    [Transaction(TransactionMode.Manual)]
    public class roomAreaBoundaries : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            var app = commandData.Application.Application;
            var doc = commandData.Application.ActiveUIDocument.Document;
            var uidoc = new UIDocument(doc);
            List<Element> elements;
            try
            {
                elements = uidoc.Selection.PickObjects(ObjectType.Element).Select(q => doc.GetElement(q)).ToList();
            }
            catch
            {
                return Result.Cancelled;
            }

            using (var t = new Transaction(doc, "Room Area Boundaries"))
            {
                t.Start();
                foreach (var e in elements)
                {
                    if (e is Room room)
                    {
                        AreaLinesFromRoom(room);
                    }
                    else if (e is RoomTag roomTag)
                    {
                        AreaLinesFromRoom(roomTag.Room);
                    }
                    else if (e is Area area)
                    {
                        RoomLinesFromArea(area);
                    }
                    else if (e is AreaTag areaTag)
                    {
                        RoomLinesFromArea(areaTag.Area);
                    }
                    else if (e is Floor floor)
                    {
                        var viewPlan = GetViewPlan(doc);
                        if (viewPlan == null) return Result.Cancelled;

                        var profile = ((Sketch)doc.GetElement(floor.SketchId)).Profile;
                        foreach (CurveArray curveArray in profile)
                        {
                            foreach (Curve curve in curveArray)
                            {
                                MakeRoomOrAreaBoundaryLine(curve, viewPlan);
                            }
                        }
                    }
                    else if (e is Wall wall)
                    {
                        var viewPlan = GetViewPlan(doc);
                        if (viewPlan == null) return Result.Cancelled;

                        MakeRoomOrAreaBoundaryLine(((LocationCurve)wall.Location).Curve, viewPlan);
                    }
                    else if (e is ModelCurve modelCurve)
                    {
                        var viewPlan = GetViewPlan(doc);
                        if (viewPlan == null) return Result.Cancelled;

                        if (modelCurve.Category.Id.IntegerValue == (int)BuiltInCategory.OST_AreaSchemeLines)
                        {
                            MakeRoomBoundaryLine(modelCurve.GeometryCurve, viewPlan);
                        }
                        else // model line was selected
                        {
                            MakeRoomOrAreaBoundaryLine(modelCurve.GeometryCurve, viewPlan);
                        }
                    }

                }
                t.Commit();
            }

            return Result.Succeeded;
        }

        private ViewPlan GetViewPlan(Document doc)
        {
            if (doc.ActiveView is ViewPlan viewPlan)
            {
                return viewPlan;
            }
            else
            {
                TaskDialog.Show("Error", "Activate a plan view before running this command and selecting this element type");
                return null;
            }
        }

        private void RoomLinesFromArea(Area area)
        {
            var plan = area.Document.ActiveView as ViewPlan;
            if (plan == null)
            {
                TaskDialog.Show("Error", "Activate an floor plan view before running this command and selecting an area");
            }

            var bsListList = area.GetBoundarySegments(new SpatialElementBoundaryOptions());
            foreach (var bsList in bsListList)
            {
                foreach (var bs in bsList)
                {
                    MakeRoomBoundaryLine(bs.GetCurve(), plan);
                }
            }
        }

        private void AreaLinesFromRoom(Room room)
        {
            var plan = room.Document.ActiveView as ViewPlan;
            if (plan == null || plan.ViewType != ViewType.AreaPlan)
            {
                TaskDialog.Show("Error", "Activate an area plan view before running this command and selecting a room");
            }

            var listlistSegments = room.GetBoundarySegments(new SpatialElementBoundaryOptions());
            foreach (var listSegments in listlistSegments)
            {
                foreach (var segment in listSegments)
                {
                    var curve = segment.GetCurve();
                    MakeAreaBoundaryLine(curve, plan);
                }
            }
        }


        private void MakeRoomOrAreaBoundaryLine(Curve curve, ViewPlan viewPlan)
        {
            if (viewPlan.Document.ActiveView.ViewType == ViewType.AreaPlan)
                MakeAreaBoundaryLine(curve, viewPlan);
            else
                MakeRoomBoundaryLine(curve, viewPlan);
        }

        private void MakeAreaBoundaryLine(Curve curve, ViewPlan viewPlan)
        {
            var doc = viewPlan.Document;
            doc.Create.NewAreaBoundaryLine(
                SketchPlane.Create(doc, Utils.makePlane(doc.Application, curve.GetEndPoint(0), curve.GetEndPoint(1))),
                curve,
                viewPlan);
        }

        private void MakeRoomBoundaryLine(Curve curve, ViewPlan viewPlan)
        {
            var doc = viewPlan.Document;
            var curveArray = new CurveArray();
            curveArray.Append(curve);
            doc.Create.NewRoomBoundaryLines(
                SketchPlane.Create(doc, Utils.makePlane(doc.Application, curve.GetEndPoint(0), curve.GetEndPoint(1))),
                curveArray,
                viewPlan);
        }
    }
}
