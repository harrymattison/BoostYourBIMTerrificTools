using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BoostYourBIMTerrificTools.SelectByType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI.Selection;
using System.Windows.Media.Animation;
using System.Windows.Forms;

namespace BoostYourBIMTerrificTools.SketchConvert
{
    // https://forums.autodesk.com/t5/revit-ideas/easily-convert-floors-to-ceilings-and-roofs-and-vice-versa/idi-p/12362679
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class SketchConvert : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;
            var selectedElement = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element, 
                new Utils.MultiCatSelectionFilter(new List<ElementId> {
                    new ElementId(BuiltInCategory.OST_Floors), 
                    new ElementId(BuiltInCategory.OST_Ceilings),
                    new ElementId(BuiltInCategory.OST_Roofs)
                }), 
                "Select floor, roof, or ceiling"));
            ElementId sketchId;
            ElementId levelId;
            if (selectedElement is Floor f)
            {
                sketchId = f.SketchId;
                levelId = f.LevelId;
            }
            else if (selectedElement is Ceiling c)
            {
                sketchId = c.SketchId;
                levelId = c.LevelId;
            }
            else if (selectedElement is FootPrintRoof roof)
            {
                // SketchId property has not yet been implemented for roofs
                // vote for it at 
                // https://forums.autodesk.com/t5/revit-ideas/api-add-sketchid-for-all-sketched-elements-roof-beam-system-etc/idi-p/10297939
                // in the meantime, do the old 'delete in a temporary transaction' trick
                List<ElementId> ids;
                using (var t = new Transaction(doc, "temp"))
                {
                    t.Start();
                    ids = doc.Delete(roof.Id).ToList();
                    t.RollBack();
                }
                sketchId = ids.First(q => doc.GetElement(q) is Sketch);
                levelId = roof.LevelId;
            }
            else
            {
                throw new NotImplementedException();
            }

            var sketch = doc.GetElement(sketchId) as Sketch;
            var profile = sketch.Profile;

            Target target;
            using (FormSketchConvert form = new FormSketchConvert(selectedElement))
            {
                if (form.ShowDialog() == DialogResult.Cancel)
                    return Result.Cancelled;
                target = form.getTarget();
            }

            using (var t = new Transaction(doc, "Sketch Convert"))
            {
                Element newElement = null;
                t.Start();
                if (target == Target.Floor)
                {
                    var floorType = new FilteredElementCollector(doc).OfClass(typeof(FloorType)).Cast<FloorType>().First();
                    newElement = Floor.Create(doc, CurveArrArrToCurveLoopList(profile), floorType.Id, levelId);
                }
                else if (target == Target.Ceiling)
                {
                    var ceilingType = new FilteredElementCollector(doc).OfClass(typeof(CeilingType)).Cast<CeilingType>().First();
                    newElement = Ceiling.Create(doc, CurveArrArrToCurveLoopList(profile), ceilingType.Id, levelId);
                }
                else if (target == Target.Roof)
                {
                    var modelCurveArray = new ModelCurveArray();
                    newElement = doc.Create.NewFootPrintRoof(
                        CurveArrArrToCurveArray(profile),
                        doc.GetElement(levelId) as Level,
                        new FilteredElementCollector(doc).OfClass(typeof(RoofType)).Cast<RoofType>().First(),
                        out modelCurveArray);
                }
                else if (target == Target.ModelLines)
                {
                    foreach (CurveArray ca in profile)
                    {
                        foreach (Curve c in ca)
                        {
                            Utils.makeCurve(doc, c);
                        }
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }

                if (newElement != null)
                    GetOffsetParameter(newElement).Set(GetOffsetParameter(selectedElement).AsDouble());

                t.Commit();
            }

            return Result.Succeeded;
        }

        private Parameter GetOffsetParameter(Element e)
        {
            if (e is RoofBase)
            {
                return e.get_Parameter(BuiltInParameter.ROOF_LEVEL_OFFSET_PARAM);
            }
            if (e is Ceiling)
            {
                return e.get_Parameter(BuiltInParameter.CEILING_HEIGHTABOVELEVEL_PARAM);
            }
            if (e is Floor)
            {
                return e.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM);
            }
            return null;
        }

        private List<CurveLoop> CurveArrArrToCurveLoopList(CurveArrArray caa)
        {
            var ret = new List<CurveLoop>();
            foreach (CurveArray ca in caa)
            {
                CurveLoop cl = new CurveLoop();
                foreach (Curve c in ca)
                {
                    cl.Append(c);
                }
                ret.Add(cl);
            }
            return ret;
        }

        private CurveArray CurveArrArrToCurveArray(CurveArrArray caa)
        {
            var ret = new CurveArray();
            foreach (CurveArray ca in caa)
            {
                CurveLoop cl = new CurveLoop();
                foreach (Curve c in ca)
                {
                    ret.Append(c);
                }
            }
            return ret;
        }

    }

    public enum Target
    {
        UNDEFINED,
        Roof,
        Floor,
        Ceiling,
        ModelLines
    }

}
