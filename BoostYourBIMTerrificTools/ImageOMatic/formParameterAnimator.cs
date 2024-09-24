using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
namespace BoostYourBIM
{
    public partial class formParameterAnimator : System.Windows.Forms.Form
    {
        public Element familyInstance = null;
       
        public formParameterAnimator(PhaseArray phases, Element fi, DisplayStyle displayStyle)
        {
            familyInstance = fi;
            InitializeComponent();

            btnFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // list of phases
            lstPhases.BeginUpdate();
            foreach (Phase phase in phases)
            {
                lstPhases.Items.Add(phase.Name);
            }
            lstPhases.EndUpdate();

            // select all items in the phase list
            lstPhases.BeginUpdate();
            for (int i = 0; i < lstPhases.Items.Count; i++)
            {
                lstPhases.SetSelected(i, true);
            }
            lstPhases.EndUpdate();

            // list of parameters
            if (fi == null)
            {
                tabControl1.SelectedTab = tabPhases;
            }
            else
            {
                if (fi is FamilyInstance)
                {
                    foreach (Parameter p in fi.Parameters)
                    {
                        if (p.IsReadOnly)
                            continue;

                        if ((p.StorageType == StorageType.Double || p.StorageType == StorageType.Integer) && p.Definition.Name != "Moves With Nearby Elements")
                            lstParam.Items.Add(p.Definition.Name);
                    }

                    if (lstParam.Items.Count > 0)
                    {
                        lstParam.SelectedIndex = 0;
                        tabControl1.SelectedTab = tabParameter;
                    }
                    else
                    {
                        TaskDialog.Show("Error", "No parameters found.");
                    }
                }
#if !R2013
                else if (fi is DisplacementElement)
                {
                    lstParam.Items.Add("X Displacement");
                    lstParam.Items.Add("Y Displacement");
                    lstParam.Items.Add("Z Displacement");
                    lstParam.SelectedIndex = 0;
                    tabControl1.SelectedTab = tabParameter;
                }
#endif

            }

            // list of display styles
            foreach (var v in Enum.GetValues(typeof(DisplayStyle)))
            {
                string name = v.ToString();
                if (name == "Wireframe" ||
                    name == "HLR" ||
                    name == "ShadingWithEdges" ||
                    name == "RealisticWithEdges" ||
                    name == "FlatColors"
                    )
                {
                    if (name == "ShadingWithEdges") name = "Shading";
                    if (name == "FlatColors") name = "Consistent Colors";
                    if (name == "RealisticWithEdges") name = "Realistic";
                    if (name == "HLR") name = "Hidden Line";
                    lstDisplayStyle.Items.Add(name);
                }
            }

            // set default based on displayStyle passed to the constructor
            if (displayStyle.ToString() == "Wireframe") lstDisplayStyle.SelectedItem = "Wireframe";
            if (displayStyle.ToString() == "HLR") lstDisplayStyle.SelectedItem = "Hidden Line";
            if (displayStyle.ToString() == "ShadingWithEdges") lstDisplayStyle.SelectedItem = "Shading";
            if (displayStyle.ToString() == "RealisticWithEdges") lstDisplayStyle.SelectedItem = "Realistic";
            if (displayStyle.ToString() == "FlatColors") lstDisplayStyle.SelectedItem = "Consistent Colors";

            lstDisplayStyle.SelectedItem = displayStyle.ToString();

            // list of file types
            foreach (var v in Enum.GetValues(typeof(ImageFileType)))
            {
                string name = v.ToString();
                switch (name)
                {
                    case "JPEGLossless":
                        name = "JPEG (high quality)";
                        break;
                    case "JPEGMedium":
                        name = "JPEG (medium quality)";
                        break;
                    case "JPEGSmallest":
                        name = "JPEG (low quality)";
                        break;
                }
                cboFileType.Items.Add(name);
            }
            cboFileType.SelectedIndex = 1;

            // list of fit direction
            foreach (var v in Enum.GetValues(typeof(FitDirectionType)))
            {
                string name = v.ToString();
                cboFitDir.Items.Add(name);
            }
            cboFitDir.SelectedIndex = 0;

            // Extents
            cboExtents.Items.Add("Current Window");
            cboExtents.Items.Add("Visible Portion of Current Window");
            cboExtents.SelectedIndex = 1;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string errors = "";

            errors += ImageOMatic.Utils.validateInt(txtPixels.Text, "Pixel size");

            if (tabControl1.SelectedTab == tabParameter)
            {
                errors += ImageOMatic.Utils.validateDouble(txtStart.Text, "Start");
                errors += ImageOMatic.Utils.validateDouble(txtEnd.Text, "End");
                errors += ImageOMatic.Utils.validateDouble(txtInc.Text, "Increment");
            }

            if (errors == "") // if previously tested items are not numbers, don't bother trying to test their values
            {
                if (Convert.ToInt32(txtPixels.Text) < 32)
                    errors += "Pixel size must be greater than 32\n";

                if (tabControl1.SelectedTab == tabParameter)
                {
                    if (Convert.ToDouble(txtInc.Text) == 0)
                        errors += "Increment cannot be 0.\n";
                    if (Convert.ToDouble(txtStart.Text) == Convert.ToDouble(txtEnd.Text))
                        errors += "Start value cannot equal End value.\n";
                    if (Convert.ToDouble(txtStart.Text) > Convert.ToDouble(txtEnd.Text) && Convert.ToDouble(txtInc.Text) > 0)
                        errors += "Start value cannot be greater than End value when increment is positive.\n";
                    if (Convert.ToDouble(txtStart.Text) < Convert.ToDouble(txtEnd.Text) && Convert.ToDouble(txtInc.Text) < 0)
                        errors += "Start value cannot be less than than End value when increment is negative.\n";
                }
            }

            if (tabControl1.SelectedTab == tabPhases && lstPhases.SelectedItems.Count == 0)
                errors += "One or more phases must be selected\n";

            if (errors.Length > 0)
                MessageBox.Show(errors, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }

        public string getSelectedTab()
        {
            return tabControl1.SelectedTab.Name;
        }

        public IList<string> getSelectedPhaseNames()
        {
            IList<string> phaseNames = new List<string>();
            foreach (object item in lstPhases.SelectedItems)
            {
                phaseNames.Add(item.ToString());
            }
            return phaseNames;
        }

        public int getPixels()
        {
            //if (Validator.CheckLicense().State == LicenseState.Invalid)
            //{
            //    TaskDialog task = new TaskDialog("Boost Your BIM");
            //    task.MainInstruction = "Upgrade for full functionality";
            //    task.MainContent = "Images in the free version are limited to 200 dpi. Upgrade to the full version to remove this restriction.\n\n" +
            //        "You will receieve a license via email shortly after upgrading";
            //    task.FooterText = "<a href=\"http://boostyourbim.wordpress.com/products\">boostyourbim.wordpress.com/products</a>";
            //    task.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Upgrade");
            //    task.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "Continue in Demo mode");
            //    TaskDialogResult taskResult = task.Show();
            //    if (taskResult == TaskDialogResult.CommandLink1)
            //        Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=UVN6XRNWK93HA");

            //    return 200;
            //}
            //else
                return Convert.ToInt32(txtPixels.Text);
        }

        public string getParameterName()
        {
            return lstParam.SelectedItem.ToString();
        }

        public ImageFileType getImageFileType()
        {
            string name = cboFileType.SelectedItem.ToString();
            switch (name)
                {
                    case "JPEG (high quality)":
                        name = "JPEGLossless";
                        break;
                    case "JPEG (medium quality)":
                        name = "JPEGMedium";
                        break;
                    case "JPEG (low quality)":
                        name = "JPEGSmallest";
                        break;
                }
            return (ImageFileType)Enum.Parse(typeof(ImageFileType), name);
        }

        public ExportRange getExportRange()
        {
            if (cboExtents.Text == "Current Window")
                return ExportRange.CurrentView;
            return ExportRange.VisibleRegionOfCurrentView;
        }

        public FitDirectionType getFitDir()
        {
            return (FitDirectionType)Enum.Parse(typeof(FitDirectionType), cboFitDir.SelectedItem.ToString());
        }

        public DisplayStyle getDisplayStyle()
        {
            string name = lstDisplayStyle.SelectedItem.ToString();
            if (name == "Shading") name = "ShadingWithEdges";
            if (name == "Consistent Colors") name = "FlatColors";
            if (name == "Realistic") name = "RealisticWithEdges";
            if (name == "Hidden Line") name = "HLR";
            return (DisplayStyle)Enum.Parse(typeof(DisplayStyle), name);
        }

        public double getEnd()
        {
            return Convert.ToDouble(txtEnd.Text);
        }

        public double getStart()
        {
            return Convert.ToDouble(txtStart.Text);
        }

        public double getInc()
        {
            return Convert.ToDouble(txtInc.Text);
        }

        public string getFolder()
        {
            return btnFolder.Text;
        }

        public bool getOpenFolder()
        {
            return chkOpenFolder.Checked;
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                 btnFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("Mailto:boostyourbim@gmail.com");
        }

        private void lstParam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstParam.Items.Count == 0)
                return;

