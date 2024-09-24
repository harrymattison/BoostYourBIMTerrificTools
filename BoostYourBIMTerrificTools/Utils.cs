using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoostYourBIMTerrificTools
{
    public static class Utils
    {
        public static string dllPath = null;
        public static Document doc = null;
        public static PushButton btnPinComment = null;
        public static PushButton btnKeyboardShortcutTutor = null;
#if !RELEASE2013
        public static DockablePaneId dockableSamplePaneId;
        public static DockablePaneId ModelComparePaneId;
#endif
        public static readonly string BOTTOM_CLIP_PLANE = "Bottom Clip Plane";
        public static readonly string TOP_CLIP_PLANE = "Top Clip Plane";
        public static readonly string VIEW_DEPTH = "View Depth";
        public static readonly string CUT_PLANE = "Cut Plane";
        public static List<string> PrintedSheetNumbers = new List<string>();
        public static FailureDefinitionId illegalViewRangeFailureId;

        public class OverwriteFamily : IFamilyLoadOptions
        {
            public bool OnFamilyFound(
              bool familyInUse,
              out bool overwriteParameterValues)
            {
                overwriteParameterValues = true;
                return true;
            }

            public bool OnSharedFamilyFound(
              Family sharedFamily,
              bool familyInUse,
              out FamilySource source,
              out bool overwriteParameterValues)
            {
                overwriteParameterValues = true;
                source = FamilySource.Family;
                return true;
            }
        }

        public static double DegreesToRadians(Double degrees)
        {
            return degrees * Math.PI / 180;
        }

        public static double RadiansToDegrees(Double radians)
        {
            return radians * 180.0 / Math.PI;
        }

        public class ViewScaleUpdater : IUpdater
        {
            // https://forums.autodesk.com/t5/revit-ideas/limit-usable-view-scales-via-view-templates/idi-p/10030238
            private UpdaterId viewScaleUpdaterId;
            public ViewScaleUpdater(AddInId id)
            {
                viewScaleUpdaterId = new UpdaterId(id, new Guid("F2FAF6B2-4C06-42d4-97C1-D2B4EB593EFF"));
            }
            public void Execute(UpdaterData data)
            {
                Document doc = data.GetDocument();
                List<ElementId> ids = new List<ElementId>();
                ids.AddRange(data.GetAddedElementIds());
                ids.AddRange(data.GetModifiedElementIds());
                foreach (ElementId id in ids)
                {
                    if (doc.GetElement(id) is View view)
                    {
                        if (!view.IsTemplate)
                            continue;

                        if (view.ViewType == ViewType.FloorPlan && 
                            view.Scale != 96 && 
                            view.Scale != 192)
                        {
                            FailureMessage failureMessage = new FailureMessage(illegalViewRangeFailureId);
                            failureMessage.SetFailingElement(id);
                            doc.PostFailure(failureMessage);
                        }

                    }
                }
            }
            public string GetAdditionalInformation() { return "ViewScaleUpdater"; }
            public ChangePriority GetChangePriority() { return ChangePriority.FloorsRoofsStructuralWalls; }
            public UpdaterId GetUpdaterId() { return viewScaleUpdaterId; }
            public string GetUpdaterName() { return "ViewScaleUpdater"; }
        }

        public static ModelLine makeLine(Document doc, Line line)
        {
            return makeLine(doc, line.GetEndPoint(0), line.GetEndPoint(1));
        }
        public static ModelCurve makeCurve(Document doc, Curve curve)
        {
            if (curve is Line line)
                return makeLine(doc, line.GetEndPoint(0), line.GetEndPoint(1));
            if (curve is Arc arc)
                return makeArc(doc, arc);
            return null;
        }

        public static ModelArc makeArc(Document doc, Arc arc)
        {
            if (arc == null)
                return null;

            ModelArc ret = null;
            using (Transaction t = new Transaction(doc, "g"))
            {

                bool started = false;
                try
                {
                    t.Start();
                    started = true;
                }
                catch
                { }

                ret = (ModelArc)doc.Create.NewModelCurve(arc, 
                    SketchPlane.Create(doc, Plane.CreateByThreePoints(arc.GetEndPoint(0), arc.Evaluate(0.5, true), arc.GetEndPoint(1))));

                if (started)
                    t.Commit();
            }
            return ret;
        }

        public static ModelLine makeLine(Document doc, XYZ pt1, XYZ pt2)
        {
            if (pt1 == null || pt2 == null)
                return null;

            if (pt1.DistanceTo(pt2) < 0.01)
                return null;

            ModelLine ret = null;
            using (Transaction t = new Transaction(doc, "g"))
            {

                bool started = false;
                try
                {
                    t.Start();
                    started = true;
                }
                catch
                { }

                ret = (ModelLine)doc.Create.NewModelCurve(Line.CreateBound(pt1, pt2), SketchPlane.Create(doc, makePlane(doc.Application, pt1, pt2)));

                if (started)
                    t.Commit();
            }
            return ret;
        }
        public static Plane makePlane(Autodesk.Revit.ApplicationServices.Application app, XYZ pt1, XYZ pt2)
        {
            XYZ v = pt1.Subtract(pt2);
            double dxy = Math.Abs(v.X) + Math.Abs(v.Y);
            XYZ w = (dxy > 0.0001) ? XYZ.BasisZ : XYZ.BasisY;
            XYZ norm = v.CrossProduct(w).Normalize();
#if RELEASE2013 || RELEASE2014 || RELEASE2015 || RELEASE2016
            return app.Create.NewPlane(norm, pt1);
#else
            return Plane.CreateByNormalAndOrigin(norm, pt1);
#endif
        }
        public class NameIDObject
        {
            public NameIDObject(string name, int idValue)
            {
                Name = name;
                IdValue = idValue;
            }
            public string Name { get; set; }
            public int IdValue { get; set; }
        }

        public static Level GetViewRangeLevel(Document doc, ElementId id, Level levelBelow)
        {
            if (id == ElementId.InvalidElementId)
                return null;

            if (id.IntegerValue == -4)
                return levelBelow;

            return doc.GetElement(id) as Level;
        }

        public static List<Solid> GetElementSolids(GeometryElement geomElem)
        {
            if (geomElem == null)
                return new List<Solid>();

            List<Solid> solids = new List<Solid>();
            foreach (GeometryObject geomObj in geomElem)
            {
                if (geomObj is Solid solid)
                {
                    if (solid.Faces.Size > 0)
                    {
                        solids.Add(solid);
                        continue;
                    }
                }
                if (geomObj is GeometryInstance geomInst)
                {
                    solids.AddRange(GetElementSolids(geomInst.GetInstanceGeometry()));
                }
            }
            return solids;
        }

        public static DetailCurve makeDetailLine(View view, XYZ pt1, XYZ pt2)
        {
            DetailCurve ret = null;
#if !RELEASE2013 && !RELEASE2014 && !RELEASE2015 && !RELEASE2016
            Plane viewPlane = Plane.CreateByNormalAndOrigin(view.ViewDirection, view.Origin);
#else
            Plane viewPlane = view.Document.Application.Create.NewPlane(view.ViewDirection, view.Origin);
#endif

            pt1 = ProjectOnto(viewPlane, pt1);
            pt2 = ProjectOnto(viewPlane, pt2);

            using (Transaction t = new Transaction(view.Document, "g"))
            {
                bool started = false;

                if (!doc.IsModifiable)
                {
                    t.Start();
                    started = true;
                }

#if RELEASE2013
                ret = view.Document.Create.NewDetailCurve(view, view.Document.Application.Create.NewLineBound(pt1, pt2));
#else
                ret = view.Document.Create.NewDetailCurve(view, Line.CreateBound(pt1, pt2));
#endif

                if (started)
                    t.Commit();
            }
            return ret;
        }

        public static XYZ ProjectOnto(Plane plane, XYZ p)
        {
            double d = SignedDistanceTo(plane, p);
            if (d == 0)
                return p;
            XYZ q = p - d * plane.Normal;
            return q;
        }

        private static double SignedDistanceTo(Plane plane, XYZ p)
        {
            XYZ v = p - plane.Origin;
            return plane.Normal.DotProduct(v);
        }

        public static void ExpandToContain(
            this BoundingBoxXYZ bb,
            XYZ p)
        {
            bb.Min = new XYZ(Math.Min(bb.Min.X, p.X),
                Math.Min(bb.Min.Y, p.Y),
                Math.Min(bb.Min.Z, p.Z));

            bb.Max = new XYZ(Math.Max(bb.Max.X, p.X),
                Math.Max(bb.Max.Y, p.Y),
                Math.Max(bb.Max.Z, p.Z));
        }

        public static void ExpandToContain(
            this BoundingBoxXYZ bb,
            BoundingBoxXYZ other)
        {
            bb.ExpandToContain(other.Min);
            bb.ExpandToContain(other.Max);
        }

        public class GenericSelectionFilter : ISelectionFilter
        {
            BuiltInCategory bic = BuiltInCategory.INVALID;

            public GenericSelectionFilter(BuiltInCategory _bic)
            {
                bic = _bic;
            }

            public bool AllowElement(Element e)
            {
                if (e.Category == null)
                    return false;

                if (e.Category.Id.IntegerValue == (int)bic)
                    return true;

                return false;
            }
            public bool AllowReference(Reference r, XYZ point)
            {
                return true;
            }
        }


        public class MultiCatSelectionFilter : ISelectionFilter
        {
            IList<ElementId> catList = null;
            public MultiCatSelectionFilter(List<ElementId> cats)
            {
                catList = cats;
            }

            public bool AllowElement(Element e)
            {
                if (e.Category == null)
                    return false;

                if (catList.Contains(e.Category.Id))
                    return true;

                return false;
            }
            public bool AllowReference(Reference r, XYZ point)
            {
                return true;
            }
        }
    }

}
