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

namespace BoostYourBIMTerrificTools.FamilyRename
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            OpenFileDialog ofd = new OpenFileDialog
            {
                AutoUpgradeEnabled = false,
                Filter = "CSV File|*.csv",
                Title = "Select Mapping File",
                Multiselect = false
            };
            if (ofd.ShowDialog() != DialogResult.OK)
                return Result.Cancelled;

            string input = ofd.FileName;
            Dictionary<string, string> mapping = new Dictionary<string, string>();
            using (StreamReader sr = new StreamReader(input))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    string[] sp = line.Split(',');
                    if (sp.Count() < 2)
                        continue;
                    if (mapping.ContainsKey(sp[0]))
                        continue;
                    mapping.Add(sp[0], sp[1]);
                }
            }

            List<string> errors = new List<string>();
            using (Transaction t = new Transaction(doc, "Family Rename"))
            {
                t.Start();
                foreach (KeyValuePair<string, string> pair in mapping)
                {
                    foreach (Family f in new FilteredElementCollector(doc)
                        .OfClass(typeof(Family))
                        .Where(q => q.Name == pair.Key))
                    {
                        if (new FilteredElementCollector(doc).OfClass(typeof(Family)).Any(q => q.Name == pair.Value))
                        {
                            errors.Add("Cannot rename from '" + pair.Key + "' to '" + pair.Value + "' because a family with this name already exists");
                        }
                        else
                        {
                            f.Name = pair.Value;
                        }
                    }
                }
                t.Commit();
            }

            if (errors.Any())
            {
                TaskDialog td = new TaskDialog("Errors")
                {
                    MainContent = string.Join(Environment.NewLine, errors)
                };
                td.Show();
            }

            return Result.Succeeded;
        }
    }
}
