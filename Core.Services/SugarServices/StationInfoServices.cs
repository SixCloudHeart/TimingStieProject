using Core.Entity;
using Core.IRepository.ISugarRepository;
using Core.IServices;
using Core.IServices.ISugarServices;
using Core.Models.SugarModel;
using Core.Services.BaseServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.SugarServices
{
    /// <summary>
    /// 站点信息服务实现
    /// </summary>
    public class StationInfoServices : BaseServices<StationInfo>, IStationInfoService
    {
        readonly IStationinfoRepository _stationinfoRepository;

        public StationInfoServices(IStationinfoRepository stationinfoRepository)
        {
            _stationinfoRepository = stationinfoRepository;
        }
        /// <summary>
        /// 添加新的站点数据
        /// </summary>
        /// <param name="stationInfo"></param>
        /// <returns></returns>
        public async Task<bool> EditStationInfoAsync(List<StationInfo> stationInfo)
        {
            bool result = false;
            foreach (StationInfo item in stationInfo)
            {
                var existSite = (await _stationinfoRepository.Query(w => w.StationName.Equals(item.StationName))).FirstOrDefault();
                if (existSite != null)
                {
                    existSite.StationName = item.StationName;
                    existSite.GuidText = item.GuidText;
                    existSite.CreateTime = DateTime.Now.ToString("");
                    result = await _stationinfoRepository.Update(existSite);
                }
                else
                {
                    int newID = (await _stationinfoRepository.Query()).Count + 1;
                    item.StationID = newID;
                    result = await _stationinfoRepository.Add(stationInfo) > 0;
                }
            }
            return result;
        }
        /// <summary>
        /// 根据名称获取站点数据
        /// </summary>
        /// <param name="stationInfo"></param>
        /// <returns></returns>
        public async Task<StationInfo> GetByStationNameStationInfoAsync(StationInfo stationInfo)
        {
            var result = await _stationinfoRepository.GetByStationNameStationInfoAsync(stationInfo);
            return result;
        }

        /// <summary>
        /// 获取站点所有数据
        /// </summary>
        /// <param name="stationInfo"></param>
        /// <returns></returns>
        public async Task<List<StationInfo>> GetStationAllInfosAsync()
        {
            var result = await _stationinfoRepository.Query();
            return result;
        }



    }
}
