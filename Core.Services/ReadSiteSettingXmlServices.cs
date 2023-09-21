using Core.Entity;
using Core.IServices;
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

namespace Core.Services
{
    /// <summary>
    /// 读取XML 配置站点实现
    /// </summary>
    public class ReadSiteSettingXmlServices : IReadSiteSettingXmlService
    {
        readonly ILogger<ReadSiteSettingXmlServices> _logger;

        public ReadSiteSettingXmlServices(ILogger<ReadSiteSettingXmlServices> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 读取XML 下的站点信息数据
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public async Task<List<StationInfo>> ReadStationConfigXmlAsync(string filePath)
        {
            try
            {
                XElement purchaseOrder = XElement.Load(filePath);

                var partNos = from item in purchaseOrder.Descendants("StationNode")
                              select item;

                List<StationInfo> list = new List<StationInfo>();

                foreach (var item in partNos.Descendants("StationAddressItems"))
                {
                    StationInfo part = new StationInfo();
                   
                        part.StationName = item.Element("StationName")?.Value.ToString() ?? "0";
                        part.TypeNameID = int.Parse(item.Element("StationType")?.Value.ToString() ?? "0");
                        part.Creater = "System";
                        part.CreateTime= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        part.ModfiyTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        part.GuidText = Guid.NewGuid().ToString();

                    list.Add(part);
                };
                await Task.CompletedTask;
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(string.Format( "读取站点XML数据出错:{0}",ex.Message.ToString()));
                return null;
            }
        }

    }
}
