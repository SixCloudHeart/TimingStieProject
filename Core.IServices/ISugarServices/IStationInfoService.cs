using Core.Entity;
using Core.IServices.IBaseServices;
using Core.Models.SugarModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices.ISugarServices
{
    public interface IStationInfoService : IBaseServices<StationInfo> 
    {
        /// <summary>
        ///  编辑站点信息 【添加 更新】
        /// </summary>
        /// <param name="stationInfo"></param>
        /// <returns></returns>
        Task<bool> EditStationInfoAsync(List<StationInfo> stationInfo);
        /// <summary>
        /// 获取站点的所有数据
        /// </summary>
        /// <returns></returns>
        Task<List<StationInfo>> GetStationAllInfosAsync();
        /// <summary>
        /// 根据名称查询对应的站点数据
        /// </summary>
        /// <param name="stationInfo"></param>
        /// <returns></returns>
        Task<StationInfo> GetByStationNameStationInfoAsync(StationInfo stationInfo);
    }
}
