using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BoostYourBIMTerrificTools
{
    [Transaction(TransactionMode.Manual)]
    public class ViewRangeLinesSet : IExternalCommand
    {
        // adapted from https://boostyourbim.wordpress.com/2016/07/15/rtcna2016-wish-2-granted-part-2/

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (!(doc.ActiveView is ViewPlan viewPlan))
            {
                TaskDialog.Show("Error", "Activate a plan view before running this command");
                return Result.Cancelled;
            }

            List<ElementId> lineIds;
            try
            {
                lineIds = uidoc.Selection.PickObjects(
                    ObjectType.Element,
                    new Utils.GenericSelectionFilter(BuiltInCategory.OST_Lines),
                    "Select lines to use for setting view range")
                .Select(q => q.ElementId)
                .ToList();
            }
            catch
            {
                return Result.Cancelled;
            }

            using (Transaction t = new Transaction(doc, "Set View Range by Lines"))
            {
                t.Start();

                PlanViewRange range = viewPlan.GetViewRange();

                Level planLevel = viewPlan.GenLevel;
                List<Level> levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().OrderBy(q => q.Elevation).ToList();
                Level levelBelow = levels.LastOrDefault(q => q.Elevation < planLevel.Elevation);

                range = SetRange(range, lineIds, PlanViewPlane.BottomClipPlane, Utils.BOTTOM_CLIP_PLANE, levelBelow, viewPlan);
                range = SetRange(range, lineIds, PlanViewPlane.TopClipPlane, Utils.TOP_CLIP_PLANE, levelBelow, viewPlan);
                range = SetRange(range, lineIds, PlanViewPlane.CutPlane, Utils.CUT_PLANE, levelBelow, viewPlan);
                range = SetRange(range, lineIds, PlanViewPlane.ViewDepthPlane, Utils.VIEW_DEPTH, levelBelow, viewPlan);

                t.Commit();
            }

            return Result.Succeeded;
        }


        private PlanViewRange SetRange(PlanViewRange range, List<ElementId> lineIds, PlanViewPlane planViewPlane, string planeName, Level levelBelow, ViewPlan viewPlan)
        {
            PlanViewRange rangeIn = range;
            if (new FilteredElementCollector(viewPlan.Document, lineIds)
                .OfClass(typeof(CurveElement))
                .Cast<CurveElement>().FirstOrDefault(q => q.LineStyle.Name.Contains(planeName)) is CurveElement bottomCurve)
            {
                Level level = Utils.GetViewRangeLevel(viewPlan.Document, range.GetLevelId(planViewPlane), levelBelow);
#if RELEASE2013
                range.SetOffset(planViewPlane, bottomCurve.GeometryCurve.get_EndPoint(0).Z - level.Elevation);
#else
                range.SetOffset(planViewPlane, bottomCurve.GeometryCurve.GetEndPoint(0).Z - level.Elevation);
#endif
                try
                {
                    viewPlan.SetViewRange(range);
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", planeName + ": " + ex.Message);
                    return rangeIn;
                }
            }
            return range;
        }

    }
}
