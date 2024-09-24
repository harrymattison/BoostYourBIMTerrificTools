using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoostYourBIMTerrificTools
{
    [Transaction(TransactionMode.Manual)]
    public class ViewRangeLines : IExternalCommand
    {
		// adapted from https://boostyourbim.wordpress.com/2016/07/15/rtcna2016-wish-2-granted-part-one/

		public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
		{
			UIDocument uidoc = commandData.Application.ActiveUIDocument;
			Document doc = uidoc.Document;

            if (!(doc.ActiveView is ViewPlan viewPlan))
            {
                TaskDialog.Show("Error", "Activate a plan view before running this command");
                return Result.Cancelled;
            }

			Element e;

			try
			{
				e = doc.GetElement(uidoc.Selection.PickObject(
				ObjectType.Element,
				new Utils.GenericSelectionFilter(BuiltInCategory.OST_Viewers),
				"Select view"));
			}
			catch
			{
				return Result.Cancelled;
			}

			string viewName = e.Name;
			ViewFamilyType vft = doc.GetElement(e.GetTypeId()) as ViewFamilyType;
			View view = new FilteredElementCollector(doc)
				.OfClass(typeof(View))
				.Cast<View>()
				.FirstOrDefault(q => q.Name == viewName && q.ViewType.ToString() == vft.ViewFamily.ToString());

			if (view == null)
			{
				TaskDialog.Show("Error", "Could not find view");
				return Result.Cancelled;
			}

			PlanViewRange range = viewPlan.GetViewRange();

            uidoc.ActiveView = view;
            BoundingBoxXYZ bbox = GetElementsExtents(new FilteredElementCollector(doc, view.Id).WhereElementIsNotElementType().ToList());

			XYZ min = bbox.Min;
			XYZ max = bbox.Max;

			Category bottomClipLineStyle = doc.Settings.Categories.Cast<Category>().FirstOrDefault(q => q.Id.IntegerValue == (int)BuiltInCategory.OST_Lines)
				.SubCategories.Cast<Category>().FirstOrDefault(q => q.Name.Contains(Utils.BOTTOM_CLIP_PLANE));

			Category topClipLineStyle = doc.Settings.Categories.Cast<Category>().FirstOrDefault(q => q.Id.IntegerValue == (int)BuiltInCategory.OST_Lines)
				.SubCategories.Cast<Category>().FirstOrDefault(q => q.Name.Contains(Utils.TOP_CLIP_PLANE));

			Category viewDepthLineStyle = doc.Settings.Categories.Cast<Category>().FirstOrDefault(q => q.Id.IntegerValue == (int)BuiltInCategory.OST_Lines)
				.SubCategories.Cast<Category>().FirstOrDefault(q => q.Name.Contains(Utils.VIEW_DEPTH));

			Category cutPlaneLineStyle = doc.Settings.Categories.Cast<Category>().FirstOrDefault(q => q.Id.IntegerValue == (int)BuiltInCategory.OST_Lines)
					.SubCategories.Cast<Category>().FirstOrDefault(q => q.Name.Contains(Utils.CUT_PLANE));

			Level planLevel = viewPlan.GenLevel;
			List<Level> levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().OrderBy(q => q.Elevation).ToList();
			Level levelBelow = levels.LastOrDefault(q => q.Elevation < planLevel.Elevation);
			using (Transaction t = new Transaction(doc, "Make View Range Lines"))
			{
				t.Start();
				Level bottomLevel = Utils.GetViewRangeLevel(doc, range.GetLevelId(PlanViewPlane.BottomClipPlane), levelBelow);
				if (bottomLevel != null)
				{
					double bottomOffset = range.GetOffset(PlanViewPlane.BottomClipPlane);
					double z = bottomLevel.Elevation + bottomOffset;
					DetailCurve curve = Utils.makeDetailLine(view, new XYZ(min.X, min.Y, z), new XYZ(max.X, max.Y, z));
					if (bottomClipLineStyle != null)
					{
						curve.LineStyle = bottomClipLineStyle.GetGraphicsStyle(GraphicsStyleType.Projection);
					}
				}
				Level topLevel = Utils.GetViewRangeLevel(doc, range.GetLevelId(PlanViewPlane.TopClipPlane), levelBelow);
				if (topLevel != null)
				{
					double topOffset = range.GetOffset(PlanViewPlane.TopClipPlane);
					double z = topLevel.Elevation + topOffset;
					DetailCurve curve = Utils.makeDetailLine(view, new XYZ(min.X, min.Y, z), new XYZ(max.X, max.Y, z));
					if (topClipLineStyle != null)
					{
						curve.LineStyle = topClipLineStyle.GetGraphicsStyle(GraphicsStyleType.Projection);
					}
				}
				Level viewDepthLevel = Utils.GetViewRangeLevel(doc, range.GetLevelId(PlanViewPlane.ViewDepthPlane), levelBelow);
				if (viewDepthLevel != null)
				{
					double z = viewDepthLevel.Elevation + range.GetOffset(PlanViewPlane.ViewDepthPlane);
					DetailCurve curve = Utils.makeDetailLine(view, new XYZ(min.X, min.Y, z), new XYZ(max.X, max.Y, z));
					if (viewDepthLineStyle != null)
					{
						curve.LineStyle = viewDepthLineStyle.GetGraphicsStyle(GraphicsStyleType.Projection);
					}
				}
				Level cutPlaneLevel = Utils.GetViewRangeLevel(doc, range.GetLevelId(PlanViewPlane.CutPlane), levelBelow);
				if (cutPlaneLevel != null)
				{
					double z = cutPlaneLevel.Elevation + range.GetOffset(PlanViewPlane.CutPlane);
					DetailCurve curve = Utils.makeDetailLine(view, new XYZ(min.X, min.Y, z), new XYZ(max.X, max.Y, z));
					if (cutPlaneLineStyle != null)
					{
						curve.LineStyle = cutPlaneLineStyle.GetGraphicsStyle(GraphicsStyleType.Projection);
					}
				}

				t.Commit();
			}
			return Result.Succeeded;
        }


		private BoundingBoxXYZ GetElementsExtents(List<Element> elements)
		{
			IEnumerable<BoundingBoxXYZ> bbs = elements
				.Where(e => e.Category != null && e.get_BoundingBox(null) != null)
				.Select(e => e.get_BoundingBox(null));

			try
			{
				return bbs.Aggregate((a, b) => { a.ExpandToContain(b); return a; });
			}
			catch
			{
				return new BoundingBoxXYZ();
			}
		}

	}
}