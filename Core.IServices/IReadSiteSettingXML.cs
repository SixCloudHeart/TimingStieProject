﻿using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface IReadSiteSettingXML
    {
        Task<List<StationInfo>> ReadStationConfigXmlAsync(string filePath);
    }
}