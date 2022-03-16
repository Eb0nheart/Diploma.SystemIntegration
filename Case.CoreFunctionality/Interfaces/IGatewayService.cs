namespace Case.CoreFunctionality.Interfaces;

// Services which acts as single "endpoint for dashboard. Should cache the data when received. 
public interface IGatewayService
{
    Dictionary<TimeOnly, double> GetSolarData(CancellationToken token = default);

    CurrentWeather GetWeatherData(CancellationToken token = default);
}
