using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.SugarModel
{
    public class StationInfoDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int StationID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TypeNameID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? StationName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? ModfiyTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Creater { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? GuidText { get; set; }
    }
}
