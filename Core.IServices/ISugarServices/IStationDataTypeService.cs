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
        Task<List<StationDataTypeDto>> GetStationDataTypesAsync(StationDataTypeDto stationDataTypeDto);
    }
}
