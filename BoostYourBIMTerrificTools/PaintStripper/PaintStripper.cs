using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#if !RELEASE2013

namespace BoostYourBIMTerrificTools.PaintStripper
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<Element> elements = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .ToList();

            List<ElementId> materialIds;

            using (FormPaintStripper form = new FormPaintStripper(doc))
            {
                if (form.ShowDialog() == DialogResult.Cancel)
                    return Result.Cancelled;

                materialIds = form.getIds();
            }

            using (Transaction t = new Transaction(doc, "Paint Stripper"))
            {
                t.Start();
                foreach (Element e in elements)
                {
                    List<Solid> solids = Utils.GetElementSolids(e.get_Geometry(new Options()));
                    foreach (Solid s in solids)
                    {
                        foreach (Face f in s.Faces)
                        {
                            RemovePaint(e, f, materialIds);
                            foreach (Face region in f.GetRegions())
                            {
                                RemovePaint(e, region, materialIds);
                            }
                        }
                    }
                }
                t.Commit();
            }
            return Result.Succeeded;

        }

        private void RemovePaint(Element e, Face f, List<ElementId> materialIds)
        {
            if (materialIds.Contains(e.Document.GetPaintedMaterial(e.Id, f)))
                e.Document.RemovePaint(e.Id, f);
        }


    }
}
#endif