using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using StringSearch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;
using VCExtensibleStorageExtension.ElementExtensions;

namespace BoostYourBIMTerrificTools
{
    class Ribbon : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            _a = application;
            if (Properties.Settings.Default.PrintSuppression != null)
            {
                PrintSuppression.Utils.catIds = new List<ElementId>();
                foreach (string s in Properties.Settings.Default.PrintSuppression)
                {
                    PrintSuppression.Utils.catIds.Add(new ElementId(int.Parse(s)));
                }
                Autodesk.Revit.ApplicationServices.ControlledApplication app = application.ControlledApplication;
                app.DocumentPrinting += PrintSuppression.Utils.Application_DocumentPrinting;
                app.DocumentPrinted += PrintSuppression.Utils.Application_DocumentPrinted;
            }

            string panelName = "Boost Your BIM";
            RibbonPanel panel = application.GetRibbonPanels().FirstOrDefault(q => q.Name == panelName);
            if (panel == null)
                panel = application.CreateRibbonPanel(panelName);

            string dll = typeof(Ribbon).Assembly.Location;
            Utils.dllPath = Path.GetDirectoryName(dll);

            PulldownButton btnBoostYourBIM = panel.AddItem(new PulldownButtonData("BoostYourBIM", "Tools!")) as PulldownButton;
            btnBoostYourBIM.Image = NewBitmapImage(GetType().Namespace, "BoostYourBIMlogo16.png");
            btnBoostYourBIM.LargeImage = NewBitmapImage(GetType().Namespace, "BoostYourBIMlogo32.png");

            PushButton btnCurtainGridFromLine = btnBoostYourBIM.AddPushButton(new PushButtonData("CurtainGrid From Line", "CurtainGrid From Line", dll, "BoostYourBIMTerrificTools.CurtainGridFromLine.CurtainGridFromLine")) as PushButton;
            btnCurtainGridFromLine.ToolTip = "Create curtain grid by selecting a line";

            PushButton btnSubcategoryMerge = btnBoostYourBIM.AddPushButton(new PushButtonData("Subcategory Merge", "Subcategory Merge", dll, "BoostYourBIMTerrificTools.SubcategoryMerge.SubcategoryMerge")) as PushButton;
            btnSubcategoryMerge.ToolTip = "";

            PushButton btnAreaSchemeDuplicate = btnBoostYourBIM.AddPushButton(new PushButtonData("Area Scheme Duplicate", "Area Scheme Duplicate", dll, "BoostYourBIMTerrificTools.AreaSchemeDuplicate.AreaSchemeDuplicate")) as PushButton;
            btnAreaSchemeDuplicate.ToolTip = "Copy an area scheme, area plan, areas, and area tags into a new view";

            PushButton btnroomAreaBoundaries = btnBoostYourBIM.AddPushButton(new PushButtonData("Room/Area Boundaries", "Room/Area Boundaries", dll, "BoostYourBIMTerrificTools.RoomAreaBoundaries.roomAreaBoundaries")) as PushButton;
            btnroomAreaBoundaries.ToolTip = "Create area boundaries from a room, room boundaries from an area line, or boundary lines from a model line";

            PushButton btnSketchConvert = btnBoostYourBIM.AddPushButton(new PushButtonData("Sketch Convert", "Sketch Convert", dll, "BoostYourBIMTerrificTools.SketchConvert.SketchConvert")) as PushButton;
            btnSketchConvert.ToolTip = "Create a floor, ceiling, roof, or model lines from the sketch of a floor, roof, or ceiling.";

            PushButton btnTopo = btnBoostYourBIM.AddPushButton(new PushButtonData("Topo From Lines - Create Topo", "Topo From Lines - Create Topo", dll, "BoostYourBIMTerrificTools.topoFromLines")) as PushButton;
            btnTopo.ToolTip = "Select lines or DWG import to create toposurface.";

            PushButton btnUpdate = btnBoostYourBIM.AddPushButton(new PushButtonData("Topo From Lines - Update", "Topo From Lines - Update", dll, "BoostYourBIMTerrificTools.topoFromLinesUpdate")) as PushButton;
            btnUpdate.ToolTip = "Select a line to update a surface created with Topo From Lines. If this line was previously used to define the surface, the corresponding points will be replaced. If the line was not previously used, its points will be added.";



#if !RELEASE2013 && !RELEASE2014 && !RELEASE2015 && !RELEASE2016
            PushButton btnPrintSupressions = btnBoostYourBIM.AddPushButton(new PushButtonData("btnPrintSupressions", "Print Suppression", dll, "BoostYourBIMTerrificTools.PrintSuppression.Command"));
            btnPrintSupressions.Image = NewBitmapImage(GetType().Namespace, "NoPrint16.png");
            btnPrintSupressions.LargeImage = NewBitmapImage(GetType().Namespace, "NoPrint.png");
#endif

#if !RELEASE2013
            PushButton btnPaintStripper = btnBoostYourBIM.AddPushButton(new PushButtonData("btnPaintStripper", "Paint Stripper", dll, "BoostYourBIMTerrificTools.PaintStripper.Command"));
#endif

