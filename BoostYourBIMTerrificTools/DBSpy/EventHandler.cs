using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BoostYourBIMTerrificTools.DBSpy
{

    public class EventHandlerSolid : RevitEventWrapper<Solid>
    {
        public override void Execute(UIApplication uiApp, Solid solid)
        {
#if !RELEASE2015 && !RELEASE2016
            try
            {
                Document doc = uiApp.ActiveUIDocument.Document;
                using (Transaction t = new Transaction(doc, "Revit Lookup"))
                {
                    t.Start();
                    DirectShape ds = DirectShape.CreateElement(doc, new ElementId((int)BuiltInCategory.OST_GenericModel));
                    ds.SetShape(new List<GeometryObject> { solid });
                    Utils.transientElements.Add(ds.Id);
                    uiApp.ActiveUIDocument.Selection.SetElementIds(new List<ElementId> { ds.Id });
                    t.Commit();
                }
            }
            catch (Exception ex)
            {

            }
#endif
        }
    }

        public class EventHandlerLine : RevitEventWrapper<Line>
    {
        public override void Execute(UIApplication uiApp, Line line)
        {
            try
            {
                Document doc = uiApp.ActiveUIDocument.Document;
                using (Transaction t = new Transaction(doc, "Revit Lookup"))
                {
                    t.Start();

                    ModelLine ml = BoostYourBIMTerrificTools.Utils.makeLine(doc, line.GetEndPoint(0), line.GetEndPoint(1));

                    Utils.transientElements.Add(ml.Id);
                    uiApp.ActiveUIDocument.Selection.SetElementIds(new List<ElementId> { ml.Id });
                    t.Commit();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }


    public class EventHandlerXYZ : RevitEventWrapper<XYZ>
    {
        public override void Execute(UIApplication uiApp, XYZ xyz)
        {
            Document doc = uiApp.ActiveUIDocument.Document;
            using (Transaction t = new Transaction(doc, "Revit Lookup"))
            {
                t.Start();
                double radius = 0.2;
                Arc arc = Arc.Create(
                  xyz - radius * XYZ.BasisZ,
                  xyz + radius * XYZ.BasisZ,
                  xyz + radius * XYZ.BasisX);

                Line line = Line.CreateBound(
                  arc.GetEndPoint(1),
                  arc.GetEndPoint(0));

                CurveLoop halfCircle = new CurveLoop();
                halfCircle.Append(arc);
                halfCircle.Append(line);

                List<CurveLoop> loops = new List<CurveLoop>
                {
                    halfCircle
                };

                Frame frame = new Frame(xyz, XYZ.BasisX, XYZ.BasisY, XYZ.BasisZ);
                Solid sphere = GeometryCreationUtilities.CreateRevolvedGeometry(frame, loops, 0, 2 * Math.PI);
#if RELEASE2015 || RELEASE2016
                DirectShape ds = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel), "d", "d");
#else
                DirectShape ds = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));
#endif
                ds.SetShape(new List<GeometryObject> { sphere });
                ds.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set(
                    "X: " + xyz.X +
                    ", Y: " + xyz.Y +
                    ", Z: " + xyz.Z 
                    );
                Utils.transientElements.Add(ds.Id);
                uiApp.ActiveUIDocument.Selection.SetElementIds(new List<ElementId> { ds.Id });
                t.Commit();
            }
        }
    }


    public class TransientElementEventHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            try
            {
                Document doc = app.ActiveUIDocument.Document;
                using (Transaction t = new Transaction(doc, "Revit Lookup"))
                {
                    t.Start();
                    if (Utils.transientElements.Any())
                    {
                        doc.Delete(Utils.transientElements);
                    }
                    t.Commit();
                }
            }
            catch
            {
            }
        }

        public string GetName()
        {
            return "Revit Lookup";
        }
    }
}