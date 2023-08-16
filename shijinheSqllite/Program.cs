using Autofac;
using Autofac.Extensions.DependencyInjection;
using Serilog;
using shijinheSqllite.AutoFacDI;
using shijinheSqllite.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Autofac.Core;
using static Core.Common.AppSettingManager.AppSetting;
using System.Configuration;
using Core.IServices.ISugarServices;
using Core.Common;
using StackExchange.Redis;
using shijinheSqllite.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//  Add Serilog
builder.Host.UseSerilog(
    (context, services, logger) =>
    {
        logger.ReadFrom.Configuration(context.Configuration)
        
        .ReadFrom.Services(services)
        .Enrich.FromLogContext() // 记录上下文
        .WriteTo.Console(); // 输出到控制台

      
    }
    );
// 使用Autofac工厂进行反射程序集注入
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutoFacRepository());
  
    //builder.RegisterModule<AutofacPropertityModuleReg>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// 注入扩展的SQLsugar
builder.Services.AddSqlsugarSetup();

//// 
//builder.Services.AddStackExchangeRedisCache(options => {
//    // 配置redis 数据库连接
//    options.Configuration = builder.Configuration.GetConnectionString("RedisConfig:Default:Connection");
//});
// 注入redis 数据库
builder.Services.AddSingleton<IConnectionMultiplexer>(await ConnectionMultiplexer.ConnectAsync(builder.Configuration.GetValue<string>("RedisConnection")??"127.0.0.1:6379"));

builder.Services.AddSwaggerGen();


builder.Services.AddControllers().AddNewtonsoftJson(options => {

    //忽略循环引用
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    //不使用驼峰样式的key
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    //设置时间格式
    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
    //忽略Model中为null的属性
    //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    //设置本地时间而非UTC时间
    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
    //添加Enum转string
    options.SerializerSettings.Converters.Add(new StringEnumConverter());

});
builder.Services.AddHostedService<SiteDataWriteSqlliteServcies>();
builder.Services.AddScoped<IScopedProcessingServices, DefaultScopedProcessingService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
