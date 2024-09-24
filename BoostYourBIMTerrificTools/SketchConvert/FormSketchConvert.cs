using Autodesk.Revit.DB;
using System;
using System.Windows.Forms;
using Form = System.Windows.Forms.Form;

namespace BoostYourBIMTerrificTools.SketchConvert
{
    public partial class FormSketchConvert : Form
    {
        public FormSketchConvert(Element e)
        {
            InitializeComponent();
            foreach (var v in Enum.GetValues(typeof(Target)))
            {
                if (v.ToString() == "UNDEFINED" ||
                    "Autodesk.Revit.DB." + v.ToString() == e.GetType().ToString())
                    continue;

                listBox1.Items.Add(v.ToString());
            }
            listBox1.SelectedIndex = 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public Target getTarget()
        {
            Enum.TryParse(listBox1.SelectedItem.ToString(), out Target target);
            return target;
        }


    }
}
