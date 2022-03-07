namespace Case.WeatherFilterApi.Models;

public class WeatherData
{
    public DateOnly Date { get; set; }

    public int Precipation { get; set; }

    public double? Temperature { get; set; }

    public Conditions Conditions { get; set; }

    public TimeOnly SunRise { get; set; }

    public TimeOnly SunSet { get; set; }

    public static WeatherData MapFromCurrentConditions(Currentconditions currentconditions)
    {
        var precipationString = JsonConvert.SerializeObject(currentconditions.precip);
        var precipationParsed = double.TryParse(precipationString, out var precipationDouble);
        return new()
        {
            Precipation = precipationParsed ? (int)(precipationDouble * 100) : 0,
            Temperature = currentconditions.temp
        };
    }

    public static WeatherData MapFromColumns(Columns forecast)
    {
        var precipationString = JsonConvert.SerializeObject(forecast.precip);
        var precipationParsed = double.TryParse(precipationString, out var precipationDouble);
        var temperature = forecast.temp.unit.ToLowerInvariant() == "celsius" ? 
            forecast.temp.type : 
            Utilities.FahrenheitToCelsius(forecast.temp.type);

        return new()
        {
            Precipation = precipationParsed ? (int)(precipationDouble * 100) : 0,
            Temperature = temperature
        };
    }
}