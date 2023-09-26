using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JsonFileMonitor.Abstractions.Constants;
using JsonFileMonitor.Abstractions.Services;

namespace JsonFileMonitor.Services
{
    public class RingBearersFileReaderService : IRingBearersFileReaderService
    {
        public async Task<string> ReadJsonFile(CancellationToken cancellationToken = default)
        {
            try
            {
                var jsonContent = await File.ReadAllTextAsync(FilePathConstants.JsonFilePath, Encoding.UTF8, cancellationToken);

                return jsonContent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return string.Empty;
            }
        }

        public DateTime GetFileLastWriteDate()
        {
            return File.GetLastWriteTime(FilePathConstants.JsonFilePath);
        }
    }
}