using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace BoostYourBIMTerrificTools.DBSpy
{
#if !RELEASE2013 && !RELEASE2014
    [Transaction(TransactionMode.Manual)]
    public class DbSpy : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            Utils.tree.DataContext = new RevitDBViewModel(Database.GetCategories());
            DockablePane dp = commandData.Application.GetDockablePane(Utils.dockableSamplePaneId);
            dp.Show();
            Utils.transientElements = new List<ElementId>();
            commandData.Application.DockableFrameVisibilityChanged += Application_DockableFrameVisibilityChanged;
            Utils.transientEvent = ExternalEvent.Create(new TransientElementEventHandler());
            return Result.Succeeded;
        }
        private void Application_DockableFrameVisibilityChanged(object sender, Autodesk.Revit.UI.Events.DockableFrameVisibilityChangedEventArgs e)
        {
            if (e.DockableFrameShown || e.PaneId != Utils.dockableSamplePaneId)
                return;

            Utils.transientEvent.Raise();
        }
    }
#endif
}
