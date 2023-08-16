using Core.Common.Extention;
using System;
using System.Collections.Generic;
using System.Text;
using static Core.Common.AppSettingManager.AppSetting;

namespace Core.Common
{
    public static class GlobalSettings
    {

        public static string Issuer
        {
            get
            {
                return AppSettings.GetValues(new string[] { "JWTConfig", "Issuer" });
            }
        }

        public static string Audience
        {
            get
            {
                return AppSettings.GetValues(new string[] { "JWTConfig", "Audience" });
            }
        }
        public static string Secret
        {
            get
            {
                return AppSettings.GetValues(new string[] { "JWTConfig", "Secret" });
            }
        }
        public static string MainDB
        {
            get
            {
                return AppSettings.GetValues(new string[] { "ConnectionStrings", "MainDB" });
            }
        }
        public static string OpcConnectionString
        {
            get
            {
                return AppSettings.GetValues(new string[] { "OpcConnectionStrings", "OpcConnectionString" });
            }
        }
        public static bool OPCAutoConnect
        {
            get
            {
                return AppSettings.GetValues(new string[] { "OpcConnectionStrings", "OPCAutoConnect" }).ToBool();
            }
        }
        public static string HttpConnectionString
        {
            get
            {
                return AppSettings.GetValues(new string[] { "HttpConnectionStrings", "HttpConnectionString" });
            }
        }
        public static bool DBTran
        {
            get
            {
                return AppSettings.GetValues(new string[] { "DBTran", "Enabled" }).ToBool();
            }
        }
        public static bool LogAOP
        {
            get
            {
                return AppSettings.GetValues(new string[] { "LogAOP", "Enabled" }).ToBool();
            }
        }
        public static string AppVersion
        {
            get
            {
                return AppSettings.GetValues(new string[] { "AppVersion", "Version" });
            }
        }


        public static string OracleSelect
        {
            get
            {
                return AppSettings.GetValues(new string[] { "Oracle", "OracleSelect" });
            }
        }

    }
}
