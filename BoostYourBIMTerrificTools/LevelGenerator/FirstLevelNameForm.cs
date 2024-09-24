using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LevelGenerator
{
  public partial class FirstLevelNameForm : Form
  {
    public string prefix;
    public string index;
    public string suffix;

    public FirstLevelNameForm()
    {
      InitializeComponent();
    }

    // Event handler for the Ok button.

    private void buttonOk_Click(object sender, EventArgs e)
    {
      if (TBNumber.Text == "")
      {
        MessageBox.Show("The first level number cannot be blank.");
        return;
      }
      prefix = TBPrefix.Text;
      suffix = TBSuffix.Text;
      index = TBNumber.Text ;
      
      this.DialogResult = DialogResult.OK;
    }


    // Event handler for the Cancel button.

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;

    }

  }
}
