namespace Case.ElectricityService;

public class ElectricityPriceListener : BackgroundService
{
    private readonly IElectricityPriceService _priceService;

    public ElectricityPriceListener(IElectricityPriceService priceService)
    {
        _priceService = priceService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _priceService.ListenForPriceRequests(stoppingToken);
    }
}
