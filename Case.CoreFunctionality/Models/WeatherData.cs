using WeatherService;

namespace Case.CoreFunctionality.Models;

public class WeatherData
{
    public DateTime ApplicableDate { get; set; }

    public bool IsCurrentWeather => DateOnly.FromDateTime(ApplicableDate) == DateOnly.FromDateTime(DateTime.Now);
    public string? Conditions => Enum.GetName(typeof(Icon), Icon);

    // Data fields chosen based on this: https://easybeinggreen.com.au/solar-panel-efficiency/

    public double? Temperature { get; set; }

    public DateTime SunRise { get; set; }

    public DateTime SunSet { get; set; }

    public int? CloudCover
        => Icon switch
        {
            Icon.cloudy => 90,
            Icon.partlycloudyday | Icon.partlycloudynight => 20,
            Icon.clearday | Icon.clearnight => 20,
            _ => null
        };
    internal Icon Icon { get; set; }

    internal static WeatherData MapFromCurrentConditions(Currentconditions currentconditions)
        => new()
        {
            ApplicableDate = currentconditions.datetime,
            Temperature = currentconditions.temp.HasValue ? Math.Round(currentconditions.temp.Value, 2) : currentconditions.temp,
            Icon = Utilities.MapIcon(currentconditions.icon),
            SunRise = currentconditions.sunrise,
            SunSet = currentconditions.sunset
        };
}