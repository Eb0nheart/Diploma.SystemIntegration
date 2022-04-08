namespace Case.ElectricityService;

public class WorkerService : BackgroundService
{
    private readonly IElectricityPriceService _priceService;

    public WorkerService(IElectricityPriceService priceService)
    {
        _priceService = priceService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _priceService.ListenForPriceRequests(stoppingToken);
    }
}
