using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Architecture;

namespace BoostYourBIMTerrificTools.SelectByType
{
    public partial class FormSelectByType : System.Windows.Forms.Form
    {
        Document doc;
        public FormSelectByType(Document _doc)
        {
            InitializeComponent();
            doc = _doc;

            cboCatType.Items.Add("Model");
            cboCatType.Items.Add("Annotation");
            cboCatType.Items.Add("Analytical");
            cboCatType.SelectedIndex = 0;

            lblNumber.Text = new UIDocument(doc).Selection.GetElementIds().Count.ToString();
        }

        private void lstCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTypes();
        }

        private void SetTypes()
        {
            if (lstCat.SelectedItem == null)
                lstTypes.DataSource = null;

            lstTypes.DataSource = GetTypes((Utils.NameIDObject)lstCat.SelectedItem);
            lstTypes.DisplayMember = "Name";
            lstTypes.ValueMember = "IdValue";
        }

        private List<Utils.NameIDObject> GetTypes(Utils.NameIDObject nameIdCategory)
        {
            List<Utils.NameIDObject> types;
            if (nameIdCategory.IdValue == (int)BuiltInCategory.OST_Lines)
            {
                types = doc.Settings.Categories
                    .get_Item(BuiltInCategory.OST_Lines)
                    .SubCategories.Cast<Category>()
                    .OrderBy(q => q.Name)
                    .Select(q => new Utils.NameIDObject(q.Name, q.Id.IntegerValue)).ToList();

                types = types
                    .Where(q => DoLinesExist(new ElementId(q.IdValue)))
                    .OrderBy(q => q.Name).ToList();
            }
            else
            {
                List<ElementType> elementTypes = new FilteredElementCollector(doc)
                    .WhereElementIsElementType()
                    .OfCategoryId(new ElementId(nameIdCategory.IdValue))
                    .Cast<ElementType>()
                    .ToList();

                if (nameIdCategory.IdValue == -2000260)
                {
                    elementTypes = new FilteredElementCollector(doc)
                    .OfClass(typeof(DimensionType))
                    .Cast<ElementType>()
                    .ToList();
                }

                if (!elementTypes.Any())
                {
                    return new List<Utils.NameIDObject>();
                }

                if (elementTypes.First() is FamilySymbol)
                {
                    types = elementTypes.Cast<FamilySymbol>()
                        .Select(q => new Utils.NameIDObject(q.FamilyName + ": " + q.Name, q.Id.IntegerValue))
                        .ToList();
                }
                else if (elementTypes.First() is StairsType)
                {
                    types = elementTypes.Cast<StairsType>()
                        .Select(q => new Utils.NameIDObject(q.ConstructionMethod + ": " + q.Name, q.Id.IntegerValue))
                        .ToList();
                }
                else if (elementTypes.First() is ConduitType)
                {
                    types = elementTypes.Cast<ConduitType>()
                        .Where(q => q.IsWithFitting)
                        .Select(q => new Utils.NameIDObject("Conduit with Fittings: " + q.Name, q.Id.IntegerValue))
                        .ToList();
                    types.AddRange(elementTypes.Cast<ConduitType>()
                        .Where(q => !q.IsWithFitting)
                        .Select(q => new Utils.NameIDObject("Conduit without Fittings: " + q.Name, q.Id.IntegerValue))
                        .ToList());
                }
                else if (elementTypes.First() is CableTrayType)
                {
                    types = elementTypes.Cast<CableTrayType>()
                        .Where(q => q.IsWithFitting)
                        .Select(q => new Utils.NameIDObject("Cable Tray with Fittings: " + q.Name, q.Id.IntegerValue))
                        .ToList();
                    types.AddRange(elementTypes.Cast<CableTrayType>()
                        .Where(q => !q.IsWithFitting)
                        .Select(q => new Utils.NameIDObject("Cable Tray without Fittings: " + q.Name, q.Id.IntegerValue))
                        .ToList());
                }
#if !RELEASE2015 && !RELEASE2016 && !RELEASE2017 && !RELEASE2018
                else if (elementTypes.First() is MEPCurveType)
                {
                    types = elementTypes.Cast<MEPCurveType>()
                        .Select(q => new Utils.NameIDObject(q.Shape + ": " + q.Name, q.Id.IntegerValue))
                        .ToList();
                }
#endif
                else
                {
                    types = elementTypes
                        .Select(q => new Utils.NameIDObject(q.Name, q.Id.IntegerValue))
                        .ToList();
                }
                types = types
                .Where(q => DoInstancesExist(new ElementId(q.IdValue)))
                .OrderBy(q => q.Name).ToList();
            }
            if (txtFilter.Text == "")
                return types;
            else
                return types.Where(q => q.Name.Contains(txtFilter.Text)).ToList();
        }