            Parameter p = ImageOMatic.Utils.getParam(familyInstance, lstParam.SelectedItem.ToString());
#if !R2013
            if (familyInstance is DisplacementElement)
            {
                double val = p.AsDouble();
                txtStart.Text = Math.Round(val, 2).ToString();
                txtEnd.Text = (Math.Round(val, 2) + 10).ToString();
            }
            else
            {
                if (p.StorageType == StorageType.Double)
                {
                    double val = p.AsDouble();
#if PREFORGETYPEID
                    if (p.DisplayUnitType == DisplayUnitType.DUT_DECIMAL_DEGREES)
#else
                    if (p.GetUnitTypeId() == UnitTypeId.Degrees)
#endif
                        val = BoostYourBIMTerrificTools.Utils.RadiansToDegrees(val);
                    txtStart.Text = Math.Round(val, 2).ToString();
                    txtEnd.Text = (Math.Round(val, 2) + 10).ToString();
                }
                else if (p.StorageType == StorageType.Integer)
                {
                    int val = p.AsInteger();
                    txtStart.Text = val.ToString();
                    txtEnd.Text = (val + 5).ToString();
                    txtInc.Text = (1).ToString();
                }
            }
#else
            if (p.StorageType == StorageType.Double)
            {
                double val = p.AsDouble();
                if (p.DisplayUnitType == DisplayUnitType.DUT_DECIMAL_DEGREES)
                    val = BoostYourBIMUtils.Utils.RadiansToDegrees(val);
                txtStart.Text = Math.Round(val, 2).ToString();
                txtEnd.Text = (Math.Round(val, 2) + 10).ToString();
            }
            else if (p.StorageType == StorageType.Integer)
            {
                int val = p.AsInteger();
                txtStart.Text = val.ToString();
                txtEnd.Text = (val + 5).ToString();
                txtInc.Text = (1).ToString();
            }
#endif
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstParam.Items.Count == 0 && tabControl1.SelectedIndex == 0)
            {
                TaskDialog.Show("Error", "Cannot select a parameter to increment. No element was selected or no parameters exist for the selected element.");
                tabControl1.SelectedIndex = 1;
            }
        }
    }
}
