using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SectionBoxFit
{
    public partial class FrmSectionBoxFit : Form 
    {
        public FrmSectionBoxFit(Autodesk.Revit.DB.Document doc)
        {
            InitializeComponent();
            
            if (doc.DisplayUnitSystem == Autodesk.Revit.DB.DisplayUnit.IMPERIAL)
            {
                grpOffset.Text = "Offset in ft";
                txtOffset.Text = "5";
            }
            else
            {
                grpOffset.Text = "Offset in mm";
                txtOffset.Text = "1500";
            }
        }

        public double getOffset()
        {
            double d = 0;
            Double.TryParse(txtOffset.Text, out d);
            return d;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            double d = 0;
            bool errorOffset = !Double.TryParse(txtOffset.Text, out d);

            if (errorOffset)
                Autodesk.Revit.UI.TaskDialog.Show("Error", "Offset must be a number.");
            else if (d <= 0)
                Autodesk.Revit.UI.TaskDialog.Show("Error", "Offset must be greater than 0.");
            else
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
