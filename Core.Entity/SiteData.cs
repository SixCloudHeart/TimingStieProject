using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity
{
    /// <summary>
    /// 站点存储数据
    /// </summary>
    [SugarTable("SiteData")]
    public class SiteData
    {
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public int SiteDataID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "StationID")]
        public int StationID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "NumberData")]
        public double NumberData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "StrData")]
        public string? StrData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ReadPLCTime")]
        public string? ReadPLCTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "IPaddress")]
        public string? IPaddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "GuidText")]
        public string? GuidText { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        public string? CreateTime { get; set; }
    }
}
