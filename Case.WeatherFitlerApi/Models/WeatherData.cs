namespace Case.WeatherFilterApi.Models;

public class WeatherData
{
    public DateTime ApplicableDate { get; set; }

    public bool IsCurrentWeather => DateOnly.FromDateTime(ApplicableDate) == DateOnly.FromDateTime(DateTime.Now);

    // Data fields chosen based on this: https://easybeinggreen.com.au/solar-panel-efficiency/

    public double? Temperature { get; set; }

    public Icon Conditions { get; set; }

    public DateTime SunRise { get; set; }

    public DateTime SunSet { get; set; }

    public int? CloudCover
        => Conditions switch
        {
            Icon.cloudy => 90,
            Icon.partlycloudyday | Icon.partlycloudynight => 20,
            Icon.clearday | Icon.clearnight => 20,
            _ => null
        };

    public static WeatherData MapFromCurrentConditions(Currentconditions currentconditions)
        => new()
        {
            ApplicableDate = currentconditions.datetime,
            Temperature = currentconditions.temp.HasValue ? Math.Round(currentconditions.temp.Value, 2) : currentconditions.temp,
            Conditions = Utilities.MapIcon(currentconditions.icon),
            SunRise = currentconditions.sunrise,
            SunSet = currentconditions.sunset
        };
}