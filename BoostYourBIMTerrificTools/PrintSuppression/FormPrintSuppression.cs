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

namespace BoostYourBIMTerrificTools.PrintSuppression
{
    public partial class FormPrintSuppression : System.Windows.Forms.Form
    {
        public FormPrintSuppression(Document doc)
        {
            InitializeComponent();
            TreeViewWithCheckBoxes.Window catTree = new TreeViewWithCheckBoxes.Window();
            elementHost.Child = catTree;            
            TreeViewWithCheckBoxes.Window window = elementHost.Child as TreeViewWithCheckBoxes.Window;
            System.Windows.Controls.TreeView tree = window.tree;

            if (Properties.Settings.Default.PrintSuppression != null)
            {
                List<Category> catsAndSubcats = Utils.GetCatsAndSubCats(doc);
                foreach (string s in Properties.Settings.Default.PrintSuppression)
                {
                    Category cat = catsAndSubcats.FirstOrDefault(q => q.Id.IntegerValue == int.Parse(s));
                    if (cat == null)
                        continue;

                    if (cat.Parent != null)
                    {
                        TreeViewWithCheckBoxes.FooViewModel parent = tree.Items.Cast<TreeViewWithCheckBoxes.FooViewModel>().FirstOrDefault(q => q.Name == cat.Parent.Name);
                        if (parent != null && parent.Children != null)
                        {
                            TreeViewWithCheckBoxes.FooViewModel child = parent.Children.FirstOrDefault(q => q.Name == cat.Name);
                            if (child != null)
                                child.IsChecked = true;
                        }
                    }
                    else
                    {
                        TreeViewWithCheckBoxes.FooViewModel parent = tree.Items.Cast<TreeViewWithCheckBoxes.FooViewModel>().FirstOrDefault(q => q.Name == cat.Name);
                        parent.IsChecked = true;
                    }
                }
            }
        }

        public List<ElementId> GetCategories()
        {
            TreeViewWithCheckBoxes.Window window = elementHost.Child as TreeViewWithCheckBoxes.Window;
            System.Windows.Controls.TreeView tree = window.tree;
            List<ElementId> ret = new List<ElementId>();
            foreach (TreeViewWithCheckBoxes.FooViewModel parent in tree.Items.Cast<TreeViewWithCheckBoxes.FooViewModel>())
            {
                if (parent.IsChecked != null && (bool)parent.IsChecked)
                {
                    ret.Add(parent.Id);
                }
                foreach (TreeViewWithCheckBoxes.FooViewModel child in parent.Children)
                {
                    if (child.IsChecked != null && (bool)child.IsChecked)
                    {
                        ret.Add(child.Id);
                    }
                }
            }
            return ret;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.OK;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.Cancel;
        }
    }
}
