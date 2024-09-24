using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoostYourBIMTerrificTools
{
    [Transaction(TransactionMode.ReadOnly)]
    public class Contact : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            Process.Start("https://boostyourbim.wordpress.com/contact/");
            return Result.Succeeded;
        }
    }
}
