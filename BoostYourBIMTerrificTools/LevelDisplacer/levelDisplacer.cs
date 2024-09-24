using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using System.Reflection;

namespace LevelDisplacer
{
    [Transaction(TransactionMode.Manual)]
    [Journaling(JournalingMode.UsingCommandData)]
    public class levelDisplacer : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;

            Document doc = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = new UIDocument(doc);

            if (doc.ActiveView.ViewType != ViewType.ThreeD)
            {
                TaskDialog.Show("Error", "Active view must be a 3D view");
                return Result.Cancelled;
            }

#if R2013
            TaskDialog.Show("Error", "This command does not run on Revit 2013.");
#else
            double incX = 0;
            double incY = 0;
            double incZ = 0;
            bool hideNoDisplace = false;

            if (commandData.JournalData.Count > 0)
            {
                IDictionary<string, string> dataMap = commandData.JournalData;
                incX = Convert.ToDouble(Utils.GetDictString(dataMap, "X"));
                incY = Convert.ToDouble(Utils.GetDictString(dataMap, "Y"));
                incZ = Convert.ToDouble(Utils.GetDictString(dataMap, "Z"));
                hideNoDisplace = Convert.ToBoolean(Utils.GetDictString(dataMap, "hide"));
            }
            else
            {
                using (FrmLevelDisplacer form = new FrmLevelDisplacer(doc))
                {
                    form.ShowDialog();
                    if (form.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                        return Result.Cancelled;

                    incX = form.getX();
                    incY = form.getY();
                    incZ = form.getZ();
                    hideNoDisplace = form.getHide();
                }

                // Write Journal Data
                IDictionary<string, string> writeMap = commandData.JournalData;
                writeMap.Clear();
                writeMap.Add("X", incX.ToString());
                writeMap.Add("Y", incY.ToString());
                writeMap.Add("Z", incZ.ToString());
                writeMap.Add("hide", hideNoDisplace.ToString()); 
            }

            if (doc.DisplayUnitSystem == DisplayUnit.METRIC)
            {
                incX = incX / 304.8;
                incY = incY / 304.8;
                incZ = incZ / 304.8;
            }

            string error = "";
            using (Transaction t = new Transaction(doc, "Level Displacer"))
            {
                t.Start();
                XYZ dir = XYZ.Zero;

                if (hideNoDisplace)
                {
                    List<ElementId> elementsToHide = new FilteredElementCollector(doc, doc.ActiveView.Id)
                        .Where(e => !DisplacementElement.IsAllowedAsDisplacedElement(e) && e.CanBeHidden(doc.ActiveView)).Select(q => q.Id).ToList();

                    if (elementsToHide.Count > 0)
                        doc.ActiveView.HideElements(elementsToHide);
                }

                foreach (Level l in new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().OrderBy(q => q.Elevation))
                {
                    IList<ElementId> elementsToDisplace = new FilteredElementCollector(doc, doc.ActiveView.Id)
                        .Where(e => 
                            (e.LevelId == l.Id || (e.LookupParameter("Schedule Level") != null && e.LookupParameter("Schedule Level").AsElementId() == l.Id))
                            && DisplacementElement.IsAllowedAsDisplacedElement(e))
                            .Select(q => q.Id).ToList();

                    if (!DisplacementElement.CanElementsBeDisplaced(doc.ActiveView, elementsToDisplace))
                    {
                        error += l.Name + Environment.NewLine;
                    }
                    else if (elementsToDisplace.Count > 0)
                        DisplacementElement.Create(doc, elementsToDisplace, dir, doc.ActiveView, null);

                    dir = new XYZ(dir.X + incX, dir.Y + incY, dir.Z + incZ);
                }
                t.Commit();
            }

            if (error.Length > 0)
            {
                TaskDialog td = new TaskDialog("Error");
                td.MainInstruction = "Unable to displace elements on the following levels. Possible cause is that the elements are already displaced in this view.";
                td.MainContent = error;
                td.Show();
            }
#endif
            return Result.Succeeded;
        }
    }

    public static class Utils
    {
        public static string GetDictString(IDictionary<string, string> dataMap, String key)
        {
            string dataValue = dataMap[key];

            if (String.IsNullOrEmpty(dataValue))
            {
                throw new Exception(key + "information does not exist in journal.");
            }
            return dataValue;
        }
    }
}
