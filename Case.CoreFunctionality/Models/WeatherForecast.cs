using WeatherService;

namespace Case.CoreFunctionality.Models;

public class WeatherForecast
{
    public double? Temperature { get; set; }

    public string Conditions { get; set; }

    public int CloudCover { get; set; }

    public DateTime DateTime { get; set; }

    internal static WeatherForecast MapFromValue(Value value)
        => new()
        {
            Temperature = value.temp.HasValue ? Math.Round(value.temp.Value, 2) : value.temp,
            Conditions = value.conditions,
            CloudCover = value.cloudcover.HasValue ? (int)value.cloudcover.Value : 0,
            DateTime = value.datetimeStr,
        };
}
