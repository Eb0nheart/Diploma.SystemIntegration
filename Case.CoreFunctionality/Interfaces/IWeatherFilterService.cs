namespace Case.CoreFunctionality.Interfaces;

public interface IWeatherFilterService
{
    Task<WeatherData> GetWeatherDataForKoldingAsync(CancellationToken token = default);

    Task<IEnumerable<WeatherForecast>> GetWeatherForecastAsync(CancellationToken token = default);
}