namespace Case.CoreFunctionality.Interfaces;

public interface IWeatherFilterService
{
    public Task<WeatherData> GetWeatherDataForKoldingAsync();
}