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
    public class ReadSiteSettingXML : IReadSiteSettingXML
    {
        readonly ILogger<ReadSiteSettingXML> _logger;

        public ReadSiteSettingXML(ILogger<ReadSiteSettingXML> logger)
        {

            _logger = logger;
        }


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
                    if (item.Attribute("SiteType").Value.ToString() == "0")
                    {
                        part.StationName = item.Element("StationName")?.Value.ToString() ?? "0";
                        part.TypeNameID = int.Parse(item.Element("StationType")?.Value.ToString() ?? "0");
                        part.Creater = "System";
                        part.CreateTime= DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss");
                        part.ModfiyTime = DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss");
                        part.GuidText = Guid.NewGuid().ToString();
                    }

                    list.Add(part);
                };
                await Task.CompletedTask;
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
                return null;
            }
        }

    }
}
