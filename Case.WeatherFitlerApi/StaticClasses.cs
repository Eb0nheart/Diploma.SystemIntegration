namespace Case.WeatherFilterApi;

public static class Utilities
{
    public static double? FahrenheitToCelsius(double? temperature)
    {
        if (temperature is null)
        {
            return null;
        }

        return Math.Round((temperature.Value - 32) * 5 / 9, 2);
    }
}

public static class Extensions
{

}