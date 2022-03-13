namespace Case.ElectricityTask.Services.Implementations;
public class ElectricityPriceService : IElectricityPriceService
{
    public async Task<Dictionary<TimeOnly, double>> GetNextDaysPricesAsync(CancellationToken token = default)
    {
        return new()
        {
            { TimeOnly.FromDateTime(DateTime.Now), 23.64 }
        };
    }
}
