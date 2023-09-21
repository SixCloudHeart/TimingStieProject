using Core.IServices.ISugarServices;
using Core.IServices;
using Serilog;
using Core.Entity;

namespace shijinheSqllite.BackgroundServices
{
    public class SiteScopedProcessingService : IScopedProcessingServices
    {
        readonly IConfiguration _configuration;
        readonly ILogger<SiteScopedProcessingService> _logger;

        readonly IStationInfoService _stationinfoService;

        readonly IReadSiteSettingXmlService _readSiteSettingXml;
        readonly IRedisServices _redisServices;
        public SiteScopedProcessingService(IRedisServices redisServices,  IStationInfoService stationinfoService, ILogger<SiteScopedProcessingService> logger, IConfiguration configuration, IReadSiteSettingXmlService readSiteSettingXml)
        {
            _redisServices = redisServices;
            _stationinfoService = stationinfoService;
            _logger = logger;
            _configuration = configuration;
            _readSiteSettingXml = readSiteSettingXml;
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
                string sitexmlFilePath = _configuration.GetValue<string>("SitePathXML");

                var siteInfos = await _readSiteSettingXml.ReadStationConfigXmlAsync(sitexmlFilePath);

                if (siteInfos == null)
                {
                    Log.Information("DoWorkAsync：获取站点数据集为空");

                    return;
                }

                // 添加站点信息 
                var isAddSuccess = await _stationinfoService.EditStationInfoAsync(siteInfos);

                if (!isAddSuccess)
                {
                    Log.Error("DoWorkAsync：添加数据出错了");
                    return;
                }
                // 获取所有站点数据
                List<StationInfo> queryStationinfos = await _stationinfoService.GetStationAllInfosAsync();

                int res = default;

                foreach (var item in queryStationinfos)
                {
                    if (item.TypeNameID.Equals(0))
                    {
                        /// 不定时数据写入
                        res = await _redisServices.SiteInfoWriteSqliteDBAsync(item, 0);
                        if (res != 10000)
                        {
                            Log.Error(string.Format("不定时写入错误:DoWorkAsync:循环错误,", item.StationName));
                            break;
                        }
                    }
                    else if (item.TypeNameID.Equals(1))
                    {
                        /// 定时数据写入
                        res = await _redisServices.SiteInfoWriteSqliteDBAsync(item, 1);
                        if (res != 10000)
                        {
                            Log.Error(string.Format("定时写入错误:DoWorkAsync:循环错误,", item.StationName));
                            break;
                        }
                    }
                    else
                    {
                        Log.Error(string.Format("SiteScopedProcessingService:DoWorkAsync:循环错误,错误的站点名称{0}", item.StationName));
                        break;
                    }
                }

                Log.Information("DoWorkAsync:结束");
                Log.Error("DoWorkAsync:Demo");

                await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
            }

        }
    }
}
