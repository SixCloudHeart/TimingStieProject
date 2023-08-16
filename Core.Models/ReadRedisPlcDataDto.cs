using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ReadRedisPlcDataDto
    {
        public string? StoreDateTime { get; set; }
        public string? Value { get; set; }
    }
}
