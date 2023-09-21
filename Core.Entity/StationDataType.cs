using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entity
{
    /// <summary>
    /// 站点数据类型
    /// </summary>
    [SugarTable("DataType")]
    public class StationDataType
    {
        [SugarColumn(ColumnName= "TypeNameID", IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int TypeNameID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "TypeName")]
        public string TypeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "CreateDate")]
        public string CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "ModifyDate")]
        public string ModifyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(ColumnName = "GuidText")]
        public string GuidText { get; set; }
    }
}
