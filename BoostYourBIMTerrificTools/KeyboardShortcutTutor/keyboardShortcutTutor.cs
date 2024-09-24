using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Windows;
using Autodesk.Revit.UI.Events;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

namespace BoostYourBIMTerrificTools.KeyboardShortcutTutor
{

    [Transaction(TransactionMode.ReadOnly)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            if (BoostYourBIMTerrificTools.Utils.btnKeyboardShortcutTutor.ItemText == "Enable Keyboard Shortcut Tutor")
            {
                Utils.enable();
            }
            else
            {
                ComponentManager.UIElementActivated -= new EventHandler<UIElementActivatedEventArgs>(Utils.uIElementActivated);
                commandData.Application.Idling -= new EventHandler<IdlingEventArgs>(Utils.idleUpdate);
                Properties.Settings.Default.KeyboardShortcutTutor = false;
                BoostYourBIMTerrificTools.Utils.btnKeyboardShortcutTutor.ItemText = "Enable Keyboard Shortcut Tutor";
            }

            Properties.Settings.Default.Save();
            return Result.Succeeded;
        }
    }

    public class WindowHandle : IWin32Window
    {
        IntPtr _hwnd;
        public WindowHandle(IntPtr h)
        {
            _hwnd = h;
        }
        public IntPtr Handle
        { get { return _hwnd; } }
    }

    public static class Utils
    {
        public static FrmHint formHint = null;
        public static IList<string> whitelist = new List<string>();
        public static WindowHandle windowHandle = null;
        public static Dictionary<string, Utils.Pair<string, string>> dict = new Dictionary<string, Utils.Pair<string, string>>();
        public static UIControlledApplication uIControlledApplication = null;

        public static void uIElementActivated(object sender, UIElementActivatedEventArgs e)
        {
            Utils.uIControlledApplication.ControlledApplication.WriteJournalComment("KeyboardShortcutTutor - entered uIElementActivated", true);

            if (e.UiElement == null)
            {
                Utils.uIControlledApplication.ControlledApplication.WriteJournalComment("KeyboardShortcutTutor - e.UiElement == null", true);
                return;
            }

            if (e.UiElement.GetType() == null)
            {
                Utils.uIControlledApplication.ControlledApplication.WriteJournalComment("KeyboardShortcutTutor - e.UiElement.GetType() == null", true);
                return;
            }

            if (e.UiElement.GetType().ToString() == "Autodesk.Private.Windows.MenuToggleButton")
            {
                Utils.uIControlledApplication.ControlledApplication.WriteJournalComment("KeyboardShortcutTutor - e.UiElement.GetType().ToString() == Autodesk.Private.Windows.MenuToggleButton", true);
                return;
            }

            if (e.Item == null)
            {
                Utils.uIControlledApplication.ControlledApplication.WriteJournalComment("KeyboardShortcutTutor - e.Item == null", true);
                return;
            }

            string id = e.Item.Id;
            if (id == null)
                return;

            Utils.uIControlledApplication.ControlledApplication.WriteJournalComment("KeyboardShortcutTutor - id = " + id, true);

            if (Utils.windowHandle == null)
            {
                Utils.uIControlledApplication.ControlledApplication.WriteJournalComment("KeyboardShortcutTutor - window handle was null", true);
#if RELEASE2016 || RELEASE2017 || RELEASE2018
                Process p = Process.GetCurrentProcess();
                windowHandle = new WindowHandle(p.MainWindowHandle);
#else
                Utils.windowHandle = new WindowHandle(Utils.uIControlledApplication.MainWindowHandle);
#endif
            }

            id = id.Replace("_RibbonListButton", "");
            id = id.Replace("SketchGalleryItem_", "");

            if (Utils.loadShortcuts())
            {
                Utils.uIControlledApplication.ControlledApplication.WriteJournalComment("KeyboardShortcutTutor - shortcuts loaded", true);
                Utils.Pair<string, string> outData = new Utils.Pair<string, string>();
                if (Utils.dict.TryGetValue(id, out outData))
                {
                    RECT windowRect = new RECT();
                    System.Drawing.Rectangle screenRect = Screen.GetBounds(new System.Drawing.Point(0, 0));
                    GetWindowRect(Utils.windowHandle.Handle, ref windowRect);
                    int x = windowRect.Right - 10;
                    if (x > screenRect.Right)
                        x = screenRect.Right;
                    int y = windowRect.Bottom - 15; // offset to move it above the status bar
                    if (y > screenRect.Bottom)
                        y = screenRect.Bottom;

                    Utils.uIControlledApplication.ControlledApplication.WriteJournalComment("KeyboardShortcutTutor - about to show form", true);
                    Utils.formHint = new FrmHint(Utils.dict, outData.First, outData.Second.Replace("&#xA;", " "), x, y);
                    if (Utils.whitelist.Contains(id))
                    {
                        Utils.formHint.Show(Utils.windowHandle);
                        Utils.SetForegroundWindow(Utils.windowHandle.Handle);
                        Utils.formHint = null;
                    }
                }
            }
        }

        // http://msdn.microsoft.com/en-us/library/ms633519(VS.85).aspx
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        // http://msdn.microsoft.com/en-us/library/a5ch4fda(VS.80).aspx
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }



        public static void enable()
        {
#if RELEASE2016 || RELEASE2017 || RELEASE2018
            Process p = Process.GetCurrentProcess();
            windowHandle = new WindowHandle(p.MainWindowHandle);
#else
            windowHandle = new WindowHandle(Utils.uIControlledApplication.MainWindowHandle);
#endif

            ComponentManager.UIElementActivated += new EventHandler<UIElementActivatedEventArgs>(uIElementActivated);
            buildWhiteList();
            Utils.uIControlledApplication.Idling += new EventHandler<IdlingEventArgs>(idleUpdate);
            Properties.Settings.Default.KeyboardShortcutTutor = true;
            BoostYourBIMTerrificTools.Utils.btnKeyboardShortcutTutor.ItemText = "Disable Keyboard Shortcut Tutor";
        }


        // http://social.msdn.microsoft.com/forums/en-US/csharpgeneral/thread/42b3db75-e61e-4f59-bf2b-c96a40cfb4e4
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static bool loadShortcuts()
        {
            dict.Clear();
            string versionName = uIControlledApplication.ControlledApplication.VersionName;
            string keyboardShortcutsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Autodesk\revit\" + versionName + @"\KeyboardShortcuts.xml");

            if (!File.Exists(keyboardShortcutsFile))
            {
                Autodesk.Revit.UI.TaskDialog td = new Autodesk.Revit.UI.TaskDialog("File Not Found")
                {
                    MainInstruction = "Keyboard shortcut file not found. Use the Revit UI to export it to:",
                    MainContent = keyboardShortcutsFile
                };
                td.Show();
                return false;
            }

            using (StreamReader reader = new StreamReader(keyboardShortcutsFile))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("\" Shortcuts=\""))
                    {
                        string[] words = line.Split('"');
                        dict.Add(words[3], new Pair<string, string>(words[5], words[1]));
                    }
                }
            }
            return true;
        }


        public class Pair<T, U>
        {
            public Pair() { }
            public Pair(T first, U second)
            {
                First = first;
                Second = second;
            }
            public T First { get; set; }
            public U Second { get; set; }
        }

        public static void buildWhiteList()
        {
            whitelist.Add("ID_OBJECTS_WALL");
            whitelist.Add("ID_OBJECTS_STRUCTURAL_WALL");
            whitelist.Add("ID_WALL_PICK_FACES");
            whitelist.Add("ID_OBJECTS_REVEAL");
            whitelist.Add("ID_OBJECTS_CORNICE");
            whitelist.Add("ID_ROOF_PICK_FACES");
            whitelist.Add("ID_OBJECTS_CEILING");
            whitelist.Add("ID_CREATE_GUTTER_TB");
            whitelist.Add("ID_CURTA_SYSTEM_PICK_FACES");
            whitelist.Add("ID_OBJECTS_FLOOR");
            whitelist.Add("ID_OBJECTS_SLAB");
            whitelist.Add("ID_FLOOR_PICK_FACES");
            whitelist.Add("ID_CREATE_SLAB_EDGE_TB");
            whitelist.Add("ID_OBJECTS_CW_GRID");
            whitelist.Add("ID_OBJECTS_MULLION");
            whitelist.Add("ID_OBJECTS_RAILING");
            whitelist.Add("ID_OBJECTS_RAMP");
            whitelist.Add("ID_OBJECTS_STAIRS");
            whitelist.Add("ID_OBJECTS_STAIRS_LEGACY");

            whitelist.Add("ID_OBJECTS_AREA_SEPARATION");
            whitelist.Add("ID_OBJECTS_AREASCHEME_BOUNDARY");
            whitelist.Add("ID_CREATE_OPENING_BY_FACE_TB");
            whitelist.Add("ID_CREATE_SHAFT_OPENING_TB");
            whitelist.Add("ID_CREATE_WALL_OPENING_TB");
            whitelist.Add("ID_CREATE_VERTICAL_OPENING_TB");
            whitelist.Add("ID_CREATE_DORMER_OPENING_TB");
            whitelist.Add("ID_OBJECTS_GRID");
            whitelist.Add("ID_OBJECTS_CLINE");

            //Structure
            whitelist.Add("ID_OBJECTS_JOIST_SYSTEM");
            whitelist.Add("ID_WORKPLANE_VIEW");
            whitelist.Add("ID_OBJECTS_CONTINUOUS_FOOTING");
            whitelist.Add("ID_OBJECTS_FOOTING_SLAB");

            whitelist.Add("ID_OBJECTS_LEVEL");

            whitelist.Add("ID_ANNOTATIONS_DIMENSION_ALIGNED");
            whitelist.Add("ID_ANNOTATIONS_DIMENSION_LINEAR");
            whitelist.Add("ID_ANNOTATIONS_DIMENSION_ANGULAR");
            whitelist.Add("ID_ANNOTATIONS_DIMENSION_RADIAL");
            whitelist.Add("ID_ANNOTATIONS_DIMENSION_DIAMETER");
            whitelist.Add("ID_ANNOTATIONS_DIMENSION_ARCLENGTH");
            whitelist.Add("ID_SPOT_ELEVATION");
            whitelist.Add("ID_SPOT_SLOPE");

            whitelist.Add("ID_OBJECTS_DETAIL_CURVES");
            whitelist.Add("ID_OBJECTS_FILLED_REGION");
            whitelist.Add("ID_MASKING_REGION");
            whitelist.Add("ID_OBJECTS_REPEATING_DETAIL");
            whitelist.Add("ID_OBJECTS_CLOUD");
            whitelist.Add("ID_OBJECTS_INSULATION");
            whitelist.Add("ID_OBJECTS_TEXT_NOTE");
            whitelist.Add("ID_BUTTON_TAG");
            whitelist.Add("ID_NEW_REFERENCE_VIEWER");
            whitelist.Add("ID_STAIRS_TRISER_NUMBER");
            whitelist.Add("ID_OBJECTS_ROOM_FILL");
            whitelist.Add("ID_OBJECTS_STAIRS_PATH");

            // SITE
            whitelist.Add("ID_SHOW_MASS_STANDARD");
            whitelist.Add("ID_SHOW_MASS_NO_OVERRIDE");
            whitelist.Add("ID_SITE_TOPO_SURFACE");
            whitelist.Add("ID_SITE_LABEL_CONTOURS");
            whitelist.Add("ID_SPLIT_SURFACE");
            whitelist.Add("ID_SITE_AREA");
            whitelist.Add("ID_SITE_BUILDINGPAD");

            // VIEW
            whitelist.Add("ID_THIN_LINES");
            whitelist.Add("ID_HIDE_ELEMENTS_EDITOR");
            whitelist.Add("ID_UNHIDE_ELEMENTS_EDITOR");
            whitelist.Add("ID_EDIT_CUT_BOUNDARY");
            whitelist.Add("ID_VIEW_DEFAULT_3DVIEW");
            whitelist.Add("ID_VIEW_NEW_3DVIEW");
            whitelist.Add("ID_VIEW_NEW_WALKTHROUGH");
            whitelist.Add("ID_PRJBROWSER_COPY");
            whitelist.Add("ID_DUPLICATE_WITH_DETAILING");
            whitelist.Add("ID_CREATE_DEPENDENT_VIEW");
            whitelist.Add("ID_WINDOW_CLOSE_HIDDEN");
            whitelist.Add("ID_WINDOW_NEW");

            // MODIFY
            whitelist.Add("ID_EDIT_CUT");
            whitelist.Add("ID_EDIT_PASTE");
            whitelist.Add("ID_EDIT_PASTE_ALIGNED");
            whitelist.Add("ID_EDIT_PASTE_ALIGNED_SAME_PLACE");
            whitelist.Add("ID_COPING");
            whitelist.Add("ID_UNCOPING");
            whitelist.Add("ID_CUT_HOST");
            whitelist.Add("ID_UNCUT_HOST");
            whitelist.Add("ID_JOIN_ELEMENTS_EDITOR");
            whitelist.Add("ID_UNJOIN_ELEMENTS_EDITOR");
            whitelist.Add("ID_SPLIT_FACE");
            whitelist.Add("ID_EDIT_BEAM_JOINS");
            whitelist.Add("ID_EDIT_WALL_JOINS");
            whitelist.Add("ID_EDIT_UNPAINT");
            whitelist.Add("ID_EDIT_DEMOLISH");
            whitelist.Add("ID_EDIT_MOVE");
            whitelist.Add("ID_ALIGN");
            whitelist.Add("ID_OFFSET");
            whitelist.Add("ID_EDIT_MOVE_COPY");
            whitelist.Add("ID_EDIT_MIRROR");
            whitelist.Add("ID_EDIT_ROTATE");
            whitelist.Add("ID_EDIT_MIRROR_LINE");
            whitelist.Add("ID_TRIM_EXTEND_CORNER");
            whitelist.Add("ID_SPLIT");
            whitelist.Add("ID_SPLIT_WITH_GAP");
            whitelist.Add("ID_EDIT_CREATE_PATTERN");
            whitelist.Add("ID_EDIT_SCALE");
            whitelist.Add("ID_TRIM_EXTEND_SINGLE");
            whitelist.Add("ID_TRIM_EXTEND_MULTIPLE");
            whitelist.Add("ID_UNLOCK_ELEMENTS");
            whitelist.Add("ID_LOCK_ELEMENTS");
            whitelist.Add("ID_BUTTON_DELETE");
            whitelist.Add("ID_EDIT_LINEWORK");
            whitelist.Add("ID_VIEW_HIDE_ELEMENTS");
            whitelist.Add("ID_VIEW_HIDE_CATEGORY");
            whitelist.Add("ID_MEASURE_LINE");
            whitelist.Add("ID_MEASURE_PICK_LINES");
            whitelist.Add("ID_CREATE_PARTS");
            whitelist.Add("ID_CREATE_ASSEMBLY");
            whitelist.Add("ID_ADD_ELEMENT_TO_ASSEMBLY");
            whitelist.Add("ID_REMOVE_ELEMENT_FROM_ASSEMBLY");

            // SKETCH
            whitelist.Add("ID_OBJECTS_CURVE_RECT");
            whitelist.Add("ID_OBJECTS_CURVE_LINE");
            whitelist.Add("ID_OBJECTS_CURVE_POLY_INSCRIBED");
            whitelist.Add("ID_OBJECTS_CURVE_POLY_CIRCUMSCRIBED");
            whitelist.Add("ID_OBJECTS_CURVE_CIRCLE");
            whitelist.Add("ID_OBJECTS_CURVE_ARC_THREE_PNT");
            whitelist.Add("ID_OBJECTS_CURVE_ARC_CENTER_ENDS");
            whitelist.Add("ID_OBJECTS_CURVE_ARC_FILLET");
            whitelist.Add("ID_OBJECTS_CURVE_SPLINE");
            whitelist.Add("ID_SKETCH_PICK_WALLS");
            whitelist.Add("ID_OBJECTS_CURVE_PICK_LINES");
            whitelist.Add("ID_OBJECTS_CURVE_ELLIPSE_PARTIAL");
            whitelist.Add("ID_OBJECTS_CURVE_ELLIPSE");
            whitelist.Add("ID_SKETCH_DEFINE_SLOPE");
            whitelist.Add("ID_FLOOR_PICK_DISPAN_EDGE");

            // FAMILY EDITOR
            whitelist.Add("ID_OBJECTS_EXTRUSION");
            whitelist.Add("ID_OBJECTS_BLEND");
            whitelist.Add("ID_OBJECTS_REVOLUTION");
            whitelist.Add("ID_OBJECTS_SWEEP");
            whitelist.Add("ID_SKETCH_2D_PATH");
            whitelist.Add("ID_PICK_PATH");
            whitelist.Add("ID_PICK_EDGES");
            whitelist.Add("ID_OBJECTS_SWEPTBLEND");
            whitelist.Add("ID_OBJECTS_EXTRUSION_CUT");
            whitelist.Add("ID_OBJECTS_BLEND_CUT");
            whitelist.Add("ID_OBJECTS_REVOLUTION_CUT");
            whitelist.Add("ID_OBJECTS_SWEEP_CUT");
            whitelist.Add("ID_OBJECTS_SWEPTBLEND_CUT");

            whitelist.Add("ID_WALL_RECT");
            whitelist.Add("ID_WALL_POLY_INSCRIBED");
            whitelist.Add("ID_WALL_POLY_CIRCUMSCRIBED");
            whitelist.Add("ID_WALL_CIRCLE");
            whitelist.Add("ID_WALL_PICK_LINES");
            whitelist.Add("ID_WALL_ARC_FILLET");
            whitelist.Add("ID_WALL_ARC_TAN");
            whitelist.Add("ID_WALL_ARC_CENTER_ENDS");
            whitelist.Add("ID_WALL_ARC_THREE_PNT");
        }

        public static void idleUpdate(object sender, IdlingEventArgs e)
        {
            if (formHint != null)
            {
                formHint.Show(windowHandle);
                Utils.SetForegroundWindow(windowHandle.Handle);
                formHint = null;
            }
        }

    }

}
