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
using static System.Collections.Specialized.BitVector32;

namespace Core.Services
{
    public class RedisServices : IRedisServices
    {
        readonly IConnectionMultiplexer _multiplexer;

        readonly IStationInfoService _stationInfoService;
        readonly ISiteDataService _stationDataService;


        readonly ILogger<RedisServices> _logger;

        public RedisServices(IConnectionMultiplexer multiplexer, IStationInfoService stationInfoService, ISiteDataService stationDataService, ILogger<RedisServices> logger)
        {
            _multiplexer = multiplexer;
            _stationInfoService = stationInfoService;
            _stationDataService = stationDataService;

            _logger = logger;
        }
        /// <summary>
        /// 处理定时类型 写入数据库
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public async Task<int> SiteInfoWriteSqliteDBAsync(StationInfo station, int selectedRedisDb)
        {
            int resultCode = 10000;

            if (station == null)
            {
                _logger.LogError("添加的站点信息为空。");
                return resultCode;
            }
            // 同步一天前的数据 删除 两天前的数据
            string lateOneDay = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");

            string getRedisKey = string.Format("{0}{1}", lateOneDay, station.StationName);

            bool  redisExistKye= await _multiplexer.GetDatabase(3).KeyExistsAsync(getRedisKey);

            if (redisExistKye)
            {
                return resultCode;
            }
            // 存放已同步的数据key在redis中 三天后自动过期
            await _multiplexer.GetDatabase(3).StringSetAsync(getRedisKey, getRedisKey, TimeSpan.FromDays(3));

           
            try
            {
                // 获取一天前的redis中的数据
                var redisStorageList = await _multiplexer.GetDatabase(selectedRedisDb).ListRangeAsync(getRedisKey);
                // 初始化添加的plc List
                List<SiteData> addSiteDataList = new List<SiteData>();

                foreach (var item in redisStorageList)
                {
                    var plcDto = JsonConvert.DeserializeObject<ReadRedisPlcDataDto>(item);
                    if (plcDto != null)
                    {
                        SiteData addSiteData = new SiteData()
                        {
                            GuidText = plcDto.ID.ToString(),
                            ReadPLCTime = plcDto.StoreDateTime,
                            StationID = station.StationID,
                            StrData = plcDto.Value,
                            CreateTime = DateTime.Now.ToString("G"),
                        };
                        addSiteDataList.Add(addSiteData);
                    }
                }
                int cureentRedisKeyStorageCount = redisStorageList.Count();
                int cureentAddSiteListCount = addSiteDataList.Count;
                if (addSiteDataList.Count==0)
                {
                    return resultCode;
                }

                bool isAddSiteSuccess = await _stationDataService.AddSiteDataAsync(addSiteDataList);


                _logger.LogInformation(string.Format("当前同步的Key是{0}，同步数据：当前redis总数:{1},sqlite添加的数据条数:{2};", getRedisKey, cureentRedisKeyStorageCount, cureentAddSiteListCount));

                // 获取前两天的日期 删除对应redis数据  
                string lateTwoDaySuffix = DateTime.Now.AddDays(-2).ToString("yyyyMMdd");


                string deteleRedisKey = string.Format("{0}{1}", lateTwoDaySuffix, station.StationName);
                var existDeteleRedisKey = await _multiplexer.GetDatabase(selectedRedisDb).KeyExistsAsync(deteleRedisKey);

                if (existDeteleRedisKey)
                {
                    await _multiplexer.GetDatabase(selectedRedisDb).KeyDeleteAsync(deteleRedisKey);
                    _logger.LogInformation(string.Format("当前删除的Key是{0}", deteleRedisKey));
                }
                else
                {
                    _logger.LogInformation(string.Format("当前删除的redisKey不存在是{0}", deteleRedisKey));
                }
                return resultCode;


            }
            catch (Exception ex)
            {
                _logger.LogError("处理数据写入sqlite失败"+ex.Message);
                _logger.LogError("失败详情" + ex.ToString());
                return 10001;
            }

        }






    }
}
