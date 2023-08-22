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
    public class SiteDataServices : BaseServices<SiteData>, ISiteDataService
    {
        readonly ISiteDataRepository _repository;
        public SiteDataServices(ISiteDataRepository repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// 添加站点对应的数据
        /// </summary>
        /// <param name="siteDatas">redis中存储的集合</param>
        /// <returns></returns>
        public async Task<bool>  AddSiteDatas(List<SiteData> siteDatas)
        {
            bool result = false;
            List<SiteData> siteDatasList = new List<SiteData>(); ;
            foreach (var siteData in siteDatas)
            {
                var isExistSiteData = await _repository.Query(w => w.GuidText.Equals(siteData.GuidText));
                if (isExistSiteData==null)
                {
                    siteDatasList.Add(siteData);
                }
            }
            result = await _repository.Add(siteDatasList) > 0;
            return result;
        }
        /// <summary>
        /// 查询对应的数据总数
        /// </summary>
        /// <returns></returns>
        public async Task<int> CountSiteDatas() 
        {
            var result = await _repository.Query();

            return result.Count;
        }
    }
}
