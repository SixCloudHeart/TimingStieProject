using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{

    public interface IReadSiteSettingXmlService
    {
        /// <summary>
        /// 读取XML 下的站点信息数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<List<StationInfo>> ReadStationConfigXmlAsync(string filePath);
    }
}
