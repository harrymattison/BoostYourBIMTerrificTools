using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoostYourBIMTerrificTools.PinComment
{
    public static class Utils
    {
        public readonly static string PinParameterName = "Pin Comment";
        public static List<Element> pinnedElements;

        public class EnterPinCommentEventHandler : IExternalEventHandler
        {
            public void Execute(UIApplication app)
            {
#if RELEASE2013 || RELEASE2014
                if (!pinnedElements.Where(q => q.get_Parameter(PinParameterName) != null).Any())
#else
                if (!pinnedElements.Where(q => q.LookupParameter(PinParameterName) != null).Any())
#endif
                {
                    TaskDialog.Show("Error", PinParameterName + " does not exist for any of the selected elements.");
                    return;
                }

                string text;
                using (PinCommentForm form = new PinCommentForm())
                {
                    form.ShowDialog();
                    text = form.getComment();
                }

                if (!string.IsNullOrEmpty(text))
                {
                    using (Transaction t = new Transaction(pinnedElements.First().Document, "Set Comment"))
                    {
                        t.Start();
                        foreach (Element e in pinnedElements)
                        {
#if RELEASE2013 || RELEASE2014
                            Parameter p = e.get_Parameter(PinParameterName);
#else
                            Parameter p = e.LookupParameter(PinParameterName);
#endif
                            if (p == null)
                            {
                                TaskDialog.Show("Error", PinParameterName + " does not exist for element " + e.Name + " " + e.Id.IntegerValue);
                                return;
                            }
                            p.Set(app.Application.Username + "|" + text);
                        }
                        t.Commit();
                    }
                }
            }

            public string GetName()
            {
                return "Pin Comment";
            }
        }


        public static void ControlledApplication_DocumentChanged(object sender, Autodesk.Revit.DB.Events.DocumentChangedEventArgs e)
        {
            List<string> transactions = e.GetTransactionNames().ToList();

            // The transaction name can be
            // Pin
            // Unpin
            // Toggle Pin

            if (transactions.FirstOrDefault(q => q.ToUpper().Contains("PIN")) == null)
                return;

            Document doc = e.GetDocument();
            UIDocument uidoc = new UIDocument(doc);
            List<Element> selectedElements = uidoc.Selection.GetElementIds().Select(q => doc.GetElement(q)).ToList();
            pinnedElements = selectedElements.Where(q => q.Pinned).ToList();
            if (pinnedElements.Any())
            {
                ExternalEvent.Create(new EnterPinCommentEventHandler()).Raise();
            }
            else
            {
                List<string> comments = new List<string>();
                foreach (Element element in e.GetModifiedElementIds().Select(q => doc.GetElement(q)).Where(q => !q.Pinned))
                {
#if RELEASE2013 || RELEASE2014
                    Parameter p = element.get_Parameter(PinParameterName);
#else
                    Parameter p = element.LookupParameter(PinParameterName);
#endif

                    if (p == null || string.IsNullOrEmpty(p.AsString()))
                        continue;

                    string[] ar = p.AsString().Split('|');
                    comments.Add(element.Category.Name + "  " + element.Name + "(id = " + element.Id.IntegerValue + ") was pinned by " + ar[0] + " because: " + ar[1]);
                }
                if (comments.Any())
                {
                    TaskDialog td = new TaskDialog("Pin Comment")
                    {
                        MainInstruction = string.Join(Environment.NewLine, comments)
                    };
                    td.Show();
                }
            }
        }
    }

}
