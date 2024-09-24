
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
using System.IO;
using System.Windows.Media.Imaging;

#endregion

namespace WallOpeningArea
{
  [Regeneration(RegenerationOption.Manual)]
  class Application : IExternalApplication
  {
    const string PIOTM_PANEL_NAME = "Wall Opening";

    public Result OnStartup(UIControlledApplication application)
    {
      // get the executing assembly
      System.Reflection.Assembly dotNetAssembly = 
        System.Reflection.Assembly.GetExecutingAssembly();

      // create the push button data for our command
      PushButtonData pbdWallOpeningArea = new PushButtonData(
        "ADNP_WALL_OPENING_AREA", "Wall Opening Area",
        dotNetAssembly.Location,
        "WallOpeningArea.Command");
      pbdWallOpeningArea.LargeImage =
        NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(),
        "icon32.png");
            pbdWallOpeningArea.Image =
  NewBitmapImage(System.Reflection.Assembly.GetExecutingAssembly(),
  "icon16.png");

            pbdWallOpeningArea.LongDescription = "Measures area created by the Opening element, wall profile editing, and inserts (such as windows).\n" +
          "Data is stored in the parameters:\n" + SharedParameterFunctions.PARAMETER_SMALL_OPEN_NAME + "\n" + SharedParameterFunctions.PARAMETER_TOTAL_OPEN_NAME;

        pbdWallOpeningArea.ToolTip = "Set wall parameters that measure opening area.";
      ContextualHelp contextHelp = new ContextualHelp(ContextualHelpType.Url, "https://boostyourbim.wordpress.com/products/#WallOpening");
      pbdWallOpeningArea.SetContextualHelp(contextHelp);

    RibbonPanel panel = null;

    string panelName = "Boost Your BIM";

    foreach (RibbonPanel rp in application.GetRibbonPanels())
    {
        if (rp.Name == panelName)
        {
            panel = rp;
            break;
        }
    }
    if (panel == null)
        panel = application.CreateRibbonPanel(panelName);

            // finally add the item
            panel.AddItem(pbdWallOpeningArea);

      return Result.Succeeded;
    }


        BitmapImage NewBitmapImage(System.Reflection.Assembly a, string imageName)
        {
            string imagePath = typeof(Application).Namespace + ".ImageFiles." + imageName;
            Stream s = a.GetManifestResourceStream(imagePath);
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = s;
            img.EndInit();
            return img;
        }


    public Result OnShutdown(UIControlledApplication application)
    {
      return Result.Succeeded;
    }
  }
}
