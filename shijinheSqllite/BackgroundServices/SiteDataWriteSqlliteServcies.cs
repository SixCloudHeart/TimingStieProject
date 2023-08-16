using Core.Common.XML;
using Core.IRepository.ISugarRepository;
using Core.IServices;
using Core.IServices.ISugarServices;
using Core.Repository.SqlSugarRepository;
using Core.Services;
using StackExchange.Redis;

namespace shijinheSqllite.BackgroundServices
{
    public class SiteDataWriteSqlliteServcies : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider;
        readonly ILogger<SiteDataWriteSqlliteServcies> _logger;


        public SiteDataWriteSqlliteServcies(ILogger<SiteDataWriteSqlliteServcies> logger, IServiceProvider serviceProvider)
        {


            _logger = logger;
            _serviceProvider = serviceProvider;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Console.Out.WriteLineAsync("开始");

            using (IServiceScope scope = _serviceProvider.CreateScope()) {
                 
                IScopedProcessingServices scopedProcessingService =
                 scope.ServiceProvider.GetRequiredService<IScopedProcessingServices>();
                await scopedProcessingService.DoWorkAsync(stoppingToken);
            }

              
            _logger.LogInformation("结束");
         

        }


      


    }
}
