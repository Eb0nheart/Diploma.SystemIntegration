namespace Case.ElectricityTask.Services;

public interface IElectricityPriceService
{
    Task<Dictionary<TimeOnly, double>> GetNextDaysPricesAsync(CancellationToken token = default);
}
