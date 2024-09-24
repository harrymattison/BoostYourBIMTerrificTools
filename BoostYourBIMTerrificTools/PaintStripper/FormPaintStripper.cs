using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoostYourBIMTerrificTools.PaintStripper
{
    public partial class FormPaintStripper : System.Windows.Forms.Form
    {
        Document doc;
        public FormPaintStripper(Document _doc)
        {
            InitializeComponent();

            doc = _doc;
            List<Utils.NameIDObject> list = new FilteredElementCollector(doc)
                .OfClass(typeof(Material))
                .OrderBy(q => q.Name)
                .Select(q => new Utils.NameIDObject(q.Name, q.Id.IntegerValue))
                .ToList();

            lstMaterials.DataSource = list;
            lstMaterials.DisplayMember = "Name";
            lstMaterials.ValueMember = "IdValue";
            if (lstMaterials.Items.Count > 0)
                lstMaterials.SelectedIndex = 0;
        }

        public List<ElementId> getIds()
        {
            List<ElementId> ids = new List<ElementId>();
            foreach (Utils.NameIDObject nameIDObject in lstMaterials.SelectedItems.Cast<Utils.NameIDObject>())
            {
                ids.Add(new ElementId(nameIDObject.IdValue));
            }
            return ids;
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

        private void setSelected(ListBox lb, bool b)
        {
            for (int i = 0; i < lb.Items.Count; i++)
            {
                lb.SetSelected(i, b);
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            setSelected(lstMaterials, true);
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            setSelected(lstMaterials, false);
        }
    }
}
