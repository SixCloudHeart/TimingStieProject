using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IRedisServices
    {
        /// <summary>
        ///  处理数据写入数据库  
        /// </summary>
        /// <param name="station"></param>
        /// <param name="selectedRedisDB"></param>
        /// <returns></returns>
        Task<int> SiteInfoWriteSqliteDBAsync(StationInfo station,int selectedRedisDB);
     
    }
}
