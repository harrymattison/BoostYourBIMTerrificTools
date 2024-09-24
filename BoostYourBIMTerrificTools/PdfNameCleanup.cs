using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoostYourBIMTerrificTools
{
    [Transaction(TransactionMode.ReadOnly)]
    public class PdfNameCleanup : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            if (Utils.PrintedSheetNumbers == null || !Utils.PrintedSheetNumbers.Any())
            {
                TaskDialog.Show("Error", "No views have been printed");
                return Result.Cancelled;
            }

            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            PrintManager pm = doc.PrintManager;
            string filename = pm.PrintToFileName;
            string folder = Path.GetDirectoryName(filename);

            RegistryKey myKey = Registry.CurrentUser.OpenSubKey("Software\\Bluebeam Software\\2019\\Brewery\\V45\\Printer Driver", true);
            if (myKey != null)
            {
                string projectsFolder = myKey.GetValue("ProjectsFolder").ToString();
                if (projectsFolder != "" && Directory.Exists(projectsFolder))
                    folder = projectsFolder;
            }

            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Select PDF Folder",
                ShowNewFolderButton = false,
                SelectedPath = folder
            };
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                folder = folderBrowserDialog1.SelectedPath;
            }

            List<string> files = Directory.GetFiles(folder, "*.pdf").ToList();
            if (!files.Any())
            {
                TaskDialog td = new TaskDialog("Error")
                {
                    MainInstruction = "No PDFs found",
                    MainContent = folder
                };
                td.Show();
                return Result.Cancelled;
            }

            doc.Application.WriteJournalComment("PdfNameCleanup: PrintedSheetNumbers: " + string.Join(",", Utils.PrintedSheetNumbers), true);
            doc.Application.WriteJournalComment("PdfNameCleanup: Pdf Files: " + string.Join(",", files), true);

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                TimeSpan timeSpan = DateTime.Now - fi.LastWriteTime;
                if (timeSpan.TotalMinutes > 30)
                    continue;
                foreach (string sheetNumber in Utils.PrintedSheetNumbers)
                {
                    string newFileName = Path.GetFileNameWithoutExtension(file);
                    string sheetnum = sheetNumber.Replace(".", "-");
                    if (newFileName.Contains(" - " + sheetnum))
                    {                        
                        string newFolder = Path.GetDirectoryName(file);
                        int pos = newFileName.IndexOf(sheetnum);
                        
                        string newName = Path.Combine(newFolder, newFileName.Substring(pos - 1, newFileName.Length - pos + 1) + ".pdf");
                        doc.Application.WriteJournalComment("PdfNameCleanup: Rename from " + file, true);
                        doc.Application.WriteJournalComment("PdfNameCleanup: Rename to " + newName, true);
                        File.Move(file, newName);
                        break;
                    }
                }
            }
            Utils.PrintedSheetNumbers.Clear();
            return Result.Succeeded;
        }
    }
}