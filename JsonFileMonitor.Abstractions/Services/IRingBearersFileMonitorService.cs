using JsonFileMonitor.Abstractions.Models.RingBearers;

namespace JsonFileMonitor.Abstractions.Services
{
    public interface IRingBearersFileMonitorService
    {
        event EventHandler<RingBearersModel?>? JsonFileChanged;
        bool HasFileChanged();
        void Cancel();
        ValueTask DisposeAsync();
    }
}