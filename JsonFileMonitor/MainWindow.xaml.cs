using System.ComponentModel;
using System.Windows;
using JsonFileMonitor.Abstractions.Services;

namespace JsonFileMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IRingBearersFileMonitorService _ringBearersFileMonitorService;

        public MainWindow(IRingBearersFileMonitorService ringBearersFileMonitorService)
        {
            _ringBearersFileMonitorService = ringBearersFileMonitorService;

            InitializeComponent();
        }

        protected override async void OnClosing(CancelEventArgs e)
        {
            await _ringBearersFileMonitorService.DisposeAsync();

            base.OnClosing(e);
        }
    }
}
