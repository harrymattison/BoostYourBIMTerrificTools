using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoostYourBIMTerrificTools.DBSpy
{
    public static class Utils
    {
        public static EventHandlerXYZ XYZEventHandler;
        public static EventHandlerLine LineEventHandler;
        public static EventHandlerSolid SolidEventHandler;
        public static List<ElementId> transientElements;
        public static ExternalEvent transientEvent;
        public static DockablePaneId dockableSamplePaneId;
        public static System.Windows.Controls.TreeView tree;

        public static string GetName(Element e)
        {
            string FamilyName = "";
            string sortPrefix = "z";
            try
            {
                if (e is FamilyInstance fi)
                    FamilyName = fi.Symbol.Family.Name + " - ";
                else if (e is FamilySymbol fs)
                    FamilyName = sortPrefix + "Family Type - ";
                else if (e is HostObjAttributes)
                    FamilyName = sortPrefix + "Type - ";
                else
                    FamilyName = "";
            }
            catch (Exception ex)
            {

            }
            return FamilyName + e.Name + " - " + e.Id.IntegerValue;
        }

    }
}
