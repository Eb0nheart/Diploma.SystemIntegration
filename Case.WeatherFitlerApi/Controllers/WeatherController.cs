using Case.CoreFunctionality.Interfaces;
using Case.CoreFunctionality.Models;
using Microsoft.AspNetCore.Mvc;

namespace Case.WeatherFitlerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherFilterService _forecastClient;

        public WeatherController(IWeatherFilterService forecastClient)
        {
            _forecastClient = forecastClient;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<WeatherData>> Get()
        {
            try
            {
                var data = await _forecastClient.GetWeatherDataForKoldingAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}