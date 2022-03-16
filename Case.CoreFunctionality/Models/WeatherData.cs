namespace Case.CoreFunctionality.Models;

public class WeatherData
{
    public CurrentWeather CurrentWeather { get; set; }

    public IEnumerable<WeatherForecast> WeatherForecasts { get; set; }
}