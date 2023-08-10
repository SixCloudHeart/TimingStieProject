using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.IRepository.IUintWorkRepository
{
    public interface IUnitOfWorkManage
    {
        SqlSugarClient GetDbClient();
    }
}
