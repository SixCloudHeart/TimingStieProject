﻿using Core.Entity;
using Core.IRepository.ISugarRepository;
using Core.IServices;
using Core.IServices.ISugarServices;
using Core.Models.SugarModel;
using Core.Services.BaseServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.SugarServices
{
    public class StationInfoServices : BaseServices<StationInfo>, IStationInfoService
    {
        readonly IReadSiteSettingXML _readSiteSettingXML;
        readonly IStationinfoRepository _stationinfoRepository;
      
        public StationInfoServices(IReadSiteSettingXML readSiteSettingXML, IStationinfoRepository stationinfoRepository)
        {
            _readSiteSettingXML = readSiteSettingXML;
            _stationinfoRepository = stationinfoRepository;
         
        }
        public async Task<bool> AddStationInfo(StationInfo stationInfo)
        {

         var existSite= await _stationinfoRepository.Query(w => w.StationName.Equals(stationInfo.StationName));
           
            return true;
        }
    }
}