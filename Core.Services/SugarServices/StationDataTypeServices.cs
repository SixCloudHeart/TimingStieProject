using Core.IRepository.ISugarRepository;
using Core.IServices.ISugarServices;
using Core.Models.SugarModel;
using Core.Services.BaseServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.SugarServices
{
    public class StationDataTypeServices : BaseServices<StationDataTypeDto>, IStationDataTypeService
    {

          private   readonly IStationDataTypeRepository _stationDataTypeRepository;
        public StationDataTypeServices(IStationDataTypeRepository stationDataTypeRepository)
        {
            _stationDataTypeRepository = stationDataTypeRepository;
        }
        public async Task<List<StationDataTypeDto>> GetStationDataTypesAsync(StationDataTypeDto stationDataTypeDto)
        {
            return await _stationDataTypeRepository.GetStationDataTypeAsync(stationDataTypeDto);
        }
    }
}
