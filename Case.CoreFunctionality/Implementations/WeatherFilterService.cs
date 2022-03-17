using WeatherService;

namespace Case.CoreFunctionality.Implementations
{
    public class WeatherFilterService : IWeatherFilterService
    {
        private readonly ForecastServiceClient _client;

        public WeatherFilterService()
        {
            _client = new ForecastServiceClient();
        }

        public async Task<WeatherData> GetWeatherDataForKoldingAsync(CancellationToken token = default)
        {
            var response = await _client.GetForecastAsync("55.495972,9.473052", "Jeger1studerende");
            
            var forecast = response?.Body.GetForecastResult;

            ArgumentNullException.ThrowIfNull(forecast, nameof(forecast));

            var forecasts = forecast.location.values;
            var currentConditions = forecast.location.currentConditions;

            ArgumentNullException.ThrowIfNull(forecasts, nameof(forecasts));
            ArgumentNullException.ThrowIfNull(currentConditions, nameof(currentConditions));

            var currentWeather = CurrentWeather.MapFromCurrentConditions(currentConditions);

            var weatherForecasts = Enumerable.Empty<WeatherForecast>();

            if (forecasts.Any())
            {
                var nextDaysForecasts = forecasts.Take(25);
                weatherForecasts = nextDaysForecasts.Select(value => WeatherForecast.MapFromValue(value));
                weatherForecasts.ToList().ForEach(f => Console.WriteLine($"{f.DateTime} - {f.Temperature} - {f.Conditions} - {f.CloudCover}"));
            }

            return new WeatherData
            {
                CurrentWeather = currentWeather,
                WeatherForecasts = weatherForecasts
            };
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherForecastAsync(CancellationToken token = default)
            => (await GetWeatherDataForKoldingAsync(token)).WeatherForecasts;
    }
}
