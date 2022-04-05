namespace Case.CoreFunctionality.Interfaces;

public interface IElectricityPriceService
{
    Task ListenForPriceRequests(CancellationToken token);
}
