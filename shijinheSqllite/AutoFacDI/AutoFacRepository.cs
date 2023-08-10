using Autofac;
using Autofac.Extras.DynamicProxy;
using System.Reflection;

namespace shijinheSqllite.AutoFacDI
{
    public class AutoFacRepository: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;
            var servicesDllFile = Path.Combine(basePath, "Core.Repository.dll");
            var repositoryDllFile = Path.Combine(basePath, "Core.Services.dll");
            if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
            {
                var msg = "Core.IRepository.Core.IServices.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。";
                throw new Exception(msg);
            }


            var cacheType = new List<Type>();
            //数据库事务测试
            //if (GlobalSettings.DBTran)
            //{
            //    builder.RegisterType<DBTranAOP>();
            //    cacheType.Add(typeof(DBTranAOP));
            //}
            // 获取 Service.dll 程序集服务，并注册
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblysServices)
                      .AsImplementedInterfaces()
                      .InstancePerDependency()
                          .EnableInterfaceInterceptors()
                      .InterceptedBy(cacheType.ToArray());//允许将拦截器服务的列表分配给注册;//引用Autofac.Extras.DynamicProxy;


            // 获取 Repository.dll 程序集服务，并注册
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository)
                   .AsImplementedInterfaces()
                   .InstancePerDependency()
                    .EnableInterfaceInterceptors();


        }
    }
}
