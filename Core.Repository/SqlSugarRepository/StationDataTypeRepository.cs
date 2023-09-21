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
    public class StationDataTypeRepository : BaseRepository<StationDataType>, IStationDataTypeRepository
    {
        public StationDataTypeRepository(IUnitOfWorkManage unitOfWork) : base(unitOfWork)
        {

        }
        /// <summary>
        /// 根据数据类型名称获取数据
        /// </summary>
        /// <param name="stationDataTypeDto"></param>
        /// <returns></returns>
        public async Task<StationDataTypeDto> GetByTypeNameStationDataAsync(StationDataTypeDto stationDataTypeDto)
        {

            var res= await Db.Queryable<StationDataType>().WhereIF(string
                .IsNullOrEmpty(stationDataTypeDto.TypeName.Trim()), s => s.TypeName.Equals(stationDataTypeDto.TypeName)).Select(s => new StationDataTypeDto()
                {
                    CreateDate = DateTime.Parse(s.CreateDate),
                    ModifyDate = DateTime.Parse(s.ModifyDate),
                    GuidText = Guid.Parse(s.GuidText),
                    TypeName = s.TypeName,
                    TypeNameID = s.TypeNameID,

                }).FirstAsync();
            return res;
        }
    }
}
