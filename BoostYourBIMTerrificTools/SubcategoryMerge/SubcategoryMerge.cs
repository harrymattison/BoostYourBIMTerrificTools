using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BoostYourBIMTerrificTools.SketchConvert;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;

namespace BoostYourBIMTerrificTools.SubcategoryMerge
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class SubcategoryMerge : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            ElementId categoryId;
            List<string> categoryNamesToMerge;
            string newCategoryName;
            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;

            using (FormSubcatMerge form = new FormSubcatMerge(doc))
            {
                if (form.ShowDialog() == DialogResult.Cancel)
                    return Result.Cancelled;
                categoryId = form.GetCategoryId();
                newCategoryName = form.GetNewCatName();
                categoryNamesToMerge = form.GetSubcatNames();
            }

            MergeFamilies(doc, categoryId, categoryNamesToMerge, newCategoryName);

            using (var t = new Transaction(doc, "Delete SubCategory"))
            {
                t.Start();
                var subcat = doc.Settings.Categories.Cast<Category>()
                    .FirstOrDefault(q => q.Id == categoryId)
                    .SubCategories.Cast<Category>()
                    .Where(q => categoryNamesToMerge.Contains(q.Name)).ToList();
                doc.Delete(subcat.Select(q => q.Id).ToList());
                t.Commit();
            }

            return Result.Succeeded;
        }

        private void MergeFamilies(Document doc, ElementId categoryId, List<string> namesOfSubcategoriesToMerge, string newCategoryName)
        {
            List<Family> families = new FilteredElementCollector(doc)
                .OfClass(typeof(Family))
                .Cast<Family>()
                .Where(q => 
                q.FamilyCategoryId == categoryId &&
                q.Name != string.Empty)
                .ToList();
            foreach (var family in families)
            {
                MergeFamilies(family, categoryId, namesOfSubcategoriesToMerge, newCategoryName);
            }
        }

        private void MergeFamilies(Family f, ElementId categoryId, List<string> namesOfSubcategoriesToMerge, string newCategoryName)
        {
            var document = f.Document;
            var famDoc = document.EditFamily(f);

            // update any subfamilies before this family
            MergeFamilies(famDoc, categoryId, namesOfSubcategoriesToMerge, newCategoryName);

            var parentCategory = famDoc.Settings.Categories.Cast<Category>().First(q => q.Id == categoryId);
            var subcategories = parentCategory.SubCategories.Cast<Category>();

            using (Transaction t = new Transaction(famDoc, "Merge Subcategories"))
            {
                t.Start();

                // new subcategory could already exist in this family if it was loaded in through a sub-family
                var newSubCat = parentCategory.SubCategories.Cast<Category>()
                    .FirstOrDefault(q => q.Name == newCategoryName) ?? famDoc.Settings.Categories.NewSubcategory(parentCategory, newCategoryName);
                
                foreach (var catName in namesOfSubcategoriesToMerge)
                {
                    Category catToMerge = null;
                    try
                    {
                        catToMerge = subcategories.FirstOrDefault(q => q.Name == catName);
                    }
                    catch
                    {
                        continue;
                    }
                    if (catToMerge == null)
                    {
                        continue;
                    }

                    if (catToMerge == null)
                        continue;
                    var elements = new FilteredElementCollector(famDoc)
                        .OfCategoryId(catToMerge.Id)
                        .ToList();
                    var curves = new FilteredElementCollector(famDoc)
                        .OfClass(typeof(CurveElement))
                        .Cast<CurveElement>().ToList();
                    var lineStyles = curves.Select(q => q.LineStyle.Id.IntegerValue).ToList();
                    var cutStyle = catToMerge.GetGraphicsStyle(GraphicsStyleType.Cut);
                    if (cutStyle != null)
                    {
                        var cutId = cutStyle.Id.IntegerValue;
                        var linesCut = curves
                            .Where(q => q.LineStyle.Id.IntegerValue == cutId)
                            .ToList();
                        foreach (var e in linesCut)
                        {
                            if (e is ModelCurve mc)
                            {
                                mc.Subcategory = newSubCat.GetGraphicsStyle(GraphicsStyleType.Cut);
                            }
                            else if (e is SymbolicCurve sc)
                            {
                                sc.Subcategory = newSubCat.GetGraphicsStyle(GraphicsStyleType.Cut);
                            }
                            else if (e is CurveByPoints cbp)
                            {
                                cbp.Subcategory = newSubCat.GetGraphicsStyle(GraphicsStyleType.Cut);
                            }
                        }
                    }
                    var projectionId = catToMerge.GetGraphicsStyle(GraphicsStyleType.Projection).Id.IntegerValue;
                    var linesProjection = curves.Where(q => q.LineStyle.Id.IntegerValue == projectionId)
                        .ToList();
                    foreach (var e in elements)
                    {
                        if (e is GenericForm gf)
                        {
                            gf.Subcategory = newSubCat;
                        }
                        else if (e is ModelText mt)
                        {
                            mt.Subcategory = newSubCat;
                        }
                    }
                    foreach (var e in linesProjection)
                    {
                        if (e is ModelCurve mc)
                        {
                            mc.Subcategory = newSubCat.GetGraphicsStyle(GraphicsStyleType.Projection);
                        }
                        else if (e is SymbolicCurve sc)
                        {
                            sc.Subcategory = newSubCat.GetGraphicsStyle(GraphicsStyleType.Projection);
                        }
                        else if (e is CurveByPoints cbp)
                        {
                            cbp.Subcategory = newSubCat.GetGraphicsStyle(GraphicsStyleType.Projection);
                        }
                    }
                    famDoc.Delete(catToMerge.Id);
                }
                t.Commit();
            }
            famDoc.LoadFamily(f.Document, new Utils.OverwriteFamily());
            famDoc.Close(false);
        }
    
    }
}