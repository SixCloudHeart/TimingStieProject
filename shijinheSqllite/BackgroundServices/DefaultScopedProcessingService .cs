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
        /// <summary>
        /// 定时 任务
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
               
                Log.Information("DoWorkAsync:开始");
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
                        if (siteInfo.TypeNameID.Equals(0))
                        {
                            var isInset = await _redisServices.NotTimingSiteInfoWriteDBAsync(siteInfo);

                        }
                        else if (siteInfo.TypeNameID.Equals(1))
                        {
                            var isInset = await _redisServices.TimingSiteInfoWriteDBAsync(siteInfo);
                        }
                        else 
                        {
                            string logMes = string.Format("DefaultScopedProcessingService:DoWorkAsync:当前循环的类型为{0}循环处理类型无此类型。",siteInfo.TypeNameID.ToString());
                            Log.Error(logMes);
                        }

                    }
                }
                Log.Information("DoWorkAsync:结束");
                Log.Error("DoWorkAsync:Demo");

                await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken);
            }
                
        }
    }
}
