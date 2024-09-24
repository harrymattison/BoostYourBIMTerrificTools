using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoostYourBIMTerrificTools.SelectByType
{
    [Transaction(TransactionMode.ReadOnly)]
    public class SelectByType : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            FormSelectByType form = new FormSelectByType(doc);
            form.Show();
            return Result.Succeeded;
        }
    }
}
