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
        // 当前时间 后缀
       private static string NOWDATETIMESUFFIX= DateTime.Now.ToString("yyyymmdd");

        readonly ILogger<RedisServices> _logger;

        public RedisServices(IConnectionMultiplexer multiplexer, IStationInfoService stationInfoService, ISiteDataService stationDataService, ILogger<RedisServices> logger)
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
            string key = string.Format("{0}{1}", station.StationName, NOWDATETIMESUFFIX);
            try
            {
                var redisList = await _multiplexer.GetDatabase(1).ListRangeAsync(key);
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
                                GuidText = plcDataDto.ID.ToString(),
                                ReadPLCTime = plcDataDto.StoreDateTime,
                                StationID = station.StationID,
                                StrData = plcDataDto.Value,
                                CreateTime = DateTime.Now.ToString("G"),
                            };
                            siteData.Add(part);
                        }
                        newId++;
                    }
                }
                // 添加站点数据
                var result = await _stationDataService.AddSiteDatas(siteData);
                // 成功了就删除记录在redis中的数据
                if (result)
                {
                    // 获取前两天的日期 删除对应redis数据  还差一点  2023年8月21日16:55:23
                    string suffixStr2 = DateTime.Now.AddDays(-2).ToString("yyyymmdd");
                    if (!NOWDATETIMESUFFIX.Equals(suffixStr2))
                    {
                        string deteleKey = string.Format("{0}{1}", station.StationName, suffixStr2);


                        bool IsDeleteSuccess = await _multiplexer.GetDatabase(1).KeyDeleteAsync(deteleKey);
                        suffixStr2 = DateTime.Now.AddDays(-1).ToString("yyyymmdd");
                        deteleKey = string.Format("{0}{1}", station.StationName, suffixStr2);
                        bool IsDeleteSuccess2 = await _multiplexer.GetDatabase(1).KeyDeleteAsync(deteleKey);

                        _logger.LogError(message: string.Format("AddSiteDatas:Redis日志：删除:{0}了key,是否成功:{1};删除{2}key,是否成功:{3};"));

                        int redisSiteDataCount = siteData.Count;
                        var wirteCount = (await _stationInfoService.GetStationInfos(null)).Where(w => w.StationID.Equals(station.StationID)).ToList().Count;

                        string logMes = string.Format("AddSiteDatas：当前写入记录的数据站点名称为:{0}，成功写入的数据量为：{1}，redis存储实际的数据量为：{2}", station.StationName, wirteCount.ToString(), redisSiteDataCount.ToString());

                        _logger.LogError(message: logMes);

                        if (redisSiteDataCount.Equals(wirteCount))
                        {

                            await _multiplexer.GetDatabase(1).KeyDeleteAsync(key);
                        }
                    }

                   

                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message);
                return false;
            }

        }
        /// <summary>
        /// 处理 redis中 定时的数据
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public async Task<bool> NotTimingSiteInfoWriteDBAsync(StationInfo station)
        {
            if (station == null)
            {
                return false;
            }
            //string key = string.Format("{0}", station.StationName);
            string key = string.Format("{0}{1}", station.StationName, NOWDATETIMESUFFIX);

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
                                CreateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"),
                            };
                            siteData.Add(part);
                        }
                        newId++;
                    }
                }
                var result = await _stationDataService.AddSiteDatas(siteData);
                // 成功了就删除记录在redis中的数据
                if (result)
                {
                    // 获取前两天的日期 删除对应redis数据  还差一点  2023年8月21日16:55:23
                    string suffixStr2 = DateTime.Now.AddDays(-2).ToString("yyyymmdd");
                    if (!NOWDATETIMESUFFIX.Equals(suffixStr2))
                    {
                        string deteleKey = string.Format("{0}{1}", station.StationName, suffixStr2);


                        bool IsDeleteSuccess = await _multiplexer.GetDatabase(1).KeyDeleteAsync(deteleKey);
                        suffixStr2 = DateTime.Now.AddDays(-1).ToString("yyyymmdd");
                        deteleKey = string.Format("{0}{1}", station.StationName, suffixStr2);
                        bool IsDeleteSuccess2 = await _multiplexer.GetDatabase(1).KeyDeleteAsync(deteleKey);

                        _logger.LogError(message: string.Format("AddSiteDatas:Redis日志：删除:{0}了key,是否成功:{1};删除{2}key,是否成功:{3};"));

                        int redisSiteDataCount = siteData.Count;
                        var wirteCount = (await _stationInfoService.GetStationInfos(null)).Where(w => w.StationID.Equals(station.StationID)).ToList().Count;

                        string logMes = string.Format("AddSiteDatas：当前写入记录的数据站点名称为:{0}，成功写入的数据量为：{1}，redis存储实际的数据量为：{2}", station.StationName, wirteCount.ToString(), redisSiteDataCount.ToString());

                        _logger.LogError(message: logMes);

                        if (redisSiteDataCount.Equals(wirteCount))
                        {

                            await _multiplexer.GetDatabase(1).KeyDeleteAsync(key);
                        }
                    }
                }
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
