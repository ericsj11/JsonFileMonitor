using JsonFileMonitor.Abstractions.Constants;
using JsonFileMonitor.Services;
using Newtonsoft.Json;

namespace JsonFileMonitor.Tests.Services
{
    public class RingBearersJsonFileMonitorServiceTests
    {
        private readonly RingBearersFileReaderService _ringBearersFileReaderService;
        private readonly RingBearersDataService _ringBearersDataService;
        private readonly RingBearersFileMonitorService _ringBearersFileMonitorService;

        public RingBearersJsonFileMonitorServiceTests()
        {
            _ringBearersFileReaderService = new RingBearersFileReaderService();
            _ringBearersDataService = new RingBearersDataService(_ringBearersFileReaderService);
            _ringBearersFileMonitorService = new RingBearersFileMonitorService(_ringBearersDataService, _ringBearersFileReaderService);
        }

        [Fact]
        public async Task CheckJsonFile_NotChanged_Then_ModifiedAndChanged()
        {
            // TODO: I had a long debate with myself regarding mocking. I tried to mock the FileSystem and all the classes with Moq, but it increased the testing time alot.
            // I also didn't like how the test turned out when there was no external dependencies and no real need for mocking. 
            // I felt like it made the test more bloated and hard to follow without adding anything useful.        

            // Act
            // Read the Json file one time.
            var originalRingBearersData = await _ringBearersDataService.GetJsonFileData();
            // Check if the file has changed
            var initialHasFileChangedResult = _ringBearersFileMonitorService.HasFileChanged();

            // Assert
            Assert.False(initialHasFileChangedResult, "The initial check should report that the file has not changed.");

            // Modify the JSON file (Write original data but the file will be changed).
            await File.WriteAllTextAsync(FilePathConstants.JsonFilePath, JsonConvert.SerializeObject(originalRingBearersData));

            // Act again after modifying the file
            var updatedHasFileChangedResult = _ringBearersFileMonitorService.HasFileChanged();

            // Assert
            Assert.True(updatedHasFileChangedResult, "After modifying the file, the service should report that the file has changed.");
        }
    }
}