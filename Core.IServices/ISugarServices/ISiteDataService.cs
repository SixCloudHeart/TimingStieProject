using Core.Entity;
using Core.IServices.IBaseServices;
using Core.Models.SugarModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices.ISugarServices
{
    public interface ISiteDataService : IBaseServices<SiteData> 
    {
       Task<bool> AddSiteDatas(List<SiteData> siteDatas);
       Task<int> CountSiteDatas();
    }
}
