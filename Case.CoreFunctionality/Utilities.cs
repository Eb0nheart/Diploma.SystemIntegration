namespace Case.CoreFunctionality;
internal class Utilities
{
    internal static Icon MapIcon(string icon)
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