using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace Core.Models.SugarModel
{
    public class StationDataTypeDto
    {
       
        public int TypeNameID { get; set; }
        /// <summary>
        /// 
        /// </summary>
       
        public string TypeName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ModifyDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid GuidText { get; set; }
    }
}
