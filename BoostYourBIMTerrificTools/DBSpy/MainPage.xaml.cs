using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace BoostYourBIMTerrificTools.DBSpy
{
    public partial class MainPageDockableDialog : IDockablePaneProvider
    {
        public MainPageDockableDialog()
        {
            InitializeComponent();
            DBSpy.Utils.tree = modelessTree;           
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this;
            data.InitialState = new DockablePaneState
            {
                DockPosition = DockPosition.Tabbed,
            };
        }

        private void modelessTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is XYZViewModel xYZViewModel)
            {
                raisePoint(xYZViewModel.XYZ);
            }
            else if (e.NewValue is XYZNamedViewModel xYZNamedViewModel)
            {
               raisePoint(xYZNamedViewModel.XYZ);
            }
            else if (e.NewValue is LineViewModel lineViewModel)
            {
                Utils.LineEventHandler.Raise(lineViewModel._line);
            }
            else if (e.NewValue is SolidViewModel solidViewModel)
            {
                Utils.SolidEventHandler.Raise(solidViewModel._solid);
            }
            else
            {
                return;
            }
        }

        private void raisePoint(XYZ pt)
        {
            if (GetElementAtPoint(pt) == null)
                Utils.XYZEventHandler.Raise(pt);

            if (GetElementAtPoint(pt) != null)
            {
                new UIDocument(BoostYourBIMTerrificTools.Utils.doc).Selection.SetElementIds(new List<ElementId> { GetElementAtPoint(pt).Id });
            }
        }

        private void TreeViewItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs args)
        {
            Document doc = BoostYourBIMTerrificTools.Utils.doc;
            if (args.Source is System.Windows.Controls.TreeViewItem tvi)
            {
                XYZ pt = null;
                if (tvi.DataContext is XYZViewModel xYZViewModel)
                {
                    pt = xYZViewModel.XYZ;
                }
                else if (tvi.DataContext is XYZNamedViewModel xYZNamedViewModel)
                {
                    pt = xYZNamedViewModel.XYZ;
                }
                else
                {
                    return;
                }

                Element e = GetElementAtPoint(pt);
                if (e == null)
                    return;
                UIView view = new UIDocument(BoostYourBIMTerrificTools.Utils.doc).GetOpenUIViews().FirstOrDefault(q => q.ViewId == doc.ActiveView.Id);
                BoundingBoxXYZ box = e.get_BoundingBox(doc.ActiveView);
                view.ZoomAndCenterRectangle(box.Min, box.Max);
                view.Zoom(0.1);
            }

        }

        private Element GetElementAtPoint(XYZ pt)
        {
            Document doc = BoostYourBIMTerrificTools.Utils.doc;
            Element e = new FilteredElementCollector(doc)
                    .OfClass(typeof(DirectShape))
                    .OfCategory(BuiltInCategory.OST_GenericModel)
                    .FirstOrDefault(q =>
                        q.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).AsString().Contains(pt.X.ToString()) &&
                        q.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).AsString().Contains(pt.Y.ToString()) &&
                        q.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).AsString().Contains(pt.Z.ToString())
                    );
            return e;
        }

        private void cmSelectAll_Click(object sender, RoutedEventArgs args)
        {
            if (Utils.tree.SelectedItem is ElementViewModel evm)
            {
                Document doc = BoostYourBIMTerrificTools.Utils.doc;
                Element e = doc.GetElement(evm.Id);
                if (e is ElementType et)
                {
                    List<ElementId> ids = new FilteredElementCollector(doc)
                        .WhereElementIsNotElementType()
                        .Where(q => q.GetTypeId() == et.Id)
                        .Select(q => q.Id).ToList();
                    if (ids.Any())
                    {
                        new UIDocument(doc).Selection.SetElementIds(ids);
                    }
                }
                
            }
        }

        private void TextBlock_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs args)
        {
            if (Utils.tree.SelectedItem is ElementViewModel evm)
            {
                Document doc = BoostYourBIMTerrificTools.Utils.doc;
                Element e = doc.GetElement(evm.Id);
                UIDocument uidoc = new UIDocument(doc);
                if (e is ElementType)
                {
                    uidoc.Selection.SetElementIds(new List<ElementId> { });
                }
                else
                {
                    uidoc.Selection.SetElementIds(new List<ElementId> { evm.Id });
                }
            }
        }

        private void show_Click(object sender, RoutedEventArgs args)
        {
            if (Utils.tree.SelectedItem is ElementViewModel evm)
            {
                Document doc = BoostYourBIMTerrificTools.Utils.doc;
                UIDocument uidoc = new UIDocument(doc);
                Element e = doc.GetElement(evm.Id);
                if (!(e is ElementType))
                {
                    uidoc.ShowElements(e);
                }

            }
        }
    }

}