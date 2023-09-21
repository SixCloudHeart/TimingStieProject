using Core.Entity;
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
    /// <summary>
    /// 站点类型服务实现
    /// </summary>
    public class StationDataTypeServices : BaseServices<StationDataType>, IStationDataTypeService
    {

          private   readonly IStationDataTypeRepository _stationDataTypeRepository;
        public StationDataTypeServices(IStationDataTypeRepository stationDataTypeRepository)
        {
            _stationDataTypeRepository = stationDataTypeRepository;
        }

        public async Task<StationDataTypeDto> GetByTypeNameStationDataAsync(StationDataTypeDto stationDataTypeDto)
        {
            return await _stationDataTypeRepository.GetByTypeNameStationDataAsync(stationDataTypeDto);
        }
    }
}
