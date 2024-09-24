using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Windows;
using DockableSample;

namespace DockableSample
{
#if !RELEASE2013
    public partial class MainPageDockableDialog : IDockablePaneProvider
    {
        private readonly APIExternalEventHandler eventHandler = new APIExternalEventHandler();
        private ExternalEvent externalEvent;
        public MainPageDockableDialog()
        {
            InitializeComponent();
            externalEvent = ExternalEvent.Create(eventHandler);
            btnPushME.Click += btnPushME_Click;
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this as FrameworkElement;
            data.InitialState = new DockablePaneState
            {
                DockPosition = DockPosition.Tabbed,
            };
        }

        private void btnPushME_Click(object sender, RoutedEventArgs e)
        {
            externalEvent.Raise();
        }
    }
#endif
}
