using System;
using System.Threading;
using System.Threading.Tasks;
using JsonFileMonitor.Abstractions.Models.RingBearers;
using JsonFileMonitor.Abstractions.Services;

namespace JsonFileMonitor.Services
{
    public class RingBearersFileMonitorService : IRingBearersFileMonitorService, IAsyncDisposable
    {
        private readonly PeriodicTimer _timer;
        private readonly Task _timerTask;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IRingBearersDataService _ringBearersDataService;
        private readonly IRingBearersFileReaderService _ringBearersFileReaderService;
        private DateTime _ringBearersFileLastWriteDate;
        private bool _initialRun = true;

        public event EventHandler<RingBearersModel?>? JsonFileChanged;

        public RingBearersFileMonitorService(IRingBearersDataService ringBearersDataService,
            IRingBearersFileReaderService ringBearersFileReaderService)
        {
            _ringBearersDataService = ringBearersDataService;
            _ringBearersFileReaderService = ringBearersFileReaderService;

            SaveFileLastWriteDate();
            _cancellationTokenSource = new CancellationTokenSource();
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
            _timerTask = HandleTimerAsync();
        }

        private async Task HandleTimerAsync()
        {
            try
            {
                do
                {
                    await GetData();
                } while (await _timer.WaitForNextTickAsync(_cancellationTokenSource.Token));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in the timer task: {ex.Message}");
                //Handle the exception but don't propagate it.
            }
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }

        public bool HasFileChanged()
        {
            // If last write is the bigger then the saved last write, the file has changed.
            return _ringBearersFileReaderService.GetFileLastWriteDate() > _ringBearersFileLastWriteDate;
        }

        private void SaveFileLastWriteDate()
        {
            _ringBearersFileLastWriteDate = _ringBearersFileReaderService.GetFileLastWriteDate();
        }

        private async ValueTask GetData()
        {
            if (_initialRun || HasFileChanged())
            {
                var ringBearers = await _ringBearersDataService.GetJsonFileData(cancellationToken: _cancellationTokenSource.Token);
                JsonFileChanged?.Invoke(this, ringBearers);
                SaveFileLastWriteDate();
                _initialRun = false;
            }
        }

        public async ValueTask DisposeAsync()
        {
            Cancel();
            _timer.Dispose();
            await _timerTask;
            _cancellationTokenSource.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}