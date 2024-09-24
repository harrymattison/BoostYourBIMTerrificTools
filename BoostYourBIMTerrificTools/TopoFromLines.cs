using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoostYourBIMTerrificTools
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class topoFromLines : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
            int.TryParse(app.VersionNumber, out int versionInt);

            Document doc = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = new UIDocument(doc);

            IList<XYZ> points = new List<XYZ>();

            IList<Tuple<int, double, double, double>> curvePoints = new List<Tuple<int, double, double, double>>();

            IList<Reference> refList = null;

            try
            {
                refList = uidoc.Selection.PickObjects(ObjectType.Element, new LineImportSelectionFilter(), "Select lines or import instances to define the toposurface.");
            }
            catch
            {
                return Result.Cancelled;
            }

            IList<string> errors = new List<string>();

            if (refList.Count == 0)
            {
                TaskDialog.Show("Error", "No lines were selected.");
                return Result.Cancelled;
            }


            foreach (Reference r in refList)
            {
                Element e = doc.GetElement(r);

                if (e is CurveElement)
                {
                    CurveElement mc = e as CurveElement;
                    for (double i = 0.01; i < 1; i = i + 0.02)
                    {
                        XYZ p = null;
                        try
                        {
                            p = mc.GeometryCurve.Evaluate(i, true);
                        }
                        catch
                        {
                            if (!errors.Contains(mc.Id.IntegerValue.ToString()))
                                errors.Add(mc.Id.IntegerValue.ToString());
                        }
                        if (p != null)
                        {
                            curvePoints.Add(new Tuple<int, double, double, double>(mc.Id.IntegerValue, p.X, p.Y, p.Z));
                            points.Add(p);
                        }
                    }
                }

                if (e is ImportInstance)
                {
                    ImportInstance ii = e as ImportInstance;
                    Transform transform = ii.GetTransform();
                    GeometryElement ge = e.get_Geometry(new Options());
                    foreach (GeometryObject go in ge)
                    {
                        GeometryInstance gi = go as GeometryInstance;
                        if (gi == null)
                            continue;

                        foreach (GeometryObject instgo in gi.SymbolGeometry)
                        {
                            if (instgo is Curve)
                            {
                                Curve c = instgo as Curve;
                                c = c.CreateTransformed(transform);
                                for (double i = 0.01; i < 1; i = i + 0.02)
                                {
                                    XYZ p = null;
                                    try
                                    {
                                        p = c.Evaluate(i, true);
                                    }
                                    catch
                                    {
                                        if (!errors.Contains(e.Id.IntegerValue.ToString()))
                                            errors.Add(e.Id.IntegerValue.ToString());
                                    }
                                    if (p != null)
                                    {
                                        curvePoints.Add(new Tuple<int, double, double, double>(e.Id.IntegerValue, p.X, p.Y, p.Z));
                                        points.Add(p);
                                    }
                                }
                            }
                            if (instgo is PolyLine)
                            {
                                PolyLine pl = instgo as PolyLine;
                                pl = pl.GetTransformed(transform);
                                for (double i = 0.01; i < 1; i = i + 0.02)
                                {
                                    XYZ p = null;
                                    try
                                    {
                                        p = pl.Evaluate(i);
                                    }
                                    catch
                                    {
                                        if (!errors.Contains(e.Id.IntegerValue.ToString()))
                                            errors.Add(e.Id.IntegerValue.ToString());
                                    }
                                    if (p != null)
                                    {
                                        curvePoints.Add(new Tuple<int, double, double, double>(e.Id.IntegerValue, p.X, p.Y, p.Z));
                                        points.Add(p);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (curvePoints.Count == 0)
            {
                TaskDialog.Show("Error", "No points could be generated on the selected lines. Manually split circles in two locations before running this command.");
                return Result.Cancelled;
            }

            using (Transaction t = new Transaction(doc, "Create Topo From Lines"))
            {
                t.Start();

                TopographySurface surface = null;
                try
                {
                    surface = TopographySurface.Create(doc, points);
                }
                catch (Autodesk.Revit.Exceptions.ArgumentException ex)
                {
                    TaskDialog.Show("Error", ex.Message);
                    t.RollBack();
                    return Result.Cancelled;
                }

                using (StreamWriter sw = new StreamWriter(Path.Combine(Path.GetTempPath(), "Topo" + surface.Id.IntegerValue + ".txt"), false))
                {
                    foreach (Tuple<int, double, double, double> tup in curvePoints)
                    {
                        sw.WriteLine(Math.Round((double)tup.Item1, 5) + "~" + Math.Round((double)tup.Item2, 5) + "~" + Math.Round((double)tup.Item3, 5) + "~" + Math.Round((double)tup.Item4, 5));
                    }
                }
                t.Commit();
            }

            if (errors.Count > 0)
            {
                string e = "";
                foreach (string s in errors)
                {
                    e += s + ",";
                }
                e = e.Remove(e.Length - 1);
                TaskDialog.Show("Error", "Could not evaluate some points on curves with the following element ids.\nIf curves are circles or ellipses, try splitting them into two lines.\n" + e);
            }

            return Result.Succeeded;
        }


        public class LineImportSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element e)
            {
                if (e.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Lines)
                    return true;

                if (e is ImportInstance)
                    return true;

                return false;
            }
            public bool AllowReference(Reference r, XYZ point)
            {
                return true;
            }
        }

    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class topoFromLinesUpdate : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
#if R2013
            TaskDialog.Show("Error","This command does not run on Revit 2013");
#else
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;


            Document doc = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = new UIDocument(doc);
            CurveElement mc = null;
            try
            {
                mc = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element, new Utils.GenericSelectionFilter(BuiltInCategory.OST_Lines), "Select line.")) as CurveElement;
            }
            catch
            {
                return Result.Cancelled;
            }

            TopographySurface topo = null;
            try
            {
                topo = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element, new Utils.GenericSelectionFilter(BuiltInCategory.OST_Topography), "Select toposurface.")) as TopographySurface;
            }
            catch
            {
                return Result.Cancelled;
            }

            IList<Element> topos = new FilteredElementCollector(doc).OfClass(typeof(TopographySurface)).ToList();

            IList<XYZ> delete = new List<XYZ>();
            IList<Tuple<int, double, double, double>> curvePoints = new List<Tuple<int, double, double, double>>();

            string file = Path.Combine(Path.GetTempPath(), "Topo" + topo.Id.IntegerValue + ".txt");
            if (File.Exists(file))
            {

                using (StreamReader sr = new StreamReader(file))
                {
                    string thisLine = "";
                    while (thisLine != null)
                    {
                        thisLine = sr.ReadLine();
                        if (thisLine == null)
                            break;

                        string[] words = thisLine.Split('~');

                        if (words.Length != 4)
                            continue;

                        if (mc.Id.IntegerValue.ToString() == words[0])
                        {
                            XYZ ptFromFile = new XYZ(Convert.ToDouble(words[1]), Convert.ToDouble(words[2]), Convert.ToDouble(words[3]));
                            foreach (XYZ ptFromTopo in topo.GetPoints())
                            {
                                if (ptFromFile.DistanceTo(ptFromTopo) < 0.01)
                                {
                                    delete.Add(ptFromTopo);
                                }
                            }
                        }
                    }
                }
            }

            IList<XYZ> points = new List<XYZ>();
            for (double i = 0.01; i < 1; i = i + 0.02)
            {
                XYZ p = null;
                try
                {
                    p = mc.GeometryCurve.Evaluate(i, true);
                }
                catch
                {
                }
                if (p != null)
                {
                    curvePoints.Add(new Tuple<int, double, double, double>(mc.Id.IntegerValue, p.X, p.Y, p.Z));
                    points.Add(p);
                }
            }

            using (TopographyEditScope tes = new TopographyEditScope(doc, "Topo Edit Scope"))
            {
                tes.Start(topo.Id);
                if (delete.Count > 0)
                {
                    using (Transaction t = new Transaction(doc, "Update Topo"))
                    {
                        t.Start();
                        topo.DeletePoints(delete);
                        t.Commit();
                    }
                }
                using (Transaction t = new Transaction(doc, "Update Topo"))
                {
                    t.Start();
                    topo.AddPoints(points);
                    t.Commit();
                }
                tes.Commit(new iFailuresPreprocessor());
            }

            using (StreamWriter sw = new StreamWriter(Path.Combine(Path.GetTempPath(), "Topo" + topo.Id.IntegerValue + ".txt"), true))
            {
                foreach (Tuple<int, double, double, double> tup in curvePoints)
                {
                    sw.WriteLine(Math.Round((double)tup.Item1, 5) + "~" + Math.Round((double)tup.Item2, 5) + "~" + Math.Round((double)tup.Item3, 5) + "~" + Math.Round((double)tup.Item4, 5));
                }
            }
#endif
            return Result.Succeeded;
        }
    }

    public class iFailuresPreprocessor : IFailuresPreprocessor
    {
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            return FailureProcessingResult.Continue;
        }
    }
}
