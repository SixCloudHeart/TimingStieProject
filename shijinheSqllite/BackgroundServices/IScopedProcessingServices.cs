namespace shijinheSqllite.BackgroundServices
{
    public interface IScopedProcessingServices
    {
        Task DoWorkAsync(CancellationToken stoppingToken);
    }
}
