using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.SugarModel
{
    public class StationDataDto
    {

        public int SiteDataID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int StationID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double NumberData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? StrData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? ReadPLCTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? IPaddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? GuidText { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? CreateTime { get; set; }
    }
}
