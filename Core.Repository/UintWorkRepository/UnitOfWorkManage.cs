using Core.IRepository.IUintWorkRepository;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Repository.UintWorkRepository
{
    public class UnitOfWorkManage : IUnitOfWorkManage
    {
        private readonly ISqlSugarClient _sqlSugarClient;
        public UnitOfWorkManage(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
           
        }

        /// <summary>
        /// 获取DB，保证唯一性
        /// </summary>
        /// <returns></returns>
         public SqlSugarClient GetDbClient()
        {
            return _sqlSugarClient as SqlSugarClient;
        }


    }
}