            PushButton btnFamilyRename = btnBoostYourBIM.AddPushButton(new PushButtonData("btnFamilyRename", "Family Rename", dll, "BoostYourBIMTerrificTools.FamilyRename.Command"));
            //PushButton btnDockableSample = btnBoostYourBIM.AddPushButton(new PushButtonData("btnDockableSample", "Dockable Sample", dll, "DockableSample.dockableSample"));
            //btnBoostYourBIM.AddPushButton(new PushButtonData("btnModelCompare", "Model Compare", dll, "BoostYourBIMTerrificTools.ModelCompare.ModelCompareCommand"));

            Utils.btnKeyboardShortcutTutor = btnBoostYourBIM.AddPushButton(new PushButtonData("btnKeyboardShortcutTutor", "Enable Keyboard Shortcut Tutor", dll, "BoostYourBIMTerrificTools.KeyboardShortcutTutor.Command"));
            Utils.btnKeyboardShortcutTutor.LargeImage = NewBitmapImage(GetType().Namespace, "KeyboardShortcutTutor32.png");
            KeyboardShortcutTutor.Utils.uIControlledApplication = application;
            if (Properties.Settings.Default.KeyboardShortcutTutor)
            {
                Utils.btnKeyboardShortcutTutor.ItemText = "Disable Keyboard Shortcut Tutor";
                KeyboardShortcutTutor.Utils.enable();
            }

            string commentButtonName = "Enable Pin Comment";
            if (Properties.Settings.Default.PinComment)
            {
                commentButtonName = "Disable Pin Comment";
                application.ControlledApplication.DocumentChanged += PinComment.Utils.ControlledApplication_DocumentChanged;
                // https://forums.autodesk.com/t5/revit-ideas/pin-unpin-custom-comment-warning/idi-p/9403005
            }
            Utils.btnPinComment = btnBoostYourBIM.AddPushButton(new PushButtonData("btnPinComment", commentButtonName, dll, "BoostYourBIMTerrificTools.PinComment.Command"));
            Utils.btnPinComment.LargeImage = NewBitmapImage(GetType().Namespace, "pin.png");

            btnBoostYourBIM.AddPushButton(new PushButtonData("btnViewRangeLines", "Make View Range Lines", dll, "BoostYourBIMTerrificTools.ViewRangeLines"));
            btnBoostYourBIM.AddPushButton(new PushButtonData("btnViewRangeLinesSet", "Set View Range from Lines", dll, "BoostYourBIMTerrificTools.ViewRangeLinesSet"));

            PushButton btnUnits2021FamilyTypeUpgrade = btnBoostYourBIM.AddPushButton(new PushButtonData("btnUnits2021FamilyTypeUpgrade", "Units 2021 Family Type Upgrade", dll, "BoostYourBIMTerrificTools.Units2021FamilyTypeUpgrade"));
            btnUnits2021FamilyTypeUpgrade.AvailabilityClassName = "BoostYourBIMTerrificTools.AvailabilityTrueAlways";

            //PushButton btnPdfNameCleanup = btnBoostYourBIM.AddPushButton(new PushButtonData("btnPdfNameCleanup", "Pdf Name Cleanup", dll, "BoostYourBIMTerrificTools.PdfNameCleanup"));
            //btnPdfNameCleanup.ToolTip = "Remove filename and view type, undo Revit replacing . with -";


            PushButton btnSelectIntersecting = btnBoostYourBIM.AddPushButton(new PushButtonData("Select Intersecting", "Select Intersecting", dll, "BoostYourBIMTerrificTools.SelectIntersecting.selectIntersecting")) as PushButton;
            btnSelectIntersecting.Image = NewBitmapImage(GetType().Namespace, "SelectIntersecting16.png");
            btnSelectIntersecting.LargeImage = NewBitmapImage(GetType().Namespace, "SelectIntersecting32.png");
            btnSelectIntersecting.ToolTip = "Select an element and this command will add to the selection set all intersecting elements.";
            btnSelectIntersecting.SetContextualHelp(new ContextualHelp(ContextualHelpType.Url, "http://boostyourbim.wordpress.com/products/#SelectIntersecting"));


