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


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//  Add Serilog
builder.Host.UseSerilog(
    (context, services, logger) =>
    {
        logger.ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext() // ��¼������
        .WriteTo.Console(); // ���������̨
    }
    );
// ʹ��Autofac�������з������ע��
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutoFacRepository());
  
    //builder.RegisterModule<AutofacPropertityModuleReg>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// ע����չ��SQLsugar
builder.Services.AddSqlsugarSetup();




builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddNewtonsoftJson(options => {

    //����ѭ������
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    //��ʹ���շ���ʽ��key
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    //����ʱ���ʽ
    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
    //����Model��Ϊnull������
    //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    //���ñ���ʱ�����UTCʱ��
    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
    //���Enumתstring
    options.SerializerSettings.Converters.Add(new StringEnumConverter());

});

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
