using Core.Entity;
using Core.IRepository.ISugarRepository;
using Core.IRepository.IUintWorkRepository;
using Core.Models.SugarModel;
using Core.Repository.BaseRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repository.SqlSugarRepository
{
    public class StationInfoRepository : BaseRepository<StationInfo>, IStationinfoRepository
    {
        public StationInfoRepository(IUnitOfWorkManage unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// 根据站点名称查询数据
        /// </summary>
        /// <param name="stationInfo"></param>
        /// <returns></returns>
        public Task<StationInfo> GetByStationNameStationInfoAsync(StationInfo stationInfo)
        {
          return   Db.Queryable<StationInfo>().Where(s =>  s.StationName.Equals(stationInfo.StationName)).FirstAsync();
        }
    }
}
