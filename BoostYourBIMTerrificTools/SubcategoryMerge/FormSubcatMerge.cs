using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace BoostYourBIMTerrificTools.SubcategoryMerge
{
    public partial class FormSubcatMerge : System.Windows.Forms.Form
    {
        Document _doc;
        public FormSubcatMerge(Document doc)
        {
            InitializeComponent();
            _doc = doc;
            List<Utils.NameIDObject> list = doc.Settings.Categories.Cast<Category>()
                .Where(q => q.SubCategories.Size > 1)
                .OrderBy(q => q.Name)
                .Select(q => new Utils.NameIDObject(q.Name, q.Id.IntegerValue))
                .ToList();

            lstCategory.DataSource = list;
            lstCategory.DisplayMember = "Name";
            lstCategory.ValueMember = "IdValue";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (GetNewCatName() == string.Empty)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Error", "Enter a new category name");
                return;
            }
            if (lstSubcat.SelectedItems.Count < 2)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Error", "Select at least two subcategories");
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void lstCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            Category cat = Category.GetCategory(_doc,
                new ElementId(((Utils.NameIDObject)lstCategory.SelectedItem).IdValue));
            List<Utils.NameIDObject> list = cat.SubCategories.Cast<Category>()
                .OrderBy(q => q.Name)
                .Select(q => new Utils.NameIDObject(q.Name, q.Id.IntegerValue))
                .ToList();

            lstSubcat.DataSource = list;
            lstSubcat.DisplayMember = "Name";
            lstSubcat.ValueMember = "IdValue";
        }

        public ElementId GetCategoryId()
        {
            return new ElementId(((Utils.NameIDObject)lstCategory.SelectedItem).IdValue);
        }
        
        public List<string> GetSubcatNames()
        {
            return lstSubcat.SelectedItems.Cast<Utils.NameIDObject>().Select(q => q.Name).ToList();
        }

        public string GetNewCatName()
        {
            return txtNewCatName.Text;
        }
    }
}
