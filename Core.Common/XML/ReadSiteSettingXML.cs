using Core.Entity;
using Core.Models.SugarModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Dm.net.buffer.ByteArrayBuffer;

namespace Core.Common.XML
{
    public class ReadSiteSettingXML
    {
        readonly ILogger<ReadSiteSettingXML> _logger;

        public ReadSiteSettingXML( ILogger<ReadSiteSettingXML> logger)
        {
            
            _logger = logger;
        }
        /// <summary>
        ///  use Linq 读取XML configuration 数据 分批次
        /// </summary>
        /// <param name="filename">文件名称</param>
        /// <param name="StationType"></param>
        /// <returns></returns>
        public  async Task<List<StationInfo>?> ReadStationConfigXml(string filePath)
        {
            try
            {
                XElement purchaseOrder = XElement.Load(filePath);
                var partNos = ( from item in purchaseOrder.Descendants("StationNode").Descendants("StationAddressItems")
                              select new StationInfo { 
                                  StationName = item.Attribute("StationName")?.ToString()??"0",
                                  TypeNameID= int.Parse(item.Attribute("StationType")?.ToString()??"0")

                              }).ToList();
                await Task.CompletedTask;
                foreach ( var item in partNos )
                {

                    item.Creater = "System";
                    item.ModfiyTime = DateTime.Now.ToString("yyyy-mm=dd HH:mm:ss");
                    item.GuidText=Guid.NewGuid().ToString();
                };

                return partNos;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
                return null;
            }
        }
    }
}
