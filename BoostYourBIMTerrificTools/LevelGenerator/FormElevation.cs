using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;

namespace LevelGenerator
{
  public partial class FormElevation : System.Windows.Forms.Form
  {
    public FormatOptions unitFormat;
    public double baseElevation;
    public FormElevation()
    {
      InitializeComponent();
    }

    public FormElevation(FormatOptions format)
    {
      InitializeComponent();
      unitFormat = format;
    }

    // Event handler for the button1 button.

    private void btnClose_Click(object sender, EventArgs e)
    {
      if (TBElevation.Text != "")
      {
        baseElevation = UnitConversion.ToDecimalFeet(
          TBElevation.Text, unitFormat);
      }
      else
        return;
      
      DialogResult = System.Windows.Forms.DialogResult.OK;
    }

  }
}
