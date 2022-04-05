using Microsoft.Extensions.Caching.Memory;

namespace Case.CoreFunctionality.Implementations;

public class GatewayService : IGatewayService
{
    private const string SolarDataKey = "SOLAR";
    private const string WeatherDataKey = "WEATHER";
    private const string OfficeDataKey = "OFFICE";

    private readonly IMemoryCache _cache;
    private readonly IWeatherFilterService _weatherService;
    private readonly ISolarPanelEfficiencyService _solarService;
    private readonly IRepository<RoomTemperature> _repository;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions;

    public GatewayService(
        IMemoryCache cache, 
        IWeatherFilterService weatherService, 
        ISolarPanelEfficiencyService solarService, 
        IRepository<RoomTemperature> repository)
    {
        _cache = cache;
        _weatherService = weatherService;
        _solarService = solarService;
        _repository = repository;
        _cacheEntryOptions = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12),
        };
    }

    public async Task<IEnumerable<RoomTemperature>> GetRoomTemperatureAsync(CancellationToken token = default)
        => await _cache.GetData(OfficeDataKey, _cacheEntryOptions, () => _repository.GetLast24HoursAsync(token));

    public async Task<Dictionary<TimeOnly, double>> GetSolarDataAsync(CancellationToken token = default)
        => await _cache.GetData(SolarDataKey, _cacheEntryOptions, () => _solarService.GetEfficiencyForTodayAsync(token));

    public async Task<IEnumerable<WeatherForecast>> GetWeatherDataAsync(CancellationToken token = default)
        => await _cache.GetData(WeatherDataKey, _cacheEntryOptions, () => _weatherService.GetWeatherForecastAsync(token));

}
