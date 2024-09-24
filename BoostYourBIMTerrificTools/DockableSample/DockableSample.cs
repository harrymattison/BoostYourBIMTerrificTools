using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BoostYourBIMTerrificTools;
using System;
using System.Linq;

namespace DockableSample
{
    [Transaction(TransactionMode.Manual)]
    public class dockableSample : IExternalCommand
    {
        Result IExternalCommand.Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
#if !RELEASE2013
            DockablePane dp = commandData.Application.GetDockablePane(Utils.dockableSamplePaneId);
            dp.Show();
#endif
            return Result.Succeeded;
        }
    }

    internal class APIExternalEventHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            try
            {
                Document doc = app.ActiveUIDocument.Document;
                using (Transaction t = new Transaction(doc, "dockable sample"))
                {
                    t.Start();
#if !RELEASE2013 && !RELEASE2014
                    ViewDrafting newView = ViewDrafting.Create(doc, new FilteredElementCollector(doc)
                        .OfClass(typeof(ViewFamilyType)).Cast<ViewFamilyType>()
                        .FirstOrDefault(q => q.ViewFamily == ViewFamily.Drafting).Id);
                    t.Commit();
#endif
                }
            }
            catch
            {
            }
        }

        public string GetName()
        {
            return "DockableSample";
        }
    }
}