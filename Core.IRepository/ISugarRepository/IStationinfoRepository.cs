using Core.Entity;
using Core.IRepository.IBaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository.ISugarRepository
{
    public interface IStationinfoRepository:IBaseRepository<StationInfo>
    {
        /// <summary>
        /// 根据名称查询数据
        /// </summary>
        /// <param name="stationInfo"></param>
        /// <returns></returns>
        public Task<StationInfo> GetByStationNameStationInfoAsync(StationInfo stationInfo);
    }
}
