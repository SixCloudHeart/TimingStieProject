using Core.IServices.IBaseServices;
using Core.Models.SugarModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices.ISugarServices
{
    public interface IStationDataTypeService : IBaseServices<StationDataTypeDto> 
    {
        Task<List<StationDataTypeDto>> GetStationDataTypesAsync(StationDataTypeDto stationDataTypeDto);
    }
}
