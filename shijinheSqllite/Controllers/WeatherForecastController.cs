using Core.IServices.ISugarServices;
using Core.Models.SugarModel;
using Microsoft.AspNetCore.Mvc;

namespace shijinheSqllite.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IStationDataTypeService _stationDataTypeService;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IStationDataTypeService stationDataTypeService, ILogger<WeatherForecastController> logger)
        {
            _stationDataTypeService = stationDataTypeService;
            _logger = logger;
        }
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };


     

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost(Name = "StationDataType")]
        public async Task<List<StationDataTypeDto>> Post(StationDataTypeDto stationDataTypeDto)
        {
            return await _stationDataTypeService.GetStationDataTypesAsync(stationDataTypeDto);
        }
    }
}