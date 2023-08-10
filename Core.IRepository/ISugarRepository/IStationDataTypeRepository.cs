using Core.Entity;
using Core.IRepository.IBaseRepository;
using Core.Models.SugarModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository.ISugarRepository
{
    /// <summary>
    /// 站点数据类型 R
    /// </summary>
    public interface IStationDataTypeRepository:IBaseRepository<StationDataType>
    {
        /// <summary>
        /// 查询站点类型
        /// </summary>
        /// <param name="stationDataTypeDto"></param>
        /// <returns></returns>
        Task<List<StationDataTypeDto>> GetStationDataTypeAsync(StationDataTypeDto stationDataTypeDto);
    }
}
