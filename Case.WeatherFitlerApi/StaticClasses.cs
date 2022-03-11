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

    public static Icon MapIcon(string icon)
    {
        if (string.IsNullOrWhiteSpace(icon))
        {
            return Icon.NotSet;
        }

        var normalisedIcon = icon.Replace("-", "");
        var iconParsed = Enum.TryParse(normalisedIcon, out Icon mappedIcon);

        return iconParsed ? mappedIcon : Icon.NotSet; 
    }
}