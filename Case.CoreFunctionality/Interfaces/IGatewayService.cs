namespace Case.CoreFunctionality.Interfaces;

// Services which acts as single "endpoint for dashboard. Should cache the data when received. 
public interface IGatewayService
{
    Task<Dictionary<TimeOnly, double>> GetSolarDataAsync(CancellationToken token = default);

    Task<IEnumerable<WeatherForecast>> GetWeatherDataAsync(CancellationToken token = default);

    Task<IEnumerable<RoomTemperature>> GetRoomTemperatureAsync(CancellationToken token = default);
}
