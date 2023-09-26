using System;
using System.Threading;
using System.Threading.Tasks;
using JsonFileMonitor.Abstractions.Models.RingBearers;
using JsonFileMonitor.Abstractions.Services;
using Newtonsoft.Json;

namespace JsonFileMonitor.Services
{
    public class RingBearersDataService : IRingBearersDataService
    {
        private readonly IRingBearersFileReaderService _ringBearersFileReaderService;

        public RingBearersDataService(IRingBearersFileReaderService ringBearersFileReaderService)
        {
            _ringBearersFileReaderService = ringBearersFileReaderService;
        }

        public async Task<RingBearersModel?> GetJsonFileData(CancellationToken cancellationToken = default)
        {
            var ringBearersJson = await _ringBearersFileReaderService.ReadJsonFile(cancellationToken);

            if (string.IsNullOrWhiteSpace(ringBearersJson))
            {
                return null;
            }

            var ringBearersModel = JsonConvert.DeserializeObject<RingBearersModel>(ringBearersJson);

            return ringBearersModel;
        }
    }
}