            PushButton btnStringSearch = btnBoostYourBIM.AddPushButton(new PushButtonData("btnStringSearch", "Parameter String Search", dll, "StringSearch.Command"));
            btnStringSearch.Image = NewBitmapImage(GetType().Namespace, "ParameterSearch.png");
            btnStringSearch.LargeImage = NewBitmapImage(GetType().Namespace, "ParameterSearch.png");

//#if !RELEASE2013 && !RELEASE2014
//            PushButton btnDbSpy = btnBoostYourBIM.AddPushButton(new PushButtonData("btnDbSpy", "DB Spy", dll, "BoostYourBIMTerrificTools.DBSpy.DbSpy"));
//#endif

            //PushButton btnCreatedHow = btnBoostYourBIM.AddPushButton(new PushButtonData("btnCreatedHow", "Created How", dll, "BoostYourBIMTerrificTools.CreatedHow.CreatedHow"));
            //application.ControlledApplication.DocumentChanged += CreatedHow.Utils.ControlledApplication_DocumentChanged;
            //CreatedHow.Utils.ExternalEventWhatChanged = new CreatedHow.EventHandlerWithString();
            //CreatedHow.Utils.createdIds = new List<Tuple<string, ElementId>>();

            PushButton btnSelectByType = btnBoostYourBIM.AddPushButton(new PushButtonData("btnSelectByType", "Select By Type", dll, "BoostYourBIMTerrificTools.SelectByType.SelectByType"));

            PushButton btnDisplaceByLevel = btnBoostYourBIM.AddPushButton(new PushButtonData("Level Displacer", "Level Displacer", dll, "LevelDisplacer.levelDisplacer"));
            btnDisplaceByLevel.Image = NewBitmapImage(GetType().Namespace, "LevelDisplacer16.png");
            btnDisplaceByLevel.LargeImage = NewBitmapImage(GetType().Namespace, "LevelDisplacer32.png");
            btnDisplaceByLevel.ToolTip = "Create per-level displacement sets and displace each set by incremented X, Y, and Z values.";
            btnDisplaceByLevel.SetContextualHelp(new ContextualHelp(ContextualHelpType.Url, "http://boostyourbim.wordpress.com/products/"));

            PushButton btnLevelGenerator = btnBoostYourBIM.AddPushButton(new PushButtonData("btnLevelGenerator", "Level Generator", dll, "LevelGenerator.LevelGenerator"));
            btnLevelGenerator.Image = NewBitmapImage(GetType().Namespace, "LevelGenerator2_16.png");
            btnLevelGenerator.LargeImage = NewBitmapImage(GetType().Namespace, "LevelGenerator2_32.png");
            btnLevelGenerator.ToolTip = "Create or rename levels with options for prefix, suffix, and offset.";
            btnLevelGenerator.SetContextualHelp(new ContextualHelp(ContextualHelpType.Url, "http://boostyourbim.wordpress.com/products/"));

            PushButton btnWallOpeningArea = btnBoostYourBIM.AddPushButton(new PushButtonData("ADNP_WALL_OPENING_AREA", "Wall Opening Area", dll, "WallOpeningArea.Command"));
            btnWallOpeningArea.LargeImage = NewBitmapImage(GetType().Namespace, "WallOpeningArea32.png");
            btnWallOpeningArea.Image = NewBitmapImage(GetType().Namespace, "WallOpeningArea32.png");
            btnWallOpeningArea.LongDescription = "Measure area created by Opening elements, wall profile editing, and inserts (such as windows). Data is stored in the parameters:\n" + WallOpeningArea.SharedParameterFunctions.PARAMETER_SMALL_OPEN_NAME + "\n" + WallOpeningArea.SharedParameterFunctions.PARAMETER_TOTAL_OPEN_NAME;
            btnWallOpeningArea.ToolTip = "Measure wall opening area.";
            btnWallOpeningArea.SetContextualHelp(new ContextualHelp(ContextualHelpType.Url, "https://boostyourbim.wordpress.com/products/#WallOpening"));

            PushButton button = btnBoostYourBIM.AddPushButton(new PushButtonData("ImageOMatic", "Image-O-Matic", dll, "ImageOMatic.ImageOMatic")) as PushButton;
            button.ToolTip = "Generate images from phases or parameter values. Select family instance or ESC for phases only.";
            button.LongDescription = "Select a family instance to modify any of its instance parameters. Push ESC to get a list of phases that will be set for the active view.";
            button.Image = NewBitmapImage(GetType().Namespace, "ImageOMatic16.png");
            button.LargeImage = NewBitmapImage(GetType().Namespace, "ImageOMatic32.png");
            button.ToolTipImage = NewBitmapImage(GetType().Namespace, "ImageOMaticTooltip.png");

