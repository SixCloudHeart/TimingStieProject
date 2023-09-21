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
        /// <summary>
        /// 添加站点数据
        /// </summary>
        /// <param name="siteDatas"></param>
        /// <returns></returns>
       Task<bool> AddSiteDataAsync(List<SiteData> siteDatas);
        /// <summary>
        /// 获取站点对象总数
        /// </summary>
        /// <returns></returns>
       Task<int> GetSiteDataElementCountAsync();
    }
}
