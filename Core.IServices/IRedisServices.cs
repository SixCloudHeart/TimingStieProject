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
        Task<bool> TimingSiteInfoWriteDBAsync(StationInfo station);
        Task<bool> NotTimingSiteInfoWriteDBAsync(StationInfo station);
    }
}
