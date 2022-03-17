using Microsoft.Extensions.Caching.Memory;

namespace Case.CoreFunctionality.Implementations;

public class GatewayService : IGatewayService
{
    private const string SolarDataKey = "SOLAR";
    private const string WeatherDataKey = "WEATHER";

    private readonly IMemoryCache _cache;
    private readonly IWeatherFilterService _weatherService;
    private readonly ISolarPanelEfficiencyService _solarService;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions;

    public GatewayService(IMemoryCache cache, IWeatherFilterService weatherService, ISolarPanelEfficiencyService solarService)
    {
        _cache = cache;
        _weatherService = weatherService;
        _solarService = solarService;
        _cacheEntryOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12),
        };
    }

    public async Task<Dictionary<TimeOnly, double>> GetSolarDataAsync(CancellationToken token = default)
    {
        if (!_cache.TryGetValue(SolarDataKey, out Dictionary<TimeOnly, double> weatherData))
        {
            var data = await _solarService.GetEfficiencyForTodayAsync(token);
            _cache.Set(SolarDataKey, data, _cacheEntryOptions);
        }

        return weatherData;
    }

    public async Task<IEnumerable<WeatherForecast>> GetWeatherDataAsync(CancellationToken token = default)
    {
        if(!_cache.TryGetValue(WeatherDataKey, out IEnumerable<WeatherForecast> weatherData))
        {
            var data = await _weatherService.GetWeatherForecastAsync(token);
            _cache.Set(WeatherDataKey, data, _cacheEntryOptions);
        }

        return weatherData;
    }
}
