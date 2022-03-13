using Case.CoreFunctionality.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Case.CoreFunctionality;

public static class Extensions
{
    public static IServiceCollection AddElectricityFunctionality(this IServiceCollection services)
    {
        services.AddTransient<IElectricityPriceService, ElectricityPriceService>();
        services.AddTransient<ISolarPanelEfficiencyService, SolarPanelEfficiencyService>();

        return services;
    }

    public static IServiceCollection AddWeatherFunctionality(this IServiceCollection services)
        => services.AddSingleton<IWeatherFilterService, WeatherFilterService>();

    public static IServiceCollection AddAllCaseFunctionality(this IServiceCollection services)
    {
        services.AddElectricityFunctionality();
        services.AddWeatherFunctionality();

        return services;
    }
}
