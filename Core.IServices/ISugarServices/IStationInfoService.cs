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
        Task<bool> AddStationInfo(StationInfo stationInfo);

        Task<List<StationInfo>> GetStationInfos(StationInfo stationInfo);
    }
}
