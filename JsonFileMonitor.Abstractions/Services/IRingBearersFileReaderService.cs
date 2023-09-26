namespace JsonFileMonitor.Abstractions.Services
{
    public interface IRingBearersFileReaderService
    {
        Task<string> ReadJsonFile(CancellationToken cancellationToken = default);
        DateTime GetFileLastWriteDate();
    }
}