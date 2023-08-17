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
        public async Task<bool>  AddSiteDatas(List<SiteData> siteDatas)
        {
            bool result = false;
            result = await _repository.Add(siteDatas) > 0;
            return result;
        }
        public async Task<int> CountSiteDatas() 
        {
            var result = await _repository.Query();

            return result.Count;
        }
    }
}
