using Microsoft.AspNetCore.Mvc;
using System.ServiceModel;

namespace Case.WeatherFitlerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ForecastServiceClient _forecastClient;

        public WeatherForecastController(ForecastServiceClient forecastClient)
        {
            _forecastClient = forecastClient;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<WeatherData>> Get()
        {
            await OpenClientIfNecessary();

            GetForecastResponse response = null;
            try
            {
                response = await _forecastClient.GetForecastAsync("55.495972,9.473052", "BINGBONG");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            var forecast = response?.Body.GetForecastResult;

            var currentConditions = forecast.location.currentConditions;

            if (forecast is null)
            {
                return BadRequest("No data available for that location");
            }

            return Ok();
        }

        private async Task OpenClientIfNecessary()
        {
            if (_forecastClient.State == CommunicationState.Opened)
            {
                await _forecastClient.OpenAsync();
            }
        }
    }
}