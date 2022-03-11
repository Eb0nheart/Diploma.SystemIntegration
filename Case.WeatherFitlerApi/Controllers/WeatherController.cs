using Microsoft.AspNetCore.Mvc;

namespace Case.WeatherFitlerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ForecastServiceClient _forecastClient;

        public WeatherController(ForecastServiceClient forecastClient)
        {
            _forecastClient = forecastClient;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<WeatherData>> Get()
        {
            GetForecastResponse response = null;
            try
            {
                response = await _forecastClient.GetForecastAsync("55.495972,9.473052", "Jeger1studerende");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            var forecast = response?.Body.GetForecastResult;

            var currentConditions = forecast.location.currentConditions;

            if (currentConditions is null)
            {
                return BadRequest("No data available for that location");
            }

            var data = WeatherData.MapFromCurrentConditions(currentConditions);

            return Ok(data);
        }
    }
}