        private bool DoLinesExist(ElementId id)
        {
            if (new FilteredElementCollector(doc)
                .OfClass(typeof(CurveElement))
                .Cast<CurveElement>()
                .Where(q => ((GraphicsStyle)q.LineStyle).GraphicsStyleCategory.Id == id).Any())
                return true;

            return false;
        }

        private bool DoInstancesExist(ElementId id)
        {
            if (doc.GetElement(id) is FamilySymbol &&
                new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .WherePasses(new FamilyInstanceFilter(doc, id))
                .Any())
                return true;

            if (new FilteredElementCollector(doc)
                .WhereElementIsNotElementType()
                .Where(q => q.GetTypeId() == id)
                .Any())
                return true;

            return false;
        }

        private void btnClose1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            UIDocument uidoc = new UIDocument(doc);
            List<ElementId> selected = uidoc.Selection.GetElementIds().ToList();
            List<Element> elements;

            FilteredElementCollector collector;
            if (chkCurrentView.Checked)
                collector = new FilteredElementCollector(doc, uidoc.ActiveView.Id);
            else
                collector = new FilteredElementCollector(doc);

            foreach (Utils.NameIDObject nameId in lstTypes.SelectedItems)
            {
                if (((Utils.NameIDObject)lstCat.SelectedItem).Name == "Lines")
                {
                    elements = collector
                        .OfClass(typeof(CurveElement))
                        .Cast<CurveElement>()
                        .Where(q => ((GraphicsStyle)q.LineStyle).GraphicsStyleCategory.Id.IntegerValue == nameId.IdValue)
                        .Cast<Element>()
                        .ToList();
                }
                else
                {
                    ElementId typeId = new ElementId(nameId.IdValue);
                    elements = collector
                        .WhereElementIsNotElementType()
                        .Where(q => q.GetTypeId() == typeId)
                        .ToList();
                }
                selected.AddRange(elements.Select(q => q.Id).ToList());
            }
            uidoc.Selection.SetElementIds(selected);
            lblNumber.Text = uidoc.Selection.GetElementIds().Count.ToString();
        }

        private void btnClearSel_Click(object sender, EventArgs e)
        {
            UIDocument uidoc = new UIDocument(doc);
            uidoc.Selection.SetElementIds(new List<ElementId> { });
            lblNumber.Text = "0";
        }

        private void cboCatType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstTypes.DataSource = null;

            CategoryType ctype = CategoryType.Model;
            if (cboCatType.SelectedItem.ToString() == "Annotation")
                ctype = CategoryType.Annotation;
            else if (cboCatType.SelectedItem.ToString() == "Analytical")
                ctype = CategoryType.AnalyticalModel;

            List<Utils.NameIDObject> cats = doc.Settings.Categories.Cast<Category>()
#if !RELEASE2015 && !RELEASE2016 && !RELEASE2017 && !RELEASE2018 && !RELEASE2019
                .Where(q => q.IsVisibleInUI)
#endif
                .Where(q => q.CategoryType == ctype)
                .Where(q => new FilteredElementCollector(doc).OfCategoryId(q.Id).Any())
                .Select(q => new Utils.NameIDObject(q.Name, q.Id.IntegerValue))
                .OrderBy(q => q.Name)
                .Where(q => GetTypes(q).Any())
                .ToList();

            lstCat.DataSource = cats;
            lstCat.DisplayMember = "Name";
            lstCat.ValueMember = "IdValue";
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            SetTypes();
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstTypes.Items.Count; i++)
            {
                lstTypes.SetSelected(i, true);
            }
        }

        private void btnNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstTypes.Items.Count; i++)
            {
                lstTypes.SetSelected(i, false);
            }
        }
    }
}
