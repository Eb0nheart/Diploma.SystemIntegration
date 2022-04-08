using Case.CoreFunctionality.Implementations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace Case.CoreFunctionality;

public static class Extensions
{
    public static IServiceCollection AddElectricityFunctionality(this IServiceCollection services)
    {
        services.AddSingleton<IElectricityPriceService, ElectricityPriceService>();
        services.AddTransient(_ => new DateTimeSerializer());
        services.AddSingleton(_ => new HttpClient());
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

    public static async Task<T> GetData<T>(this IMemoryCache cache, string cacheKey, MemoryCacheEntryOptions cacheEntryOptions, Func<Task<T>> getDataFromProvider)
    {
        if (!cache.TryGetValue(cacheKey, out T data))
        {
            data = await getDataFromProvider();
            cache.Set(cacheKey, data, cacheEntryOptions);
        }

        return data;
    }
}
