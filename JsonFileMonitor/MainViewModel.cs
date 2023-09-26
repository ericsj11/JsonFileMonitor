using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using JsonFileMonitor.Abstractions.Models.RingBearers;
using JsonFileMonitor.Abstractions.Services;

namespace JsonFileMonitor
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private RingBearersModel? _ringBearersModel;
        private string? _monitorStatus;
        private readonly IRingBearersFileMonitorService _ringBearersFileMonitorService;
        private readonly IRingBearersDataService _ringBearersDataService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel(IRingBearersFileMonitorService ringBearersFileMonitorService,
            IRingBearersDataService ringBearersDataService)
        {
            _ringBearersFileMonitorService = ringBearersFileMonitorService;
            _ringBearersDataService = ringBearersDataService;

            MonitorStatus = "Green";
            _ringBearersFileMonitorService.JsonFileChanged += MonitorService_JsonFileChanged;
            GetRingBearersModelCommand = new AsyncRelayCommand(GetRingBearersModel);
            StopMonitorServiceCommand = new RelayCommand(StopMonitorService);
        }

        public RingBearersModel? RingBearersModel
        {
            get { return _ringBearersModel; }
            set
            {
                _ringBearersModel = value;

                if (_ringBearersModel?.RingBearers != null)
                {
                    _ringBearersModel.RingBearers = _ringBearersModel.RingBearers.OrderBy(x => x.Order).ToList();
                }

                OnPropertyChanged(nameof(RingBearersModel));
            }
        }

        public string? MonitorStatus
        {
            get { return _monitorStatus; }
            set
            {
                _monitorStatus = value;
                OnPropertyChanged(nameof(MonitorStatus));
            }
        }

        public ICommand GetRingBearersModelCommand { get; }
        public ICommand StopMonitorServiceCommand { get; }

        public async Task GetRingBearersModel()
        {
            RingBearersModel = await _ringBearersDataService.GetJsonFileData();
        }

        private void StopMonitorService()
        {
            _ringBearersFileMonitorService.Cancel();
            MonitorStatus = "Red";
        }

        private void MonitorService_JsonFileChanged(object? sender, RingBearersModel? e)
        {
            RingBearersModel = e;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}