using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Windows.Media.Imaging;

// catch error after parameter.Set & hit cancel

namespace ImageOMatic
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class ImageOMatic : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = new UIDocument(doc);

            Element familyInstance = null;
            Autodesk.Revit.DB.View view = doc.ActiveView;

            if (uidoc.Selection.GetElementIds().Count == 1)
			{
				familyInstance = doc.GetElement(uidoc.Selection.GetElementIds().FirstOrDefault());
			}

            if (familyInstance == null)
            {
                try
                {
                    familyInstance = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element, new Utils.FamilyInstanceOrDisplacementSelectionFilter(), "Select a family instance / displacement set for parameters or ESC to use Phases only.").ElementId);
                }
                catch (Exception) // nothing selected - show list of phases
                {  }
            }
            
            uidoc.Selection.SetElementIds(new List<ElementId> { }); // clear the selection

            string errors = "";

            using (BoostYourBIM.formParameterAnimator form = new BoostYourBIM.formParameterAnimator(doc.Phases, familyInstance, doc.ActiveView.DisplayStyle))
            {
                form.ShowDialog();

                if (form.DialogResult == DialogResult.Cancel)
                    return Result.Cancelled;

                ImageExportOptions options = new ImageExportOptions();
                options.HLRandWFViewsFileType = form.getImageFileType();
                options.ShadowViewsFileType = form.getImageFileType();
                options.ZoomType = ZoomFitType.FitToPage;
                options.PixelSize = form.getPixels();
                options.FitDirection = form.getFitDir();
                options.ExportRange = form.getExportRange();

                DirectoryInfo dirInfo = Directory.CreateDirectory(form.getFolder() + "\\Image-O-Matic-"
                    + DateTime.Now.Hour.ToString("D2") + "-"
                    + DateTime.Now.Minute.ToString("D2") + "-"
                    + DateTime.Now.Second.ToString("D2"));
                string path = Path.Combine(form.getFolder(), dirInfo.ToString());
                
                using (TransactionGroup tg = new TransactionGroup(doc, "group"))
                {
                    tg.Start();
                    using (Transaction t = new Transaction(doc, "Display Style"))
                    {
                        t.Start();
                        try
                        {
                            view.DisplayStyle = form.getDisplayStyle();
                        }
                        catch (Autodesk.Revit.Exceptions.InvalidOperationException)
                        {
                            TaskDialog.Show("Error", "Cannot set view display style. If display style is controlled by a View Template, those settings will be used.");
                        }
                        t.Commit();
                    }
                    if (form.getSelectedTab() == "tabPhases")
                    {
                        IList<string> phaseList = form.getSelectedPhaseNames();
                        int n = phaseList.Count;
                        string s = "{0} of " + n.ToString() + " images processed...";
                        string caption = "Export Phase Images";
                        using (BoostYourBIMUtils.formProgress progressForm = new BoostYourBIMUtils.formProgress(caption, s, n))
                        {
                            int ctr = 0;
                            foreach (string phaseName in phaseList)
                            {
                                if (progressForm.getAbortFlag())
                                    break;

                                Parameter parameter = Utils.getParam(view, "Phase");
                                Element phase = (from pp in new FilteredElementCollector(doc).OfClass(typeof(Phase)).Cast<Phase>() where pp.Name == phaseName select pp).First();
                                using (Transaction t = new Transaction(doc, "Modify phase"))
                                {
                                    if (parameter != null)
                                    {
                                        t.Start();
                                        parameter.Set(phase.Id);
                                        t.Commit();
                                        uidoc.RefreshActiveView();
                                    }
                                    options.FilePath = path + "\\" + ctr.ToString("D5") + " " + phase.Name.Replace(".","_");
                                    try
                                    {
                                        doc.ExportImage(options);
                                    }
                                    catch (Exception)
                                    {
                                        errors += ctr + " ";
                                    }
                                    ctr++;
                                    progressForm.Increment();
                                }
                            }
                        }
                    }
                    else if (form.getSelectedTab() == "tabParameter")
                    {
                        int n = (int)((form.getEnd() - form.getStart()) / form.getInc()) + 1;
                        string s = "{0} of " + n.ToString() + " images processed...";
                        string caption = "Export Parameter Images";

                        double thisValue = form.getStart();

                        Parameter parameter = null;

                        using (BoostYourBIMUtils.formProgress progressForm = new BoostYourBIMUtils.formProgress(caption, s, n))
                        {
                            using (Transaction t = new Transaction(doc, "Modify parameter"))
                            {
                                for (int ctr = 1; ctr <= n ; ctr++)
                                {
                                    if (progressForm.getAbortFlag())
                                        break;

                                    t.Start();
                                    FailureHandlingOptions failOpt = t.GetFailureHandlingOptions();
                                    failOpt.SetFailuresPreprocessor(new Utils.RollbackFailures());
                                    t.SetFailureHandlingOptions(failOpt);

                                    string paramName = "";

                                    if (familyInstance is FamilyInstance)
                                    {
                                        parameter = Utils.getParam(familyInstance, form.getParameterName());

                                        paramName = parameter.Definition.Name;

                                        double convertedValue = thisValue;
                                        if (parameter.StorageType == StorageType.Double &&
#if PREFORGETYPEID
                                            parameter.DisplayUnitType == DisplayUnitType.DUT_DECIMAL_DEGREES)
#else
                                            parameter.GetUnitTypeId() == UnitTypeId.Degrees)
#endif
                                            convertedValue = BoostYourBIMTerrificTools.Utils.DegreesToRadians(thisValue);
                                        try
                                        {
                                            if (parameter.StorageType == StorageType.Integer && !Utils.isInt(convertedValue.ToString()))
                                            {
                                                errors += ctr + " ";
                                                t.RollBack();
                                                thisValue += form.getInc();
                                                progressForm.Increment();
                                                continue;
                                            }
                                            if (!parameter.Set(convertedValue)) // give error if set failed
                                            {
                                                errors += ctr + " ";
                                                t.RollBack();
                                                thisValue += form.getInc();
                                                progressForm.Increment();
                                                continue;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            errors += ctr + " ";
                                            t.RollBack();
                                            thisValue += form.getInc();
                                            progressForm.Increment();
                                            continue;
                                        }

                                        t.Commit();

                                        if (t.GetStatus() == TransactionStatus.RolledBack)
                                        {
                                            errors += ctr + " ";
                                            thisValue += form.getInc();
                                            progressForm.Increment();
                                            continue;
                                        }
                                    }
#if !R2013
                                    else if (familyInstance is DisplacementElement)
                                    {
                                        parameter = Utils.getParam(familyInstance, form.getParameterName());
                                        paramName = form.getParameterName();
                                        parameter.Set(thisValue);
                                        thisValue += form.getInc();
                                        t.Commit();
                                    }
#endif
                                    uidoc.RefreshActiveView();
                                    options.FilePath = path + "\\" + ctr.ToString("D5") + " " + paramName + "=" + thisValue.ToString().Replace(".", "_");
                                    try
                                    {
                                        doc.ExportImage(options);
                                    }
                                    catch (Exception)
                                    {
                                        errors += ctr + " ";
                                    }
                                    thisValue += form.getInc();
                                    progressForm.Increment();
                                }
                            }
                        }
                    }
                    else
                    {
                        TaskDialog.Show("Error", "Bad selected tab");
                        return Result.Cancelled;
                    }
                    tg.RollBack();
                }
                if (form.getOpenFolder())
                    Process.Start(path);

                form.Close();
            }

            if (errors.Length > 0)
            {
                errors = "The following images could not be exported:\n" + errors;
                TaskDialog.Show("Some Images Failed", errors);
            }

            return Result.Succeeded;
        }
    }
}
