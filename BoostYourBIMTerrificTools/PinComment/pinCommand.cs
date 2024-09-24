using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;

namespace BoostYourBIMTerrificTools.PinComment
{
    [Transaction(TransactionMode.ReadOnly)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            Application app = commandData.Application.Application;
            if (BoostYourBIMTerrificTools.Utils.btnPinComment.ItemText == "Enable Pin Comment")
            {
                app.DocumentChanged += Utils.ControlledApplication_DocumentChanged;
                Properties.Settings.Default.PinComment = true;

                BoostYourBIMTerrificTools.Utils.btnPinComment.ItemText = "Disable Pin Comment";
            }
            else
            {
                app.DocumentChanged -= Utils.ControlledApplication_DocumentChanged;
                Properties.Settings.Default.PinComment = false;
                BoostYourBIMTerrificTools.Utils.btnPinComment.ItemText = "Enable Pin Comment";
            }

            Properties.Settings.Default.Save();
            return Result.Succeeded;
        }
    }
}
