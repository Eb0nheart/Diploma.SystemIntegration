namespace Case.WeatherFilterApi.Models;
public class WeatherResponse
{
    public WeatherData Forecast { get; set; }

    public WeatherData CurrentWeather { get; set; }
}
