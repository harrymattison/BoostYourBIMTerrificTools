using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace WallOpeningArea
{
  public static class SharedParameterFunctions
  {
    public static string PARAMETER_SMALL_OPEN_NAME =
      "Area of Openings < 'Max Value'";
    public const string PARAMETER_TOTAL_OPEN_NAME =
      "Opening Area";
    public const string PARAMETER_GROUP_NAME =
      "Wall Opening Parameters";

        public static Parameter getParam(Element e, string s)
        {
            return e.Parameters.Cast<Parameter>().FirstOrDefault(q => q.Definition.Name == s);
        }

    public static string SharedParameterFilePath
    {
      get
      {
        return System.IO.Path.Combine(
          System.IO.Path.GetDirectoryName(
          System.Reflection.Assembly.GetExecutingAssembly().Location),
          "WallOpeningsSharedParam.txt");
      }
    }

    public static bool OpenOrCreateWallSharedParameter(
        Document doc)
    {
      // Check on one element if the parameter already exist
      FilteredElementCollector coll = 
        new FilteredElementCollector(doc);
      coll.OfClass(typeof(Wall));
      Element ele = coll.FirstElement();
      if (ele != null)
      {
        Parameter param = getParam(ele,
          PARAMETER_SMALL_OPEN_NAME);
        if (param != null) return true; //already exist
      }

      // Create if not exist
      try
      {
        if (!System.IO.File.Exists(SharedParameterFilePath))
          System.IO.File.Create(SharedParameterFilePath).Close();
      }
      catch 
      {
        TaskDialog.Show("Unable to create Shared Parameter file",
          "The plug-in could not create the required shared " +
          "parameter file at " + SharedParameterFilePath + 
          ". Command cancelled.");
        return false;
      }

      // Open the shared parameter file
      doc.Application.SharedParametersFilename =
          SharedParameterFilePath;
      DefinitionFile sharedParamDefFile =
          doc.Application.OpenSharedParameterFile();

      // Create a category set to apply the parameter
      CategorySet categorySet =
        doc.Application.Create.NewCategorySet();
      categorySet.Insert(doc.Settings.Categories.
        get_Item(BuiltInCategory.OST_Walls));
      categorySet.Insert(doc.Settings.Categories.
        get_Item(BuiltInCategory.OST_CurtainWallPanels));
      Binding binding = doc.Application.Create.
        NewInstanceBinding(categorySet);

      // Create a shared parameter group
      string groupName = PARAMETER_GROUP_NAME;
      DefinitionGroup sharedParamDefGroup =
          sharedParamDefFile.Groups.get_Item(groupName);
      if (sharedParamDefGroup == null)
        sharedParamDefGroup = sharedParamDefFile.Groups.
          Create(groupName);

      // Create the parameter definition for small openings
      // Check if exists, create if required
      Definition paramSmallOpeningDef =
          sharedParamDefGroup.Definitions.get_Item(
          PARAMETER_SMALL_OPEN_NAME);
      if (paramSmallOpeningDef == null)
#if !PRE_FORGETYPE
                paramSmallOpeningDef =
        sharedParamDefGroup.Definitions.Create(new ExternalDefinitionCreationOptions(
        PARAMETER_SMALL_OPEN_NAME,
        SpecTypeId.Area));
#else
                paramSmallOpeningDef =
            sharedParamDefGroup.Definitions.Create(new ExternalDefinitionCreationOptions(
            PARAMETER_SMALL_OPEN_NAME,
            ParameterType.Area));
#endif

            // Apply parameter for small openings to walls
            doc.ParameterBindings.Insert(paramSmallOpeningDef, binding);

      // Create the parameter definition for total openings
      // Check if exists, create if required
      Definition paramTotalOpeningDef =  
        sharedParamDefGroup.Definitions.get_Item( 
        PARAMETER_TOTAL_OPEN_NAME);
      if (paramTotalOpeningDef == null)
#if !PRE_FORGETYPE
                paramTotalOpeningDef =
            sharedParamDefGroup.Definitions.Create(new ExternalDefinitionCreationOptions(
            PARAMETER_TOTAL_OPEN_NAME, 
            SpecTypeId.Area));
#else
                paramTotalOpeningDef =
            sharedParamDefGroup.Definitions.Create(new ExternalDefinitionCreationOptions(
            PARAMETER_TOTAL_OPEN_NAME,
            ParameterType.Area));
#endif
            // Apply parameter for total openings to walls
            doc.ParameterBindings.Insert(paramTotalOpeningDef, binding);

      return true;
    }
  }
}
