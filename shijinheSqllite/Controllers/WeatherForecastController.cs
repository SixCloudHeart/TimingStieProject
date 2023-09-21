using Core.IServices.ISugarServices;
using Core.Models.SugarModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace shijinheSqllite.Controllers
{
    [ApiController]
    [Route("/api/v2/[controller]/[Action]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IStationDataTypeService _stationDataTypeService;
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDistributedCache _cache;

        public WeatherForecastController(IStationDataTypeService stationDataTypeService, ILogger<WeatherForecastController> logger, IDistributedCache cache)
        {
            _stationDataTypeService = stationDataTypeService;
            _logger = logger;
            _cache = cache;
        }
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };


     

      

        [HttpPost(Name = "StationDataType")]
        public async Task<StationDataTypeDto> Post(StationDataTypeDto stationDataTypeDto)
        {
            return await _stationDataTypeService.GetByTypeNameStationDataAsync(stationDataTypeDto);
        }

      

    }
}