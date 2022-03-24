using Case.CoreFunctionality.Implementations;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace Case.CoreFunctionality;

public static class Extensions
{
    public static IServiceCollection AddElectricityFunctionality(this IServiceCollection services)
    {
        services.AddTransient<IElectricityPriceService, ElectricityPriceService>();
        services.AddTransient<ISolarPanelEfficiencyService, SolarPanelEfficiencyService>();
        services.AddSingleton(_ => new WebClient
        {
            Credentials = new NetworkCredential("studerende", "kmdp4gslmg46jhs")
        });

        return services;
    }

    public static IServiceCollection AddWeatherFunctionality(this IServiceCollection services)
        => services.AddSingleton<IWeatherFilterService, WeatherFilterService>();

    public static IServiceCollection AddRoomTemperatureFunctionality(this IServiceCollection services)
    {
        //SqlMapper.SetTypeMap(typeof(RoomTemperature),
        //    new CustomPropertyTypeMap(typeof(RoomTemperature), (type, columnName) =>
        //        type.GetProperties().FirstOrDefault(prop =>
        //            prop.GetCustomAttributes(false).OfType<ColumnAttribute>().Any(attr => attr.Name == columnName))));
        services.AddSingleton<IRepository<RoomTemperature>, RoomTemperatureRepository>();
        return services;
    }

    public static IServiceCollection AddAllCaseFunctionality(this IServiceCollection services)
    {
        services.AddElectricityFunctionality();
        services.AddWeatherFunctionality();
        services.AddMemoryCache();
        services.AddRoomTemperatureFunctionality();
        services.AddSingleton<IGatewayService, GatewayService>();

        return services;
    }
}
