using Core.Entity;
using Core.IRepository.ISugarRepository;
using Core.IServices;
using Core.IServices.ISugarServices;
using Core.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class RedisServices : IRedisServices
    {
        readonly IConnectionMultiplexer _multiplexer;

        readonly IStationInfoService _stationInfoService;
        readonly ISiteDataService _stationDataService;
      

        readonly ILogger<RedisServices> _logger;

        public RedisServices(IConnectionMultiplexer multiplexer, IStationInfoService stationInfoService, ISiteDataService stationDataService,  ILogger<RedisServices> logger)
        {
            _multiplexer = multiplexer;
            _stationInfoService = stationInfoService;
            _stationDataService = stationDataService;
          
            _logger = logger;
        }
        /// <summary>
        /// 处理redis 存储在数据库1 的数据
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public async Task<bool> TimingSiteInfoWriteDBAsync(StationInfo station)
        {
            if (station == null)
            {
                return false;
            }
            string suffixStr = DateTime.Now.ToString("yyyymmdd");
            string key = string.Format("{0}{1}", station.StationName, suffixStr);
            var redisList = await _multiplexer.GetDatabase(1).ListRangeAsync(key);

            StationInfo siteInfoList = (await _stationInfoService.Query(w => w.StationName == (station.StationName.ToString() ?? ""))).ToList().FirstOrDefault();

            var redisExistKey = (await _multiplexer.GetDatabase(4).StringGetAsync(siteInfoList.StationName)).HasValue;



            if (redisExistKey)
            {
                
                return false;
            }
            else
            {

                List<SiteData> siteData = new List<SiteData>();

                if (redisList != null)
                {
                    foreach (var item in redisList)
                    {
                        if (string.IsNullOrEmpty(item.ToString().Trim()))
                        {
                            var plcDataDto = JsonConvert.DeserializeObject<ReadRedisPlcDataDto>(item);
                            SiteData part = new SiteData()
                            {

                                GuidText = Guid.NewGuid().ToString(),
                                ReadPLCTime = plcDataDto.StoreDateTime,
                                StationID = station.StationID,
                                StrData = plcDataDto.Value,
                            };
                            siteData.Add(part);
                        }
                    }
                }
                var result = await _stationDataService.AddSiteDatas(siteData);
                return result;
            }

        }

        public async Task<bool> NotTimingSiteInfoWriteDBAsync(StationInfo station)
        {
            if (station == null)
            {
                return false;
            }
#if DEBUG
            string key = string.Format("{0}", station.StationName);
#else
            string suffixStr = DateTime.Now.ToString("yyyymmdd");
            string key = string.Format("{0}{1}", station.StationName, suffixStr);
#endif
           
            try
            {
                var redisList = await _multiplexer.GetDatabase(0).ListRangeAsync(key);
                var redisExistKey = (await _multiplexer.GetDatabase(4).StringGetAsync(key)).HasValue;
                List<SiteData> siteData = new List<SiteData>();
                if (redisExistKey)
                {
                    return false;
                }
                if (redisList != null)
                {
                    var newId = await _stationDataService.CountSiteDatas() + 1;
                    await _multiplexer.GetDatabase(4).StringSetAsync(key, key);
                    foreach (var item in redisList)
                    {
                        if (!string.IsNullOrEmpty(item.ToString().Trim()))
                        {
                            
                            var plcDataDto = JsonConvert.DeserializeObject<ReadRedisPlcDataDto>(item);
                            SiteData part = new SiteData()
                            {
                                 SiteDataID = newId,
                                GuidText = Guid.NewGuid().ToString(),
                                ReadPLCTime = plcDataDto.StoreDateTime,
                                StationID = station.StationID,
                                StrData = plcDataDto.Value,
                            };
                            siteData.Add(part);
                        }
                        newId++;
                    }
                }
                var result = await _stationDataService.AddSiteDatas(siteData);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message);
                return false;
            }


        }
    }
}
