using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LevelDisplacer
{
    public partial class FrmLevelDisplacer : Form
    {
        public FrmLevelDisplacer(Autodesk.Revit.DB.Document doc)
        {
            InitializeComponent();

            if (doc.DisplayUnitSystem == Autodesk.Revit.DB.DisplayUnit.IMPERIAL)
            {
                grpOffset.Text = "Offset Increments in ft";
                txtZ.Text = "20";
            }
            else
            {
                grpOffset.Text = "Offset Increments in mm";
                txtZ.Text = "6000";
            }
        }

        public bool getHide()
        {
            return chkHide.Checked;
        }

        public double getX()
        {
            double d = 0;
            Double.TryParse(txtX.Text, out d);
            return d;
        }
        public double getY()
        {
            double d = 0;
            Double.TryParse(txtY.Text, out d);
            return d;
        }
        public double getZ()
        {
            double d = 0;
            Double.TryParse(txtZ.Text, out d);
            return d;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            double d = 0;
            bool errorX = !Double.TryParse(txtX.Text, out d);
            bool errorY = !Double.TryParse(txtY.Text, out d);
            bool errorZ = !Double.TryParse(txtZ.Text, out d);

            if (errorX || errorY || errorZ)
                Autodesk.Revit.UI.TaskDialog.Show("Error", "All increments must be numbers.");
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

        private void lblPatreon_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.patreon.com/BoostYourBIM");
        }

        private void lblLearn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://boostyourbim.wordpress.com/learn/");
        }
    }
}
