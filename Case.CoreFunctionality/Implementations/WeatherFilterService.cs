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

        public async Task<WeatherData> GetWeatherDataForKoldingAsync()
        {
            var response = await _client.GetForecastAsync("55.495972,9.473052", "Jeger1studerende");
            
            var forecast = response?.Body.GetForecastResult;

            ArgumentNullException.ThrowIfNull(forecast, nameof(forecast));

            var currentConditions = forecast.location.currentConditions;

            ArgumentNullException.ThrowIfNull(currentConditions, nameof(currentConditions));

            return WeatherData.MapFromCurrentConditions(currentConditions);
        }
    }
}