            btnBoostYourBIM.AddSeparator();

            PushButton btnLearn = btnBoostYourBIM.AddPushButton(new PushButtonData("btnLearn", "You can learn the Revit API", dll, "BoostYourBIMTerrificTools.Learn"));
            btnLearn.ToolTip = "You can do more than just use terrific tools like these - you can build your own tools too!";

            PushButton btnSupport = btnBoostYourBIM.AddPushButton(new PushButtonData("btnSupport", "Support Boost Your BIM", dll, "BoostYourBIMTerrificTools.Support"));
            btnSupport.ToolTip = "Your generous support makes it possible for Boost Your BIM to provide these terrific free tools";

            PushButton btnContact = btnBoostYourBIM.AddPushButton(new PushButtonData("btnContact", "Contact Boost Your BIM", dll, "BoostYourBIMTerrificTools.Contact"));
            btnSupport.ToolTip = "Boost Your BIM provides custom development services to help your BIM be even better";

            application.ViewActivated += Application_ViewActivated;

            IssueList.RevisionWatcher revisionWatcher = new IssueList.RevisionWatcher(application.ActiveAddInId);
            UpdaterRegistry.RegisterUpdater(revisionWatcher);
            ElementFilter revisionElementFilter = new ElementCategoryFilter(BuiltInCategory.OST_RevisionClouds);
            UpdaterRegistry.AddTrigger(revisionWatcher.GetUpdaterId(), revisionElementFilter, Element.GetChangeTypeAny());
            UpdaterRegistry.AddTrigger(revisionWatcher.GetUpdaterId(), revisionElementFilter, Element.GetChangeTypeElementDeletion());
            UpdaterRegistry.AddTrigger(revisionWatcher.GetUpdaterId(), revisionElementFilter, Element.GetChangeTypeElementAddition());
            application.Idling += IssueList.Utils.Application_Idling;

            //Utils.ModelComparePaneId = new DockablePaneId(new Guid("{189CBD31-69F6-485F-A8D0-5A3B61E2EC26}"));
            //application.RegisterDockablePane(Utils.ModelComparePaneId, "Model Compare", new ModelCompare. as IDockablePaneProvider);

//#if !RELEASE2013
//            Utils.dockableSamplePaneId = new DockablePaneId(new Guid("{489CBD31-69F6-485F-A8D0-5A3B61E2EC26}"));
//            application.RegisterDockablePane(Utils.dockableSamplePaneId, "Dockable Pane", new DockableSample.MainPageDockableDialog() as IDockablePaneProvider);
//#endif
//            application.ControlledApplication.DocumentOpened += ControlledApplication_DocumentOpened;
//            application.ControlledApplication.DocumentCreated += ControlledApplication_DocumentCreated;

//            application.ControlledApplication.DocumentPrinted += ControlledApplication_DocumentPrinted;
//            Utils.PrintedSheetNumbers = new List<string>();

//#if !RELEASE2013 && !RELEASE2014
//            DBSpy.Utils.XYZEventHandler = new DBSpy.EventHandlerXYZ();
//            DBSpy.Utils.LineEventHandler = new DBSpy.EventHandlerLine();
//            DBSpy.Utils.SolidEventHandler = new DBSpy.EventHandlerSolid();
//            DBSpy.Utils.dockableSamplePaneId = new DockablePaneId(new Guid("{419CBD32-69F5-175F-A8D1-5A3B61E5EC29}"));
//            application.RegisterDockablePane(DBSpy.Utils.dockableSamplePaneId, "DB Spy", new DBSpy.MainPageDockableDialog());
//#endif

            //PresentationTraceSources.Refresh();
            //PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            //PresentationTraceSources.DataBindingSource.Listeners.Add(new DebugTraceListener());
            //PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning | SourceLevels.Error;

            //Utils.ViewScaleUpdater viewScaleUpdater = new Utils.ViewScaleUpdater(application.ActiveAddInId);
            //UpdaterRegistry.RegisterUpdater(viewScaleUpdater, true);
            //UpdaterRegistry.AddTrigger(
            //    viewScaleUpdater.GetUpdaterId(), 
            //    new ElementClassFilter(typeof(View)), 
            //    Element.GetChangeTypeParameter(new ElementId(BuiltInParameter.VIEW_SCALE)));

