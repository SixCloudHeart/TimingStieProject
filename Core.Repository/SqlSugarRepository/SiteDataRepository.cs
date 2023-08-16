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
    public class SiteDataRepository : BaseRepository<SiteData>, ISiteDataRepository
    {
        public SiteDataRepository(IUnitOfWorkManage unitOfWork) : base(unitOfWork)
        {

        }
     
    }
}
