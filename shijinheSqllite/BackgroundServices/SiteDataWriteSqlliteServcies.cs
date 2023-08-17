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
    public class SiteDataWriteSqlliteServcies : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider;
        


        public SiteDataWriteSqlliteServcies( IServiceProvider serviceProvider)
        {


            _serviceProvider = serviceProvider;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            Log.Information("开始");
           // await Console.Out.WriteLineAsync("开始");
            using (IServiceScope scope = _serviceProvider.CreateScope()) {
                 
                IScopedProcessingServices scopedProcessingService =
                 scope.ServiceProvider.GetRequiredService<IScopedProcessingServices>();
                await scopedProcessingService.DoWorkAsync(stoppingToken);
            }

          



        }


      


    }
}
