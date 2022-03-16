using WeatherService;

namespace Case.CoreFunctionality.Models;

public class CurrentWeather
{
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

    internal static CurrentWeather MapFromCurrentConditions(Currentconditions currentconditions)
        => new()
        {
            Temperature = currentconditions.temp.HasValue ? Math.Round(currentconditions.temp.Value, 2) : currentconditions.temp,
            Icon = Utilities.MapIcon(currentconditions.icon),
            SunRise = currentconditions.sunrise,
            SunSet = currentconditions.sunset
        };
}