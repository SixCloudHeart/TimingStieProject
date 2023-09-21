using Core.Entity;
using Core.IServices.IBaseServices;
using Core.Models.SugarModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices.ISugarServices
{
    public interface IStationDataTypeService : IBaseServices<StationDataType> 
    {
        /// <summary>
        /// 根据数据类型名称获取数据
        /// </summary>
        /// <param name="stationDataTypeDto"></param>
        /// <returns></returns>
        Task<StationDataTypeDto> GetByTypeNameStationDataAsync(StationDataTypeDto stationDataTypeDto);
    }
}
