using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoostYourBIMTerrificTools.PrintSuppression
{
    [Transaction(TransactionMode.ReadOnly)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            using (FormPrintSuppression form = new FormPrintSuppression(doc))
            {
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                    return Result.Cancelled;

                Utils.catIds = form.GetCategories();
            }
          
            StringCollection stringCollection = new StringCollection();
            stringCollection.AddRange(Utils.catIds.Select(q => q.IntegerValue.ToString()).ToArray());
            Properties.Settings.Default.PrintSuppression = stringCollection;
            Properties.Settings.Default.Save();

            doc.Application.DocumentPrinting -= Utils.Application_DocumentPrinting;
            doc.Application.DocumentPrinted -= Utils.Application_DocumentPrinted;

            if (Utils.catIds.Any())
            {
                doc.Application.DocumentPrinting += Utils.Application_DocumentPrinting;
                doc.Application.DocumentPrinted += Utils.Application_DocumentPrinted;
            }

            return Result.Succeeded;
        }
    }

    public static class Utils
    {
        public static List<ElementId> catIds;
        public static List<Category> GetCatsAndSubCats(Document doc)
        {
            List<Category> catsAndSubcats = new List<Category>();
            List<Category> categories = doc.Settings.Categories.Cast<Category>().ToList();
            foreach (Category cat in categories)
            {
                List<Category> subcats = cat.SubCategories.Cast<Category>().ToList();
                catsAndSubcats.Add(cat);
                catsAndSubcats.AddRange(subcats);
            }
            return catsAndSubcats;
        }

        public static void Application_DocumentPrinted(object sender, DocumentPrintedEventArgs e)
        {
            List<ElementId> viewIds = e.GetPrintedViewElementIds().ToList();
            viewIds.AddRange(e.GetFailedViewElementIds());
            DoHiding(e.Document, viewIds, false);
        }

        private static void DoHiding(Document doc, List<ElementId> viewIds, bool hide)
        {
            try
            {
                List<View> viewsToProcess = new List<View>();
                foreach (View view in viewIds.Select(q => doc.GetElement(q) as View))
                {
                    viewsToProcess.Add(getViewOrTemplate(view));
                    if (view is ViewSheet viewSheet)
                    {
                        foreach (View viewOnSheet in viewSheet
                            .GetAllViewports()
                            .Select(q => doc.GetElement(q) as Viewport)
                            .Select(w => doc.GetElement(w.ViewId) as View))
                        {
                            viewsToProcess.Add(getViewOrTemplate(viewOnSheet));
                        }
                    }
                }

                if (hide)
                {
                    ShouldUnhide = new Dictionary<ElementId, List<ElementId>>();
                }

                using (Transaction t = new Transaction(doc, "Print Suppression " + hide.ToString()))
                {
                    t.Start();
                    foreach (View view in viewsToProcess)
                    {
#if !RELEASE2013 && !RELEASE2014 && !RELEASE2015 && !RELEASE2016
                        foreach (ElementId catId in catIds.Where(q => view.CanCategoryBeHidden(q)))
                        {
                            Category cat = GetCatsAndSubCats(doc).FirstOrDefault(q => q.Id == catId);

                            if (hide && !view.GetCategoryHidden(catId))
                            {
                                if (ShouldUnhide.ContainsKey(view.Id))
                                    ShouldUnhide[view.Id].Add(catId);
                                else
                                    ShouldUnhide[view.Id] = new List<ElementId> { cat.Id };
                            }

                            if (hide || (ShouldUnhide.ContainsKey(view.Id) && ShouldUnhide[view.Id].Contains(catId)))
                            {
                                view.SetCategoryHidden(catId, hide);
                            }
                        }
#endif
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog td = new TaskDialog(ex.Message)
                {
                    MainContent = ex.StackTrace
                };
                td.Show();
            }
        }

        private static View getViewOrTemplate(View view)
        {
            ElementId templateId = view.ViewTemplateId;
            if (templateId != ElementId.InvalidElementId)
            {
                 return view.Document.GetElement(templateId) as View;
            }
            else
            {
                return view;
            }
        }

        private static Dictionary<ElementId, List<ElementId>> ShouldUnhide;

        public static void Application_DocumentPrinting(object sender, DocumentPrintingEventArgs e)
        {
            DoHiding(e.Document, e.GetViewElementIds().ToList(), true);
        }

    }
}