            //Utils.illegalViewRangeFailureId = new FailureDefinitionId(Guid.NewGuid());
            //FailureDefinition.CreateFailureDefinition(
            //    Utils.illegalViewRangeFailureId, 
            //    FailureSeverity.Error,
            //    "This view scale is not allowed.");

            return Result.Succeeded;
        }



        public class DebugTraceListener : TraceListener
        {
            public override void Write(string message)
            {
            }

            public override void WriteLine(string message)
            {
                Debugger.Break();
            }
        }

        private void ControlledApplication_DocumentPrinted(object sender, Autodesk.Revit.DB.Events.DocumentPrintedEventArgs e)
        {
            foreach (View v in e.GetPrintedViewElementIds().Select(q => e.Document.GetElement(q) as View))
            {
                if (v is ViewSheet vs)
                {
                    Utils.PrintedSheetNumbers.Add(vs.SheetNumber);
                }
            }
        }

        private void ControlledApplication_DocumentCreated(object sender, Autodesk.Revit.DB.Events.DocumentCreatedEventArgs e)
        {
#if !RELEASE2013
            closeUI(new UIApplication(sender as Application));
#endif
        }

        private void ControlledApplication_DocumentOpened(object sender, Autodesk.Revit.DB.Events.DocumentOpenedEventArgs e)
        {
#if !RELEASE2013
            closeUI(new UIApplication(sender as Application));
#endif
        }

#if !RELEASE2013
        private void closeUI(UIApplication uiapp)
        {
            uiapp.GetDockablePane(Utils.dockableSamplePaneId).Hide();
        }
#endif

        public static BitmapImage NewBitmapImage(string ns, string imageName)
        {
            string imagePath = ns + ".ImageFiles." + imageName;
            Assembly ass = Assembly.GetExecutingAssembly();
            Stream s = ass.GetManifestResourceStream(imagePath);
            if (s == null)
            {
                return null;
            }
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = s;
            img.EndInit();
            return img;
        }

        private void Application_ViewActivated(object sender, Autodesk.Revit.UI.Events.ViewActivatedEventArgs e)
        {
            Utils.doc = e.Document;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            SearchHitNavigator.Shutdown();

            Unsubscribe();

            return Result.Succeeded;
        }
        static JtWindowHandle _hWndRevit = null;
        static int _pending_element_id;
        static bool _subscribing = false;
        static UIControlledApplication _a;

        static void Subscribe()
        {
            if (!_subscribing)
            {
                _a.Idling
                  += new EventHandler<IdlingEventArgs>(
                    OnIdling);

                _subscribing = true;
            }
        }

        static void Unsubscribe()
        {
            if (_subscribing)
            {
                _a.Idling -= new EventHandler<IdlingEventArgs>(OnIdling);

                _subscribing = false;
            }
        }
        public delegate void SetElementId(int id);
        public static void ShowForm(
  SortableBindingList<SearchHit> data)
        {
            SearchHitNavigator.Show(data,
              new SetElementId(SetPendingElementId),
              _hWndRevit);

            Subscribe();
        }

        static void SetPendingElementId(int id)
        {
            _pending_element_id = id;
        }

        static void OnIdling(
  object sender,
  IdlingEventArgs ea)
        {
            if (!SearchHitNavigator.IsShowing)
            {
                Unsubscribe();
            }

            int id = _pending_element_id;

            if (0 != id)
            {
                // Support both 2011, where sender is an 
                // Application instance, and 2012, where 
                // it is a UIApplication instance:

                UIApplication uiapp
                  = sender is UIApplication
                  ? sender as UIApplication // 2012
                  : new UIApplication(
                      sender as Application); // 2011

                UIDocument uidoc
                  = uiapp.ActiveUIDocument;

                Document doc
                  = uidoc.Document;

                ElementId eid = new ElementId(id);
                Element e = doc.GetElement(eid);
                if (e != null)
                {
                    Debug.Print(
                      "Element id {0} requested --> {1}",
                      id, new ElementData(e));
#if RELEASE2013 || RELEASE2014
                    SelElementSet selSet = SelElementSet.Create();
                    selSet.Add(e);
                    uidoc.Selection.Elements = selSet;
#else
                    uidoc.Selection.SetElementIds(new List<ElementId> { e.Id });
#endif
                    uidoc.RefreshActiveView();
                }

                _pending_element_id = 0;
            }
        }

    }



    public class AvailabilityTrueAlways : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication a, CategorySet b)
        {
            return true;
        }
    }

}