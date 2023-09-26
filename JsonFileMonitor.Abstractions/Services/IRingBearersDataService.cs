using JsonFileMonitor.Abstractions.Models.RingBearers;

namespace JsonFileMonitor.Abstractions.Services
{
    public interface IRingBearersDataService
    {
        Task<RingBearersModel?> GetJsonFileData(CancellationToken cancellationToken = default);
    }
}