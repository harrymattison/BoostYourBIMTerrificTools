#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using BoostYourBIMTerrificTools;
using Application = Autodesk.Revit.ApplicationServices.Application;
#endregion

namespace StringSearch
{
    /// <summary>
    /// String search external command implementation 
    /// compatible with both Revit 2011 and 2012 API.
    /// </summary>
    [Regeneration(RegenerationOption.Manual)] // 2011
    [Transaction(TransactionMode.ReadOnly)]
    public class Command : IExternalCommand
    {
        /// <summary>
        /// Display an informational message.
        /// </summary>
        //static public void InfoMsg( string msg )
        //{
        //  TaskDialog.Show( 
        //    AboutBox.AssemblyProduct, 
        //    msg );
        //}

        #region Execute
        /// <summary>
        /// The Revit 2012 API provides the new method
        /// Autodesk.Revit.UI.Selection.GetElementIds
        /// returning an ICollection of ElementId. In 
        /// Revit 2011, we implement it ourselves for 
        /// backwards compatibility.
        /// </summary>
        ICollection<ElementId> GetSelectedElementIds(
          UIDocument uidoc)
        {
            return uidoc.Selection.GetElementIds();
        }

        /// <summary>
        /// External command mainline.
        /// Determine Revit application window handle.
        /// Prompt user for search string and options.
        /// Retrieve all requested elements and search their parameters.
        /// List the result in a log file and a data container.
        /// </summary>
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Application app = uiapp.Application;
            ICollection<ElementId> selids = GetSelectedElementIds(new UIDocument(Utils.doc));

            try
            {
                using (JtLogFile log = new JtLogFile("SearchString"))
                {
                    SortableBindingList<SearchHit> data = new SortableBindingList<SearchHit>();

                    while (null == data || 0 == data.Count)
                    {
                        // Display search form:

                        SearchForm form = new SearchForm(log.Path);

                        DialogResult r = form.ShowDialog();

                        if (DialogResult.Cancel == r)
                        {
                            message = string.Empty;
                            return Result.Cancelled;
                        }

                        if (form.CurrentSelection && 0 == selids.Count)
                        {
                            Autodesk.Revit.UI.TaskDialog.Show("Alert", "Sorry; you cannot search the current element selection, because it is empty.");
                            continue;
                        }

                        // Run filtered element collector:
                        List<Document> docs = new List<Document>();
                        if (form.RvtLinks)
                        {
                            docs.Add(Utils.doc);
#if !RELEASE2013
                            docs.AddRange(Utils.doc.Application.Documents.Cast<Document>().Where(q => q.IsLinked));
#endif
                        }
                        else
                        {
                            docs.Add(Utils.doc);
                        }


                        List<FilteredElementCollector> collectors = new List<FilteredElementCollector>();
                        UIDocument uidoc = new UIDocument(Utils.doc);
                        foreach (Document d in docs)
                        {
#region Set up filtered element collector
                            FilteredElementCollector a
                              = form.CurrentView
                                ? new FilteredElementCollector(
                                  d, d.ActiveView.Id)
                              : form.CurrentSelection
                                ? new FilteredElementCollector(
                                    d, GetSelectedElementIds(uidoc))
                              : new FilteredElementCollector(d);

                            if (form.ElementType && form.NonElementType)
                            {
                                a.WhereElementIsElementType();

                                FilteredElementCollector b = form.CurrentView
                                  ? new FilteredElementCollector(d, d.ActiveView.Id)
                                  : new FilteredElementCollector(d);

                                b.WhereElementIsNotElementType();

                                a.UnionWith(b);
                            }
                            else if (form.ElementType)
                            {
                                a.WhereElementIsElementType();
                            }
                            else if (form.NonElementType)
                            {
                                a.WhereElementIsNotElementType();
                            }
                            else
                            {
                                message = "Please select at least one or both of Element type and non-Element type.";
                                return Result.Failed;
                            }

                            if (!form.AllCategories)
                            {
                                BuiltInCategory bic
                                  = (BuiltInCategory)Enum.Parse(
                                    typeof(BuiltInCategory),
                                    form.CategoryName);

                                a.OfCategory(bic);
                            }
#endregion // Set up filtered element collector
                            collectors.Add(a);
                        }
                        // Search element parameter data:


                        foreach (FilteredElementCollector collector in collectors)
                        {
                            StringSearcher ss = new StringSearcher(
                              collector, form.SearchOptions);

                            try
                            {
                                foreach (SearchHit sh in ss.Run(log, out message))
                                {
                                    data.Add(sh);
                                }
                            }
                            catch (ArgumentException ex)
                            {
                                if (ex.StackTrace.Contains(
                                  "RegularExpressions.RegexParser.ScanRegex"))
                                {
                                    Autodesk.Revit.UI.TaskDialog.Show("Alert", "Invalid regular expression. Error message:\r\n"
                      + ex.Message
                      + "\r\n What's a regular expression? See: http://regexlib.com/cheatsheet.aspx).");
                                }
                                else
                                {
                                    throw ex;
                                }
                            }
                        }

                        if (message.Length > 0)
                            Autodesk.Revit.UI.TaskDialog.Show("Alert", message);
                        else if (data.Count == 0)
                            Autodesk.Revit.UI.TaskDialog.Show("Alert", "No matches found");
                    }

                    if (data.Count > 0)
                    {
                        Ribbon.ShowForm(data);
                    }

                    return Result.Succeeded;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message + Environment.NewLine + ex.StackTrace;
                return Result.Failed;
            }
        }
#endregion // Execute
    }
}
