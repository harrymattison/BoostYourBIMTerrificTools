using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Windows;

namespace ModelCompare
{
#if !RELEASE2013
    public partial class MainPageDockableDialog : IDockablePaneProvider
    {
        //private readonly APIExternalEventHandler eventHandler = new APIExternalEventHandler();
        //private ExternalEvent externalEvent;
        public MainPageDockableDialog()
        {
            InitializeComponent();
           // externalEvent = ExternalEvent.Create(eventHandler);
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this as FrameworkElement;
            data.InitialState = new DockablePaneState
            {
                DockPosition = DockPosition.Tabbed,
            };
        }

        //private void btnPushME_Click(object sender, RoutedEventArgs e)
        //{
        //    externalEvent.Raise();
        //}
    }
#endif

}
