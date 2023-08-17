using Core.IServices.ISugarServices;
using Core.IServices;
using Serilog;

namespace shijinheSqllite.BackgroundServices
{
    public class DefaultScopedProcessingService : IScopedProcessingServices
    {
        readonly IConfiguration _configuration;
        readonly ILogger<DefaultScopedProcessingService> _logger;

        readonly IStationInfoService _stationinfoService;

        readonly IReadSiteSettingXML _readSiteSettingXML;
        readonly IRedisServices _redisServices;
        public DefaultScopedProcessingService(IRedisServices redisServices, IReadSiteSettingXML readSiteSettingXML, IStationInfoService stationinfoService, ILogger<DefaultScopedProcessingService> logger, IConfiguration configuration)
        {
            _redisServices = redisServices;
            _readSiteSettingXML = readSiteSettingXML;
            _stationinfoService = stationinfoService;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                string xmlPath = _configuration.GetValue<string>("SitePathXML");
              

                var siteInfos = await _readSiteSettingXML.ReadStationConfigXmlAsync(xmlPath);

                if (siteInfos == null)
                {
                    return;
                }
                foreach (var siteInfo in siteInfos)
                {
                    siteInfo.GuidText=Guid.NewGuid().ToString();
                    var isAddSuccess = await _stationinfoService.AddStationInfo(siteInfo);
                    if (isAddSuccess)
                    {
                        var isInset = await  _redisServices.NotTimingSiteInfoWriteDBAsync(siteInfo);
                        
                    }
                }
                Log.Information("DoWorkAsync:结束");
                Log.Error("DoWorkAsync:Demo");

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
                
        }
    }
}
