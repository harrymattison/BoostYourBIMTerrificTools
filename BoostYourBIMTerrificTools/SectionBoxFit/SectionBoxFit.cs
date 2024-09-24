using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Autodesk.Revit.Attributes;
using System.IO;
using System.Windows.Media.Imaging;
using BoostYourBIMTerrificTools;

namespace SectionBoxFit
{
    [Transaction(TransactionMode.Manual)]
    public class sectionBoxFit : IExternalCommand
    {
        const double _eps = 1.0e-9;

        /// <summary>
        /// Return true if the given real number is almost zero.
        /// </summary>
        static bool IsAlmostZero(double a)
        {
            return _eps > Math.Abs(a);
        }

        /// <summary>
        /// Return true if the given vector is almost vertical.
        /// </summary>
        static bool IsVertical(XYZ v)
        {
            return IsAlmostZero(v.X) && IsAlmostZero(v.Y);
        }

        /// <summary>
        /// Return true if v and w are non-zero and perpendicular.
        /// </summary>
        static bool IsPerpendicular(XYZ v, XYZ w)
        {
            double a = v.GetLength();
            double b = v.GetLength();
            double c = Math.Abs(v.DotProduct(w));
            return _eps < a
              && _eps < b
              && _eps > c;

            // To take the relative lengths of a and b into 
            // account, you can scale the whole test, e.g
            // c * c < _eps * a * b... can you?
        }

        /// <summary>
        /// Return the signed volume of the paralleliped 
        /// spanned by the vectors a, b and c. In German, 
        /// this is also known as Spatprodukt.
        /// </summary>
        static double SignedParallelipedVolume(
          XYZ a,
          XYZ b,
          XYZ c)
        {
            return a.CrossProduct(b).DotProduct(c);
        }

        /// <summary>
        /// Return true if the three vectors a, b and c 
        /// form a right handed coordinate system, i.e.
        /// the signed volume of the paralleliped spanned 
        /// by them is positive.
        /// </summary>
        bool IsRightHanded(XYZ a, XYZ b, XYZ c)
        {
            return 0 < SignedParallelipedVolume(a, b, c);
        }

        /// <summary>
        /// Return the minimal aligned bounding box for 
        /// a Revit scope box element. The only 
        /// information we can obtain from the scope box 
        /// are its 12 boundary lines. Algorithm: Pick an 
        /// arbitrary line as the X axis and its starting 
        /// point as the origin. Find the three other 
        /// lines starting or ending at the origin, and 
        /// use them to define the Y and Z axes. If 
        /// necessary, swap Y and Z to form a
        /// right-handed coordinate system.
        /// </summary>

        bool GetVectorsFromElement(HostObject ho, out XYZ origin, out XYZ vx, out XYZ vy, out XYZ vz)
        {
            origin = null;
            vx = null;
            vy = null;
            vz = null;

            PlanarFace face = null;

            double thickness = 0;

            if (ho is Wall)
            {
                Wall w = ho as Wall;
                thickness = w.Width;
                Reference sideFaceRef = HostObjectUtils.GetSideFaces(w, ShellLayerType.Exterior).FirstOrDefault();
                Face f = w.GetGeometryObjectFromReference(sideFaceRef) as Face;
                face = f as PlanarFace;

                if (f != null && face == null)
                {
                    TaskDialog.Show("Error", "Must select a planar face.");
                    return false;
                }

                if (face == null) // curtain wall
                {
                    Application app = w.Document.Application;
                    Options options = app.Create.NewGeometryOptions();
                    options.IncludeNonVisibleObjects = true;
                    PlanarFace fBiggest = null;
                    foreach (GeometryObject obj in w.get_Geometry(options))
                    {
                        if (!(obj is Solid))
                            continue;
                        Solid s = obj as Solid;
                        double areaBiggest = 0;
                        if (s.Faces.Size == 1)
                        {
                            PlanarFace thisFace = s.Faces.get_Item(0) as PlanarFace;
                            if (thisFace.Area > areaBiggest)
                            {
                                areaBiggest = thisFace.Area;
                                fBiggest = thisFace;
                            }
                        }
                    }
                    face = fBiggest;
                }
            }
            else if (ho is CeilingAndFloor || ho is RoofBase)
            {
                Reference faceRef = HostObjectUtils.GetTopFaces(ho).FirstOrDefault();

                if (face.ComputeNormal(face.GetBoundingBox().Max.Subtract(face.GetBoundingBox().Min)).Z > 0)
                    faceRef = HostObjectUtils.GetTopFaces(ho).LastOrDefault();

                face = ho.GetGeometryObjectFromReference(faceRef) as PlanarFace;

                if (ho is Floor)
                {
                    Floor floor = ho as Floor;
                    thickness = floor.FloorType.GetCompoundStructure().GetWidth();
                }
            }

            if (face == null)
            {
                TaskDialog.Show("Error", "Could not get wall face.");
                return false;
            }

            BoundingBoxUV facebb = face.GetBoundingBox();

            UV min = facebb.Min;
            UV max = facebb.Max;

            origin = face.Evaluate(min);

            UV diag = max.Subtract(min);

            UV xDirEnd = new UV(facebb.Min.U + diag.U, facebb.Min.V);
            XYZ xDirEndXYZ = face.Evaluate(xDirEnd);

            XYZ zDirEndXYZ = face.Evaluate(new UV(facebb.Min.U, facebb.Min.V + diag.V));
            vx = xDirEndXYZ.Subtract(origin);
            vz = zDirEndXYZ.Subtract(origin);

            vy = vz.CrossProduct(vx).Normalize().Multiply(thickness);

            if (ho is Floor || ho is RoofBase || ho is Ceiling)
            {
                XYZ temp = vy;
                vy = vz;
                vz = temp;
                //if (vz.Z > 0)
                //    vz = vz.Negate();
            }

            return true;
        }

