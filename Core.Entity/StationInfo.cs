using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity
{
    /// <summary>
    /// 站点信息 
    /// </summary>
    [SugarTable("StantionInfo")]
    public class StationInfo
    {
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public int StationID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "TypeNameID")]
        public int TypeNameID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "StationName")]
        public string? StationName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "CreateTime")]
        public string? CreateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ModfiyTime")]
        public string? ModfiyTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "Creater")]
        public string? Creater { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "GuidText")]
        public string? GuidText { get; set; }
    }
}
