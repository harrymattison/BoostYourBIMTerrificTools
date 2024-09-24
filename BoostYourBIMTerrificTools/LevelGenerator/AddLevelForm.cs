using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace LevelGenerator
{
  public partial class AddLevelForm : System.Windows.Forms.Form
  {
    // The list to store level elevations.
    
    public List<double> levelList = new List<double>();

    // The list to store the level names.

    public List<string> levelNames = new List<string>();
    
    public IList<LevelDefinition> existingLevels = null;

    // Current document's length unit format.

    FormatOptions unitFormat = null;
    
    // The initial level name in the batch inserting.

    public string initialLevelName;

    // If no level in the document, 
    // store the base elevation of the first default level.

    public double baseElevation = 0;

    // Constructor

    public AddLevelForm(List<LevelDefinition> levels,
      FormatOptions format)
    {
      existingLevels = levels;
      unitFormat = format;

      InitializeComponent();
    }

    private void AddLevelForm_Load(object sender, EventArgs e)
    {
      InitializeGrid();
      string str = FormSettingAccess.Read("LevelGenerator",
      "prefix", "{{}}", 100);

      //Remove the start and end two letters. The reason is that 
      // GetPrivateProfileString doesn't read the start and end spaces.

      TBPrefix.Text = str.Substring(2, str.Length - 4);

      str = FormSettingAccess.Read("LevelGenerator",
        "suffix", "{{}}", 100);
      TBSuffix.Text = str.Substring(2, str.Length - 4);
      
    }

    // Initialize the grid with the existing level data.

    public void InitializeGrid()
    {

      // Gray cell style.

      DataGridViewCellStyle grayStyle = new DataGridViewCellStyle();      
      grayStyle.BackColor = System.Drawing.Color.LightGray;

      // Show existing levels.

      if (existingLevels.Count > 0)
      {
        for (int i = 0; i < existingLevels.Count; i++)
        {
          DataGridViewRow newRow = new DataGridViewRow();

          LevelGrid.Rows.Insert(i, newRow);
          LevelGrid[0, i].Value = i + 1;
          LevelGrid[1, i].Value = existingLevels[i].mName;

          SetCellFormatUnit(3, i, existingLevels[i].mElevation);
          LevelGrid.Rows[i].ReadOnly = true;
          this.LevelGrid.Rows[i].DefaultCellStyle = grayStyle;
        }
      }
      else if (existingLevels.Count == 0)
      {
        DataGridViewRow newRow = new DataGridViewRow();
        LevelGrid.Rows.Insert(0, newRow);
        LevelGrid[0, 0].Value = 1;
        LevelGrid[1, 0].Value = "Level 1";
        
        // If no level exists, set the default level to 
        // the elevation user specified.

        SetCellFormatUnit(3, 0, baseElevation);

        // Set the base default level elevation to 0.
        SetCellFormatUnit(2, 0, 0.0);
      }

      // Calculate level height. 
      // Level Height means the distance from the level to its upper
      // level.

      if (this.LevelGrid.Rows.Count > 1)
      {
        LevelGrid[2, LevelGrid.Rows.Count - 1].Value = "0";
        for (int i = LevelGrid.Rows.Count - 2; i >= 0; i--)
        {
          SetCellFormatUnit(2, i, GetCellDecimalFeet(3, i) -
            GetCellDecimalFeet(3, i + 1));

        }
      }

      // Set the level elevation and the index column gray
      // Make it look as if the two columns are read-only.

      LevelGrid.Columns[0].DefaultCellStyle = grayStyle;
      LevelGrid.Columns[3].DefaultCellStyle = grayStyle;
      
      // Calculate the value of the column index. (The first column). 

      ResetRowIndex();
    }
    
    // Event handler for the Ok button.

    private void btnOk_Click(object sender, EventArgs e)
    {

      // Clear the selected state for all selected levels. The 
      // following in this method will highlight levels that were 
      // not correctly defined.

      for (int i = 0; i < LevelGrid.Rows.Count; i++)
      {
        LevelGrid.Rows[i].Selected = false;
      }

      // Verify the given names for the new levels: 
      // No duplicated names exisit.  
      // The level height must be greater than 0. 

      // Check if there are levels with the same elevation.     

      int zeroOffsetNumber = 0;
      for (int i = 0; i < LevelGrid.Rows.Count - 1; i++)
      {
        if (GetCellDecimalFeet(2, i) < 0.000001)
        {
          LevelGrid.Rows[i].Selected = true;
          zeroOffsetNumber++;
        }
      }

      if (zeroOffsetNumber > 0)
      {
        MessageBox.Show("The selected Level Heights are 0 or negative. Please update the Level Height value."
             , "Level Generator");
        return;
      }

      // Check if there are duplicated level names. If there are, 
      // highlight rows that have duplicate level names.

      string levelName1 = null;
      string levelName2 = null;
      int duplicateLevelNames = 0;
      for (int i = 0; i < LevelGrid.Rows.Count - 1; i++)
      {
        levelName1 = LevelGrid[1, i].Value.ToString();
        
        // Compare the level names one by one.

        for (int j = i + 1; j < LevelGrid.Rows.Count - 1; j++)
        {
          levelName2 = LevelGrid[1, j].Value.ToString();

          if (levelName1.Equals(levelName2))
          {
            duplicateLevelNames += 2;
            LevelGrid.Rows[i].Selected = true;
            LevelGrid.Rows[j].Selected = true;
          }
        }
      }

      if (duplicateLevelNames > 0)
      {
        MessageBox.Show(
          "Some level names are duplicate.\r\n" +
          "Please speicify a unique name for each level."
          , "Level name check");
        return;
      }

      
      // Collect newly defined levels.

      levelList.Clear();
      levelNames.Clear();
      string levelName = null;      
      for (int i = 0; i < LevelGrid.Rows.Count; i++)
      {
        if (LevelGrid.Rows[i].ReadOnly == false)
        {
          levelName = LevelGrid[1, i].Value.ToString();

          levelList.Add(GetCellDecimalFeet(3, i));
          levelNames.Add(levelName);
        }
      }

      FormSettingAccess.Write(
      "LevelGenerator", "prefix", "{{" + TBPrefix.Text + "}}"
      );

      FormSettingAccess.Write(
      "LevelGenerator", "suffix", "{{" + TBSuffix.Text + "}}"
      );

      this.DialogResult = DialogResult.OK;
    }

    // Event handler for the Cancel button.

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }
    
    // Add the defined levels to the table

    private void btnAdd_Click(object sender, EventArgs e)
    {
        double d = 0;
        if (!Double.TryParse(textLevelHeight.Text, out d))
        {
            MessageBox.Show("Level Height must be a number.", "Level Generator");
            return;
        }

      // Check three necessary text box values
      // Check the start nubmer

      if (TBNumber.Text == "" || TBNumber.Text == null)
      {
        MessageBox.Show("Start Number cannot be blank."
            , "Level Generator");
        TBNumber.Focus();
        return;
      }

      // Check the Repeat Level text box.

      if (textCount.Text == "" || textCount.Text == null)
      {
        MessageBox.Show("Repeat Levels cannot be blank."
            , "Level Generator");
        textCount.Focus();
        return;
      }

      // Check the Level Height text box.

      if (textLevelHeight.Text == "" || textLevelHeight.Text == null)
      {
        MessageBox.Show("The Level Height text box cannot be blank."
            , "Level Generator");
        textLevelHeight.Focus();
        return;
      }
      
      // Convert the Level Height to the document units.

      double eachElevation = UnitConversion.ToDecimalFeet(
      textLevelHeight.Text, unitFormat);

      if (eachElevation < 0.00001)
      {
        MessageBox.Show("Level Height cannot be zero or negative.",
          "Level Generator");
        textLevelHeight.Focus();
        return;
      }

      int count = int.Parse(textCount.Text);
      if (count < 1)
      {
        MessageBox.Show("The number of levels cannot be less than 1.",
          "Level Generator");
        textCount.Focus();
        return;
      }

      // Compose the start level name of the set of levels that will 
      // be added to the table.
      // Get the suffix and prefix strings.

      initialLevelName = TBPrefix.Text + TBNumber.Text + TBSuffix.Text;
      string levelName = initialLevelName;
      string prefix = TBPrefix.Text;
      string suffix = TBSuffix.Text;
      int number = int.Parse(TBNumber.Text);

      // Read the existing name list.

      string[] existingLevelNames =
        new string[this.LevelGrid.Rows.Count];

      // Check if new level's name conflicts with existing names.
      
      int rowNumber = this.LevelGrid.Rows.Count;
      for (int j = 0; j < rowNumber; j++)
      {
        existingLevelNames[j] = this.LevelGrid[1, j].Value.ToString();
      }

      // Get current level's elevation value.

      DataGridViewRow row = this.LevelGrid.CurrentRow;
      double currentElevation = GetCellDecimalFeet(3, row.Index);
      
      string newLevelName = null;
      row = this.LevelGrid.CurrentRow;
      for (int i = 0; i < count; i++)
      {

        newLevelName = prefix + (number + i).ToString() + suffix;

        DataGridViewRow newRow = new DataGridViewRow();

        // After insert a new row, the selected row
        // is kept unchanged. 
        // Selected row index increases 1.

        LevelGrid.Rows.Insert(row.Index - i, newRow);
        LevelGrid[1, row.Index - i - 1].Value = newLevelName;
        SetCellFormatUnit(
         3, row.Index - i - 1,
         currentElevation + eachElevation * (i + 1.0)
         );
        SetCellFormatUnit(2, row.Index - i - 1, eachElevation);

      }

      LevelGrid.CurrentCell = LevelGrid[1, row.Index - count];

      ResetRowIndex();
      if (RecalculateLevelHeight() == true)
      {
        MessageBox.Show("Level Height cannot be negative.\r\n" +
          "Please delete some of the lower levels or reduce the height of the lower levels.", "Level Generator");
      }

      // After inserting levels to the table.
      // Set the current level to the topmost level just added.
      // To make it easy to continue inserting levels, set the 
      // Start Number value to  number = number + count.
      
      TBNumber.Text = (number + count).ToString();
    }


    // Recalculae the Index column value.
    // Ensure the Index value increases from bottom to top.

    private void ResetRowIndex()
    {
      int rowNumber = LevelGrid.Rows.Count;
      for (int i = 0; i < rowNumber; i++)
      {
        LevelGrid[0, i].Value = rowNumber - i;
      }

    }

    // Recalculate the level height from the level elevation value.

    private bool RecalculateLevelHeight()
    {
      bool negativeOffset = false;
      int rowNumber = LevelGrid.Rows.Count;
      double cellValue;
      for (int i = rowNumber - 2; i >= 0; i--)
      {
        if (LevelGrid.Rows[i].ReadOnly == false)
        {
          cellValue = GetCellDecimalFeet(3, i + 1)
            + GetCellDecimalFeet(2, i);
          SetCellFormatUnit(3, i, cellValue);
        }

        // Adjust the existing level's height.

        else  
        {
          cellValue = GetCellDecimalFeet(3, i)
            - GetCellDecimalFeet(3, i + 1);
          SetCellFormatUnit(2, i, cellValue);
          if (cellValue < 0.000001)
            negativeOffset = true;
        }
      }
      return negativeOffset;
    }


    // Event handler for the Remove button.
    // (Remove the selected rows).

    private void btnRemove_Click(object sender, EventArgs e)
    {
      int deletedRows = 0;
      DataGridViewSelectedRowCollection selectedRows = null;
      selectedRows = LevelGrid.SelectedRows;
      if (selectedRows.Count > 0)
      {
        foreach (DataGridViewRow row in selectedRows)
        {
          // Cannot delete existing row in the model.

          if (row.ReadOnly == false)
          {
            LevelGrid.Rows.Remove(row);
            deletedRows++;
          }
        }
      }
      else
      {

        // If no row is selected, remove the current row.
        
        DataGridViewRow row = this.LevelGrid.CurrentRow;
        if (row.ReadOnly == false)
        {
          this.LevelGrid.Rows.Remove(row);
          deletedRows++;
        }
      }

      if (deletedRows == 0)
      {
        MessageBox.Show("Existing levels cannot be deleted with this application.",
            "Delete levels");
      }
      else
      {
        ResetRowIndex();
        RecalculateLevelHeight();
      }
    }

    // Rename the selected rows name in batches.

    private void btnRenameLevel_Click(object sender, EventArgs e)
    {
      string prefix;
      string suffix;
      int number;

      // Don't allow existing level to be renamed.
      // If existing gray row is selected, return it.
      
      foreach (DataGridViewRow selectedRow in LevelGrid.SelectedRows)
      {
        if (selectedRow.ReadOnly == true)
        {
          MessageBox.Show("One or more existing levels are selected."
              + " Names of existing levels cannot be renamed.",
              "Level Generator");
          return;
        }
      }

      if (LevelGrid.SelectedRows.Count == 1)
      {
        if (MessageBox.Show("Only one level is selected."
            + " You can directly rename a level by typing "
            + "the new name in the level's Level Name cell.\n\r"
            + "This command is intended for renaming several selected levels.\r\n"
            + "Do you want to continue renaming the selected level in this way?"
            , "Rename selected levels", MessageBoxButtons.YesNo) 
            == DialogResult.No)
        {
          return;
        }
      }

      // Show the dialog to type the new name rule.

      FirstLevelNameForm form = new FirstLevelNameForm();
      if (DialogResult.OK == form.ShowDialog())
      {
        prefix = form.prefix;
        suffix = form.suffix;
        number = int.Parse(form.index);
      }
      else
      {
        return;
      }

   
      // Change all new level names.      
      
      for (int i = 0; i < LevelGrid.SelectedRows.Count; i++)
      {
        if (LevelGrid.SelectedRows[i].ReadOnly == false)
        {
          // Get the selected row index.

          int index = LevelGrid.SelectedRows[i].Index;

          LevelGrid[1, index].Value = prefix + (number + i).ToString()
            + suffix;
        }
      }
    }

    // After changing the level height cell, recalculate all the 
    // level heights and  elevations.

    private void LevelGrid_CellEndEdit(
      object sender, DataGridViewCellEventArgs e)
    {
      // Update the current cell's format in case user input is not 
      // formatted. 

      if (e.ColumnIndex == 2)
      {
        double feetValue = GetCellDecimalFeet(e.ColumnIndex, e.RowIndex);
        if (feetValue <= -0.0000001)
        {
          MessageBox.Show("The Level Height cannot be negative.");
          return;
        }
        SetCellFormatUnit(e.ColumnIndex, e.RowIndex, feetValue);

        RecalculateLevelHeight();
      }
    }


    // Get the specifiled cell's value.
    // The return value is decimal feet.

    private double GetCellDecimalFeet(int col, int row)
    {
      double result = 0.0;
      string cellText = LevelGrid[col, row].Value.ToString();

      result = UnitConversion.ToDecimalFeet(cellText, unitFormat);
      return result;
    }

    // Asign value to the specified cell. The unit depends on the 
    // document's length unit setting.

    // Call this to fill the value of  the Level Height and Level 
    // elevation.

    private void SetCellFormatUnit(int col, int row, double value)
    {
      string cellText = UnitConversion.ToFormatInUnit(value, unitFormat);
      LevelGrid[col, row].Value = cellText;
    }


    // Don't allow users to delete any row just by clicking the 
    // Del key. Because this action may delete an existing level in  
    // the table.

    private void LevelGrid_UserDeletingRow(object sender, 
      DataGridViewRowCancelEventArgs e)
    {      
      // Even if users press the Del key, nothing happens to the  
      // table content.

      e.Cancel = true;  
    }
  }


  public class LevelDefinition
  {
    public double mElevation;
    public string mName;

    //true= existing. false = new definition.

    public bool mExisting;

    //if mExisting is true, remember the ElementId.

    public Autodesk.Revit.DB.ElementId mElementId;
    public LevelDefinition()
    {
      mElevation = 0;
      mName = null;
      mExisting = true;
      mElementId = null;
    }
  }
  
  // The help function class.
  
  public class UnitConversion
  {
    // Convert any string to the value in the unit of the current 
    // document Length unit setting.

    public static double ToDecimalFeet(
      string value, FormatOptions format)
    {
      double result = 0;

#if PREFORGETYPEID
      switch (format.DisplayUnits)
      {
        case DisplayUnitType.DUT_DECIMAL_FEET:
          result = double.Parse(value);
          break;
        case DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES:
          result = GetDecimalFeetFromFeetAndFractionInches(value);
          break;
        case DisplayUnitType.DUT_DECIMAL_INCHES:
          result = double.Parse(value) / 12.0;
          break;
        case DisplayUnitType.DUT_FRACTIONAL_INCHES:
          result = GetDecimalFeetFromFractionInches(value);
          break;
        case DisplayUnitType.DUT_MILLIMETERS:
          result = double.Parse(value) / 304.8;
          break;
        case DisplayUnitType.DUT_CENTIMETERS:
          result = double.Parse(value) / 30.48;
          break;
        case DisplayUnitType.DUT_METERS:
          result = double.Parse(value) / 0.3048;
          break;
        case DisplayUnitType.DUT_METERS_CENTIMETERS:
          result = double.Parse(value) / 0.3048;
          break;
          }
#else
            if (format.GetUnitTypeId() == UnitTypeId.Feet)
            {
                result = double.Parse(value);
            }
            else if (format.GetUnitTypeId() == UnitTypeId.FeetFractionalInches)
            {
                result = GetDecimalFeetFromFeetAndFractionInches(value);
            }
            else if (format.GetUnitTypeId() == UnitTypeId.Inches)
            {
                result = double.Parse(value) / 12.0;
            }
            else if (format.GetUnitTypeId() == UnitTypeId.FractionalInches)
            {
                result = GetDecimalFeetFromFractionInches(value);
            }
            else if (format.GetUnitTypeId() == UnitTypeId.Millimeters)
            {
                result = double.Parse(value) / 304.8;
            }
            else if (format.GetUnitTypeId() == UnitTypeId.Centimeters)
            {
                result = double.Parse(value) / 30.48;
            }
            else if (format.GetUnitTypeId() == UnitTypeId.Meters)
            {
                result = double.Parse(value) / 0.3048;
            }
            else if (format.GetUnitTypeId() == UnitTypeId.MetersCentimeters)
            {
                result = double.Parse(value) / 0.3048;
            }
#endif
      return result;

    }

    // Convert the double value to a string that matches the unit
    // setting in the document.

    public static string ToFormatInUnit(
      double value, FormatOptions format)
    {
      string result = null;
      double inch = 0.0;
      string roundFormat = format.Accuracy.ToString();
      roundFormat = roundFormat.Replace("1", "0");

#if PREFORGETYPEID
            switch (format.DisplayUnits)
      {
        case DisplayUnitType.DUT_DECIMAL_FEET:
          result = value.ToString(roundFormat);
          break;
        case DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES:
          result = GetFeetIncheFromDecimalFeet(value);
          break;
        case DisplayUnitType.DUT_DECIMAL_INCHES:
          inch = value * 12.0;
          result = inch.ToString(roundFormat);
          break;
        case DisplayUnitType.DUT_FRACTIONAL_INCHES:
          result = GetFractionIncheFromDecimalFeet(value);
          break;
        case DisplayUnitType.DUT_MILLIMETERS:
          result = (value * 304.8).ToString(roundFormat);
          break;
        case DisplayUnitType.DUT_CENTIMETERS:
          result = (value * 30.48).ToString(roundFormat);
          break;
        case DisplayUnitType.DUT_METERS:
          result = (value * 0.3048).ToString(roundFormat);
          break;
        case DisplayUnitType.DUT_METERS_CENTIMETERS:
          result = (value * 0.3048).ToString(roundFormat);
          break;
      }
#else
            if (format.GetUnitTypeId() == UnitTypeId.Feet)
                result = value.ToString(roundFormat);
            else if (format.GetUnitTypeId() == UnitTypeId.FeetFractionalInches)
                result = GetFeetIncheFromDecimalFeet(value);
            else if (format.GetUnitTypeId() == UnitTypeId.Inches)
            {
                inch = value * 12.0;
                result = inch.ToString(roundFormat);
            }
            else if (format.GetUnitTypeId() == UnitTypeId.FractionalInches)
                    result = GetFractionIncheFromDecimalFeet(value);
            else if (format.GetUnitTypeId() == UnitTypeId.Millimeters)
                    result = (value * 304.8).ToString(roundFormat);
            else if (format.GetUnitTypeId() == UnitTypeId.Centimeters)
                    result = (value * 30.48).ToString(roundFormat);
            else if (format.GetUnitTypeId() == UnitTypeId.Meters)
                    result = (value * 0.3048).ToString(roundFormat);
            else if (format.GetUnitTypeId() == UnitTypeId.MetersCentimeters)
                    result = (value * 0.3048).ToString(roundFormat);


#endif
            return result;
    }


    // Convert the feet+inches to the decimal feet.

    public static double GetDecimalFeetFromFeetAndFractionInches(
 string value)
    {
      // The possible formats:
      // 20
      // 20'
      // 20'  0"
      // 10'  11"
      // 20'11"
      // 11"
      // 11 1/2"
      // 20' 11"
      // 10'  1 1/2"
      // 10'  10 1/2"
      // 10'  10 1/12"
      // 10'  10 10/12"

      double result = 0;

      // Find the feet part from the string.

      int feetIndex = value.IndexOf("'");
      
      // Get position of the fractional inch.

      int fractionSlashIndex = value.IndexOf("/");
      
      
      // Get the space between the inch and fractional inch.
      // (The separator is a space).

      int inchFractionSeparatorIndex = value.LastIndexOf(" ");
      int inchSymbolIndex = value.IndexOf("\"");

      string inch = null;
      string numerator = null;
      string denominator = null;
      double fractionInch = 0;
           
      string feet = null;

      try
      {
        if (feetIndex > -1)
        {
          feet = value.Substring(0, feetIndex);
          
          // Not found, no fractional inch.
          
          if (fractionSlashIndex == -1)
          {
            if (inchSymbolIndex > -1)
            {
              inch = value.Substring(feetIndex + 1,
                inchSymbolIndex - feetIndex - 1);
              
              // Remove space.

              inch.Trim(); 
            }
            else if (inchSymbolIndex == -1)
            {
              inch = value.Substring(feetIndex + 1,
                  value.Length - 1 - feetIndex);
              inch.Trim();
            }
          }
          else if (fractionSlashIndex > -1) // fractional
          {
            //10'  1 1/2"
            if (inchFractionSeparatorIndex > -1)
            {
              inch = value.Substring(feetIndex + 1,
                inchFractionSeparatorIndex - feetIndex);
              inch.Trim();
              
              // 10'  10 10/12"
              // Get numerator & denominator.

              numerator = value.Substring(inchFractionSeparatorIndex + 1,
                fractionSlashIndex - inchFractionSeparatorIndex - 1);
              numerator.Trim();
              if (inchSymbolIndex > -1)
              {
                denominator = value.Substring(fractionSlashIndex + 1,
                  inchSymbolIndex - fractionSlashIndex - 1);
                denominator.Trim();
              }
              else
              {
                denominator = value.Substring(fractionSlashIndex + 1,
                  value.Length - 1 - fractionSlashIndex);
                denominator.Trim();
              }
            }
            else if (inchFractionSeparatorIndex == -1)
            {
              //10'10/12"

              inch = null;
              numerator = value.Substring(feetIndex + 1,
                fractionSlashIndex - feetIndex - 1);
              numerator.Trim();
              if (inchSymbolIndex > -1)
              {
                denominator = value.Substring(fractionSlashIndex + 1,
                  inchSymbolIndex - fractionSlashIndex - 1);
                denominator.Trim();
              }
              else
              {
                denominator = value.Substring(fractionSlashIndex + 1,
                  value.Length - 1 - fractionSlashIndex);
                denominator.Trim();
              }
            }
          }
        }
        else  //no feet.
        {
          if (inchSymbolIndex == -1 && fractionSlashIndex == -1)
          {
            feet = value;
          }

          // 11"
          
          if (fractionSlashIndex == -1) //not found, no fractional inch.
          {
            if (inchSymbolIndex > -1)
            {
              inch = value.Substring(0,
                value.Length - 1);
              inch.Trim(); //remove space
            }
          }
          else if (fractionSlashIndex > -1) // fractional
          {
            // 10 10/12"

            if (inchFractionSeparatorIndex > -1)
            {
              inch = value.Substring(0, inchFractionSeparatorIndex);
              inch.Trim();
              // 10'  10 10/12"
              // get numerator & denominator.
              numerator = value.Substring(inchFractionSeparatorIndex + 1,
                fractionSlashIndex - inchFractionSeparatorIndex - 1);
              numerator.Trim();
              if (inchSymbolIndex > -1)
              {
                denominator = value.Substring(fractionSlashIndex + 1,
                  inchSymbolIndex - fractionSlashIndex - 1);
                denominator.Trim();
              }
              else
              {
                denominator = value.Substring(fractionSlashIndex + 1,
                  value.Length - 1 - fractionSlashIndex);
                denominator.Trim();
              }
            }
            else if (inchFractionSeparatorIndex == -1)
            {
              //  10/12"
              inch = null;
              numerator = value.Substring(0, fractionSlashIndex);
              numerator.Trim();
              if (inchSymbolIndex > -1)
              {
                denominator = value.Substring(fractionSlashIndex + 1,
                  inchSymbolIndex - fractionSlashIndex - 1);
                denominator.Trim();
              }
              else
              {
                denominator = value.Substring(fractionSlashIndex + 1,
                  value.Length - 1 - fractionSlashIndex);
                denominator.Trim();
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Level Generator");
      }

      if (feet != null && feet != "")
      {
        result = double.Parse(feet);
      }

      if (inch != null && inch != "" && inch != " ")
      {
        result += double.Parse(inch) / 12.0;
      }

      if (denominator != null && denominator != "")
      {
        fractionInch = 
          double.Parse(numerator) / double.Parse(denominator) / 12.0;
        result += fractionInch;
      }

      return result;
    }

    public static double GetDecimalFeetFromFractionInches(string value)
    {
      // The possible formats:
      // 121
      // 121"
      // 121 1/2
      // 121 1/2"      
      // 121 5/64"
      // 121 11/64"

      double result = 0;

      try
      {
        // Find the inch separator.
        int InchIndex = value.IndexOf(" ");
        // find the slash of the fraction.
        int fractionSlashIndex = value.IndexOf("/");
        int inchSymbolIndex = value.IndexOf("\"");

        // Read the inch.
        string inch = null;

        // No fraction part.

        if (InchIndex == -1)  
        {
          inch = value.Substring(0, value.Length - 1);
        }
        else
        {
          inch = value.Substring(0, InchIndex);
        }
        inch.Trim();

        string numerator = null;
        string denominator = null;
        double fractionInch = 0;
        if (fractionSlashIndex == -1) //not found, no fractional inch.
        {
          fractionInch = 0;
        }
        else
        {
          numerator = value.Substring(InchIndex + 1,
            fractionSlashIndex - InchIndex - 1);
          if (inchSymbolIndex > -1)
          {
            denominator = value.Substring(fractionSlashIndex + 1,
              inchSymbolIndex - fractionSlashIndex - 1);
          }
          else
          {
            denominator = value.Substring(fractionSlashIndex + 1,
              value.Length - fractionSlashIndex - 1);
          }
          numerator.Trim();
          denominator.Trim();
          fractionInch = 
            (double.Parse(numerator) / double.Parse(denominator));
        }

        // Get the decimal feet from the inches.
        
        result = (int.Parse(inch) + fractionInch) / 12.0;
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Level Generator");
      }


      return result;
    }

    // Reduction fraction.

    public static void ReductionFraction(
      ref int numerator, ref int denominator)
    {
      int remUp = 0;
      int remDown = 0;
      for (int i = 2; i <= numerator; i++)
      {
        Math.DivRem(numerator, i, out remUp);
        Math.DivRem(denominator, i, out remDown);
        if (numerator >= i)
        {
          if (remUp == 0 && remDown == 0)
          {
            numerator /= i;
            denominator /= i;
            ReductionFraction(ref numerator, ref denominator);
          }
        }
        else
        {
          return;
        }
      }
      return;
    }

    // Get the length in Feet Inches from a given double feet.

    public static string GetFeetIncheFromDecimalFeet(double value)
    {
      // 20'  0"
      // 10'  11"
      // 10'  1 1/2"
      // 10'  10 1/2"
      // 10'  10 1/12"

      string result = null;

      int feet = int.Parse(Math.Floor(value).ToString());
      int inch = int.Parse(Math.Floor((value - feet) * 12).ToString());
      int numerator = int.Parse(Math.Round((value - feet - inch / 12.0) 
        * 12.0 * 256.0).ToString());
      int denominator = 256;

      if (numerator == denominator)
      {
        inch++;
        numerator = 0;
      }

      if (inch == 12)
      {
        feet++;
        inch = 0;
      }
      ReductionFraction(ref numerator, ref denominator);

      if (numerator != 0)
        result = string.Format("{0}'  {1} {2}/{3}\"",
          feet,
          inch,
          numerator,
          denominator);
      else if (numerator == 0)
        result = string.Format("{0}'  {1}\"", feet, inch);

      return result;
    }

    //Get fraction inches from the decimal feet.

    public static string GetFractionIncheFromDecimalFeet(double value)
    {
      // 121.5" =   121 1/2"
      // 121"
      // 121 5/64"= 121.0781"
      // 121 11/64"

      string result = null;

      value = value * 12.0;  //convert to inch.

      int inch = int.Parse(Math.Floor(value).ToString());

      int numerator = 
        int.Parse(Math.Round((value - inch) * 256.0).ToString());
      int denominator = 256;

      if (numerator == denominator)
      {
        inch++;
        numerator = 0;
      }

      ReductionFraction(ref numerator, ref denominator);

      if (numerator != 0)
        result = string.Format("{0} {1}/{2}\"", inch, numerator
          , denominator);
      else
        result = string.Format("{0}\"", inch);

      return result;
    }

  }

  // Class used to read and write the ini file.

  class FormSettingAccess
  {
    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(
        string section, string key, string def, StringBuilder retVal,
        int size, string filePath);

    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(
        string section, string key, string val, string filePath);

    static string mConfigFilePath = Path.GetDirectoryName(
        Assembly.GetAssembly(typeof(FormSettingAccess)).Location)
        + "\\LGSettings.ini";

    public FormSettingAccess()
    {

    }
    
    // Static method. 
    // Write content to the ini file.

    public static void Write(
        string lpAppName, string lpKeyName, string lpString)
    {
      WritePrivateProfileString(
          lpAppName, lpKeyName, lpString, mConfigFilePath);
    }

    // Static method.
    // Read content from the ini file.

    public static string Read(string lpAppName, string lpKeyName,
        string lpDefault, int nSize)
    {
      StringBuilder temp = new StringBuilder(500);
      GetPrivateProfileString(lpAppName, lpKeyName, lpDefault,
          temp, nSize, mConfigFilePath);
      return temp.ToString();
    }
  }
}
