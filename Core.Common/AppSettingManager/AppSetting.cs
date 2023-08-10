using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.AppSettingManager
{
    public class AppSetting
    {
        public class AppSettings
        {
            private static object _lock = new object();
            private static IConfiguration? _Configuration;
            public AppSettings(IConfiguration configuration)
            {
                Configuration = configuration;
            }


            public static IConfiguration Configuration
            {
                get
                {
                    if (_Configuration == null)
                    {
                        lock (_lock)
                        {
                            if (_Configuration == null)
                            {

#if DEBUG
                                var builder = new ConfigurationBuilder()
                                        .SetBasePath(AppContext.BaseDirectory)
                                        .AddJsonFile("appsettings.Development.json");
                                _Configuration = builder.Build();
                                //Console.WriteLine("appsettings.Development.json");
#else
                            var builder = new ConfigurationBuilder()
                                .SetBasePath(AppContext.BaseDirectory)
                                .AddJsonFile("appsettings.json");
                            _Configuration = builder.Build();
                            Console.WriteLine("appsettings.json");

#endif


                            }
                        }
                    }

                    return _Configuration;
                }
                set
                {
                    _Configuration = value;
                }
            }
            public static string GetValues(params string[] sections)
            {
                try
                {
                    var val = string.Empty;
                    for (int i = 0; i < sections.Length; i++)
                    {
                        val += sections[i] + ":";
                    }

                    return Configuration[val.TrimEnd(':')]??"";
                }
                catch (Exception)
                {
                    return "";
                }

            }
        }
    }
}