        void GetVectorsFromPickedEdges(Document doc, out XYZ origin, out XYZ vx, out XYZ vy, out XYZ vz)
        {
            UIDocument uidoc = new UIDocument(doc);
            origin = null;
            vx = null;
            vy = null;
            vz = null;
            Reference r1 = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Edge, "Select x");
            Element e = doc.GetElement(r1);
            Edge edge1 = e.GetGeometryObjectFromReference(r1) as Edge;
            vx = ((Line)edge1.AsCurve()).Direction;

            Reference r2 = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Edge, "Select y");
            Element e2 = doc.GetElement(r2);
            Edge edge2 = e2.GetGeometryObjectFromReference(r2) as Edge;
            vy = ((Line)edge2.AsCurve()).Direction;

            Reference r3 = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Edge, "Select z");
            Element e3 = doc.GetElement(r3);
            Edge edge3 = e3.GetGeometryObjectFromReference(r3) as Edge;
            vz = ((Line)edge3.AsCurve()).Direction;

            if (vz.Z < 0)
                vz = vz.Negate();

            if (edge1.AsCurve().GetEndPoint(0).IsAlmostEqualTo(edge2.AsCurve().GetEndPoint(0)))
            {
                origin = edge1.AsCurve().GetEndPoint(0);
            }
            if (edge1.AsCurve().GetEndPoint(0).IsAlmostEqualTo(edge2.AsCurve().GetEndPoint(1)))
            {
                origin = edge1.AsCurve().GetEndPoint(0);
            }
            if (edge1.AsCurve().GetEndPoint(1).IsAlmostEqualTo(edge2.AsCurve().GetEndPoint(0)))
            {
                origin = edge1.AsCurve().GetEndPoint(1);
            }
            if (edge1.AsCurve().GetEndPoint(1).IsAlmostEqualTo(edge2.AsCurve().GetEndPoint(1)))
            {
                origin = edge1.AsCurve().GetEndPoint(1);
            }

            if (origin == null)
            {
                TaskDialog.Show("e", "Origin = null");
                return;
            }

        }

        /// <summary>
        /// Return a suitable bounding box for a Revit 
        /// section view from the scope box position.
        /// Algorithm:
        /// 1. Assume a scope box can only be a rectangular 
        /// straight box. The user can rotate it on the 
        /// X, Y plane, but not make it slanted.
        /// 2. Retrieve a horizontal line at the bottom of 
        /// the box. Use one of its points as the origin.
        /// 3. Find the other two lines connected to the 
        /// origin.
        /// 4. Retrieve the vx, vy, vz vectors and lengths 
        /// from the three lines.
        /// 5. Create a new transform and bounding box 
        /// using this data.
        /// Revised algorithm:
        /// 1. Find vertical edge closest to viewer.
        /// 2. Use its bottom endpoint as the origin.
        /// 3. Find the other two edges emanating from the origin.
        /// 4. Use the three edges for the bounding box definition.
        /// </summary>

        void GetVectorsFromScopeBox(Element scopeBox, XYZ viewdirTowardViewer, out XYZ origin, out XYZ vx, out XYZ vy, out XYZ vz)
        {
            Document doc = scopeBox.Document;
            Application app = doc.Application;

            origin = null;
            vx = null;
            vy = null;
            vz = null;

            // Determine a possible view point outside the 
            // scope box extents in the direction of the 
            // viewer.

            BoundingBoxXYZ bb = scopeBox.get_BoundingBox(null);

            XYZ v = bb.Max - bb.Min;

            double size = v.GetLength();

            XYZ viewPoint = bb.Min
              + 10 * size * viewdirTowardViewer;

            // Retrieve scope box geometry, 
            // consisting of exactly twelve lines.

            Options opt = app.Create.NewGeometryOptions();
            GeometryElement geo = scopeBox.get_Geometry(opt);
            int n = geo.Count<GeometryObject>();

            if (12 != n)
            {
                throw new ArgumentException("Expected exactly"
                  + " 12 lines in scope box geometry");
            }

            // Determine origin as the bottom endpoint of 
            // the edge closest to the viewer, and vz as the 
            // vertical upwards pointing vector emanating
            // from it. (Todo: if several edges are equally 
            // close, pick the leftmost one, assuming the 
            // given view direction and Z is upwards.)

            double dist = double.MaxValue;
            XYZ p, q;

            foreach (GeometryObject obj in geo)
            {
                Debug.Assert(obj is Line,
                  "expected only lines in scope box geometry");

                Line line = obj as Line;

                p = line.GetEndPoint(0);
                q = line.GetEndPoint(1);
                v = q - p;

                if (IsVertical(v))
                {
                    if (q.Z < p.Z)
                    {
                        p = q;
                        v = v.Negate();
                    }

                    if (p.DistanceTo(viewPoint) < dist)
                    {
                        origin = p;
                        dist = origin.DistanceTo(viewPoint);
                        vz = v;
                    }
                }
            }

            // Find the other two axes emanating from the 
            // origin, vx and vy, and ensure right-handedness

            foreach (GeometryObject obj in geo)
            {
                Line line = obj as Line;

                p = line.GetEndPoint(0);
                q = line.GetEndPoint(1);
                v = q - p;

                if (IsVertical(v)) // already handled this
                {
                    continue;
                }

                if (p.IsAlmostEqualTo(origin)
                  || q.IsAlmostEqualTo(origin))
                {
                    if (q.IsAlmostEqualTo(origin))
                    {
                        v = v.Negate();
                    }
                    if (null == vx)
                    {
                        Debug.Assert(IsPerpendicular(vz, v),
                          "expected orthogonal lines in scope box geometry");

                        vx = v;
                    }
                    else
                    {
                        Debug.Assert(null == vy,
                          "expected exactly three orthogonal lines to originate in one point");

                        Debug.Assert(IsPerpendicular(vz, v),
                          "expected orthogonal lines in scope box geometry");

                        Debug.Assert(IsPerpendicular(vx, v),
                          "expected orthogonal lines in scope box geometry");

                        vy = v;

                        if (!IsRightHanded(vx, vy, vz))
                        {
                            XYZ tmp = vx;
                            vx = vy;
                            vy = tmp;
                        }
                        break;
                    }
                }
            }

        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            View3D view = doc.ActiveView as View3D;

            if (null == view)
            {
                message = "Command must be run in a 3D view.";
                return Result.Failed;
            }

            double offset = 0;
            using (FrmSectionBoxFit form = new FrmSectionBoxFit(doc))
            {
                form.ShowDialog();
                if (form.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                    return Result.Cancelled;
                offset = form.getOffset();
            }

            if (doc.DisplayUnitSystem == DisplayUnit.METRIC)
            {
                offset = offset / 304.8;
            }

            //   Element scopeBox = new FilteredElementCollector( doc, view.Id ).OfCategory( BuiltInCategory.OST_VolumeOfInterest ).WhereElementIsNotElementType().FirstElement();

            XYZ origin = null;
            XYZ vx = XYZ.Zero;
            XYZ vy = XYZ.Zero;
            XYZ vz = XYZ.Zero;

            // GetVectorsFromPickedEdges(doc, out origin, out vx, out vy, out vz);

            Element e = null;
            try
            {
                e = doc.GetElement(uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, new Utils.GenericSelectionFilter(BuiltInCategory.OST_Walls), "Select a straight wall."));
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }


            BoundingBoxXYZ bb = new BoundingBoxXYZ();
            bb.Min = XYZ.Zero;

            if (e is Wall)
            {
                bool result = GetVectorsFromElement((HostObject)e, out origin, out vx, out vy, out vz);

                if (!result)
                {
                    return Result.Cancelled;
                }

                bb.Max = new XYZ(vx.GetLength() + offset * 2, vy.GetLength() + offset * 2, vz.GetLength() + offset * 2);

                Transform t = Transform.Identity;
                t.Origin = origin.Subtract(vx.Normalize().Multiply(offset)).Subtract(vy.Normalize().Multiply(offset)).Subtract(vz.Normalize().Multiply(offset));
                t.BasisX = vx.Normalize();
                t.BasisY = vy.Normalize();
                t.BasisZ = vz.Normalize();

                Debug.Assert(t.IsConformal, "expected resulting transform to be conformal");

                bb.Transform = t;
            }
            else
            {
                //BoundingBoxXYZ elementBB = e.get_BoundingBox(doc.ActiveView);
                //bb.Max = elementBB.Max.Subtract(elementBB.Min);
            }

            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Move And Resize Section Box");

                view.SetSectionBox(bb);

                tx.Commit();
            }
            return Result.Succeeded;
        }
    }
}
