using Core.IServices.ISugarServices;
using Core.IServices;

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

                string xmlPath = _configuration.GetConnectionString("SitePathXML");
                var siteInfos = (await _readSiteSettingXML.ReadStationConfigXmlAsync(xmlPath)).FindAll(w => w.TypeNameID.Equals(0));

                if (siteInfos == null)
                {
                    return;
                }
                foreach (var siteInfo in siteInfos)
                {
                    var isAddSuccess = await _stationinfoService.AddStationInfo(siteInfo);
                    if (isAddSuccess)
                    {
                        var isInset = await _redisServices.NotTimingSiteInfoWriteDBAsync(siteInfo);
                        if (!isInset)
                        {
                            break;
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
                
        }
    }
}
