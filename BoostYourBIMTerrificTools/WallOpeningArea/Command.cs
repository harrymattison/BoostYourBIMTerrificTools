#region Imported Namespaces

//.NET common used namespaces
using System;
using System.Collections.Generic;

//Revit.NET common used namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

#endregion

namespace WallOpeningArea
{
  [Transaction(TransactionMode.Manual)]
  [Regeneration(RegenerationOption.Manual)]
  public class Command : IExternalCommand
  {
    /// <summary>
    /// The one and only method required by the IExternalCommand 
    /// interface, the main entry point for a external command.
    /// </summary>
    /// <param name="commandData">Input argument providing access
    /// to the Revit application and its documents 
    /// and their properties.</param>
    /// <param name="message">Return argument to display a message to
    /// the user in case of error if Result is not Succeeded.</param>
    /// <param name="elements">Return argument to highlight elements 
    /// on the graphics screen if Result is not Succeeded.</param>
    /// <returns>Cancelled, Failed or Succeeded Result code.
    /// </returns>
    public Result Execute(
        ExternalCommandData commandData,
        ref string message,
        ElementSet elements)
    {
      Document doc = commandData.Application.ActiveUIDocument.
        Document;

      if (!(doc.ActiveView is View3D))
      {
        TaskDialog.Show("Require 3D View",
            "This command must be executed from a 3D " +
            "view with all relevant elements visible");
        return Result.Cancelled;
      }

      // Open a form to collect this value
      SearchConfigForm frm = new SearchConfigForm(commandData);
      if (frm.ShowDialog() != System.Windows.Forms.
          DialogResult.OK) return Result.Cancelled;
      double minOpeningValue = (double)frm.AreaField;

      WallAreaFunctions.CalculateWallOpeningAreas(
          commandData.Application.ActiveUIDocument, minOpeningValue);

      // Must return some code
      return Result.Succeeded;
    }
  }
}
