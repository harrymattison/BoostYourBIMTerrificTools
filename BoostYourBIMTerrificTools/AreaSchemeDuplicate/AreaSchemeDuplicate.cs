using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using BoostYourBIMTerrificTools.SketchConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BoostYourBIMTerrificTools.AreaSchemeDuplicate
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class AreaSchemeDuplicate : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;
            var existingView = doc.ActiveView as ViewPlan;
            if (existingView.ViewType != ViewType.AreaPlan)
            {
                TaskDialog.Show("Error", "Activate an area plan before running this tool");
            }

            var areaScheme = existingView.AreaScheme;
            Level level = existingView.GenLevel;

            var areaLines = new FilteredElementCollector(doc, existingView.Id)
                .OfClass(typeof(CurveElement))
                .Cast<CurveElement>()
                .Where(q => q.CurveElementType == CurveElementType.AreaSeparation);

            var areas = new FilteredElementCollector(doc, existingView.Id)
                .OfClass(typeof(SpatialElement))
                .OfCategory(BuiltInCategory.OST_Areas)
                .Cast<Area>();

            var areaTags = new FilteredElementCollector(doc, existingView.Id)
                .OfCategory(BuiltInCategory.OST_AreaTags)
                .Cast<AreaTag>();

            ViewPlan newView = null;
            using (var t = new Transaction(doc, "Copy Area Scheme"))
            {
                t.Start();
                var newId = ElementTransformUtils.CopyElement(doc, areaScheme.Id, XYZ.Zero).First();
                var newAreaScheme = doc.GetElement(newId) as AreaScheme;
                var ctr = 1;
                while (true)
                {
                    try
                    {
                        newAreaScheme.Name = areaScheme.Name + " " + ctr;
                        break;
                    }
                    catch
                    {
                        ctr++;
                    }
                }
                newView = ViewPlan.CreateAreaPlan(doc, newAreaScheme.Id, level.Id);
                newView.Scale = existingView.Scale;

                ElementTransformUtils.CopyElements(existingView, areaLines.Select(q => q.Id).ToList(), newView, Transform.Identity, new CopyPasteOptions());

                // if NewAreaBoundaryLine had an option for "Apply Area Rules", then we might want to do it this way instead of CopyElements
                // but as it is, these two approaches seem equivalent
                // https://forums.autodesk.com/t5/revit-ideas/api-access-to-the-apply-area-rules-option-that-exists-in-the-ui/idi-p/12379943
                //foreach (var areaLine in areaLines)
                //{
                //    doc.Create.NewAreaBoundaryLine(areaLine.SketchPlane, areaLine.GeometryCurve, newView);
                //}

                foreach (var area in areas)
                {
                    var point = ((LocationPoint)area.Location).Point;
                    var newArea = doc.Create.NewArea(newView, new UV(point.X, point.Y));
                    newArea.get_Parameter(BuiltInParameter.AREA_TYPE).Set(area.get_Parameter(BuiltInParameter.AREA_TYPE).AsElementId());
                    foreach (var areaTag in areaTags.Where(q => q.Area.Id == area.Id))
                    {
                        var tagPoint = areaTag.TagHeadPosition;
                        if (areaTag.HasLeader)
                        {
                            tagPoint = areaTag.LeaderEnd;
                        }
                        var newTag = doc.Create.NewAreaTag(newView, newArea, new UV(tagPoint.X, tagPoint.Y));
                        newTag.TagHeadPosition = areaTag.TagHeadPosition;
                        newTag.AreaTagType = areaTag.AreaTagType;
                        newTag.HasLeader = areaTag.HasLeader;
                        newTag.TagOrientation = areaTag.TagOrientation;
                        if (newTag.TagOrientation == SpatialElementTagOrientation.Model)
                        {
                            newTag.RotationAngle = areaTag.RotationAngle;
                        }
                        if (newTag.HasLeader)
                        {
                            newTag.LeaderElbow = areaTag.LeaderElbow;
                        }
                    }
                }
                t.Commit();
            }
            uidoc.ActiveView = newView;
            return Result.Succeeded;
        }
    }
}