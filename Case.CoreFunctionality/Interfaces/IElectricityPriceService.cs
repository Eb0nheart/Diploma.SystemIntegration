namespace Case.CoreFunctionality.Interfaces;

public interface IElectricityPriceService
{
    Task<Dictionary<TimeOnly, double>> GetNextDaysPricesAsync(CancellationToken token = default);
}
