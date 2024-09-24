using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Runtime.InteropServices;


using Autodesk.Revit .DB;
using Autodesk.Revit.UI;
using Autodesk.Revit .ApplicationServices;
using Autodesk.Revit.Attributes ;


namespace LevelGenerator
{
  // This command is used to create multiple levels by one command. 

  [TransactionAttribute(TransactionMode.Manual)]
  [RegenerationAttribute(RegenerationOption.Manual)]
  public class LevelGenerator : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData, 
      ref string messages, 
      ElementSet elements)
    {

      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;
 
      // Check its a family document.

      if(doc.IsFamilyDocument == true)
      {
        MessageBox.Show("This command cannot be used for family files.","Level Generator");
        return Result.Cancelled;
      }

      // Read all existing levels in the current document 
      // (elevation and name).

      FilteredElementCollector collector = new FilteredElementCollector(doc);
      collector.OfClass(typeof(Level));
      
      List<LevelDefinition> existinglevels = new List<LevelDefinition>();          
      foreach (Element elem in collector)
      {
        Level level = elem as Level;
        LevelDefinition ld = new LevelDefinition();
        ld.mElevation = level.Elevation;
        ld.mName = level.Name;
        ld.mExisting = true;
        ld.mElementId = level.Id;
        existinglevels.Add(ld);
      }

      // Sort the levels so that they are correctly ordered in the 
      // dialog level table.

      existinglevels.Sort(new LevelComparer());

#if PREFORGETYPEID
            FormatOptions unitOptions = null;
      unitOptions = doc.GetUnits().GetFormatOptions(UnitType.UT_Length);
#else
        FormatOptions unitOptions = null;
        unitOptions = doc.GetUnits().GetFormatOptions(SpecTypeId.Length);
#endif            
            // If there are no levels in the model, need to specify the 
            // base level.

            double baseElevation = 0.0;

      if (existinglevels.Count == 0)
      {
        FormElevation elevationForm = new FormElevation(unitOptions);
        if (elevationForm.ShowDialog() == DialogResult.OK)
        {
          baseElevation = elevationForm.baseElevation;
        }
      }

      // Show the main form.

      AddLevelForm form = new AddLevelForm(existinglevels,unitOptions);
      form.baseElevation = baseElevation;

      if (form.ShowDialog() == DialogResult.Cancel)
        return Result.Cancelled;

      // Get the newly added level definition data

      List<double> levels = null;
      List<string> levelNames = null;      
      levels = form.levelList;
      levelNames = form.levelNames;   

      // Add the level definition to the document.

      try
      {
        // Create levels.

        Transaction trans = new Transaction(doc, "LevelGenerator");                
        trans.Start();        
        CreateLevels(doc, levels, levelNames);
        trans.Commit();
      }
      catch (Exception ex)
      {
        messages = ex.Message;
        return Result.Failed;
      }
      
      return Result.Succeeded;
    }

    // The function to create levels.

    private bool CreateLevels(
      Document doc,
      List<double> levels, 
      List<string> names)
    {
      string analyticalSuffix = FormSettingAccess.Read("LevelGenerator",
        "AnalyticalSuffix", " - Analytical", 100);

      // Write this back to the file for the first time. So user can 
      // change the suffix through the ini file.

      FormSettingAccess.Write(
        "LevelGenerator","AnalyticalSuffix", analyticalSuffix);

      FilteredElementCollector collector = new FilteredElementCollector(doc);
      collector.OfClass(typeof(ViewFamilyType));

      ViewFamilyType planViewVFT = null;
      ViewFamilyType ceilingViewVFT = null;
      ViewFamilyType structuralViewVFT = null;

      foreach (Element elem in collector)
      {
        ViewFamilyType vfType = elem as ViewFamilyType;
        if(vfType.ViewFamily == ViewFamily.FloorPlan)
        {
          planViewVFT = vfType;
        }
        else if(vfType.ViewFamily  == ViewFamily.CeilingPlan)
        {
          ceilingViewVFT = vfType;
        }
        else if (vfType.ViewFamily == ViewFamily.StructuralPlan)
        {
          structuralViewVFT = vfType;
        }        
      }


      for (int i = 0; i < levels.Count; i++)
      {
        Level level = Level.Create(doc,levels[i]);
        level.Name = names[i];
        
          

        //doc.Create.NewViewPlan(names[i], level, ViewPlanType.FloorPlan); //for Revit 2012/13

        ViewPlan viewPlan = ViewPlan.Create(doc,planViewVFT.Id,level.Id);  // for Revit 2014
        viewPlan.Name = names[i];
          
        // Revit Structure doesn't suppor the ceiling plan view.

        if(doc.Application.Product == ProductType.Architecture)
        {
          //doc.Create.NewViewPlan(names[i], level, ViewPlanType.CeilingPlan); //for Revit 2012/13

          viewPlan = ViewPlan.Create(doc, ceilingViewVFT.Id, level.Id);  // for Revit 2014
          viewPlan.Name = names[i];
        }

        // Create the analytical plan view in Revit Structure

        if (doc.Application.Product == ProductType.Structure)
        {
          //doc.Create.NewViewPlan(names[i] + analyticalSuffix, level, ViewPlanType.FloorPlan); //for Revit 2012/13

          viewPlan = ViewPlan.Create(doc, structuralViewVFT.Id, level.Id);  // for Revit 2014
          viewPlan.Name = names[i] + analyticalSuffix;
        }
      }

      return true;

    }

    // Get the level object with the specified name from 
    // the target document.
    
    private Level GetLevelWithName(Document doc, string levelName)
    {
      Level targetLevel = null;

      FilteredElementCollector collector;
      collector = new FilteredElementCollector(doc);

      collector.OfClass(typeof(Level));

      var levels = from elem in collector
                   where elem.Name.Equals(levelName)
                   select elem;

      if (levels.Count() > 0)
      {
        targetLevel = levels.First() as Level;
      }
      return targetLevel;

    }
  }

  // Helper function to compare levels when sorting.

  public class LevelComparer : IComparer<LevelDefinition>
  {
#region IComparer<LevelDefinition> Members

    public int Compare(LevelDefinition x, LevelDefinition y)
    {
      return y.mElevation.CompareTo(x.mElevation);
    }

#endregion
  }




}