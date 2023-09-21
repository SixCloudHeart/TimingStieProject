using Core.Common.XML;
using Core.IRepository.ISugarRepository;
using Core.IServices;
using Core.IServices.ISugarServices;
using Core.Repository.SqlSugarRepository;
using Core.Services;
using Serilog;
using StackExchange.Redis;

namespace shijinheSqllite.BackgroundServices
{
    /// <summary>
    ///  数据写入Sqllite服务实现
    /// </summary>
    public class SiteDataWriteSqlliteServcies : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider;
        


        public SiteDataWriteSqlliteServcies( IServiceProvider serviceProvider)
        {


            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 定时执行
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            
            await Console.Out.WriteLineAsync("开始");
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {

                IScopedProcessingServices scopedProcessingService =
                 scope.ServiceProvider.GetRequiredService<IScopedProcessingServices>();
                await scopedProcessingService.DoWorkAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken);
            }

        



        }


      


    }
}
