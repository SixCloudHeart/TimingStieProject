using Core.Common;
using SqlSugar;

namespace shijinheSqllite.Extensions
{
    public static class SqlsugarSetup
    {

        public static void AddSqlsugarSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            // 把多个连接对象注入服务，这里必须采用Scope，因为有事务操作
            services.AddScoped<ISqlSugarClient>(o =>
            {
                try
                {

                    SqlSugarClient db = new SqlSugarClient(
                        new ConnectionConfig()
                        {
                            ConnectionString = GlobalSettings.MainDB,
                            DbType = DbType.Sqlite,//设置数据库类型
                            IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                            InitKeyType = InitKeyType.Attribute,
                        });
                    //db.Aop.OnLogExecuted
                    db.Aop.OnLogExecuting = (sql, pars) =>
                    {
                        foreach (var item in pars)
                        {
                            sql = sql.Replace(item.ParameterName.ToString(), $"'{item.Value?.ToString()}'");
                        }
                        Console.WriteLine("【SQL语句】：" + sql);
                        //Console.WriteLine();
                        //if (GlobalSettings.LogAOP)
                        //{
                        //    LogLock.OutSql2Log("SqlLog", new string[] { GetParas(pars), "【SQL语句】：" + sql });
                        //}
                        var sqlStr = sql;
                        var test = db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value));
                        foreach (var item in pars)
                        {
                            Console.WriteLine($"{item.ParameterName}:{item.Value}");
                            sqlStr = sqlStr.Replace(item.ParameterName, "'" + item?.Value?.ToString() + "'");
                        }

                    };
                    db.Aop.OnError = (exp) =>
                    {
                        foreach (var item in exp.Parametres as SugarParameter[])
                        {
                            exp.Sql = exp.Sql.Replace(item.ParameterName.ToString(), $"'{item.Value?.ToString()}'");
                        }
                        //Console.WriteLine("【SQL语句】：" + sql);
                        //Console.WriteLine("SQL：" + GetParas(exp.Parametres) + "【SQL语句】：" + exp.Sql);
                        Console.WriteLine();
                    };
                    db.Aop.OnLogExecuted = (sql, pars) =>
                    {
                        foreach (var item in pars)
                        {
                            sql = sql.Replace(item.ParameterName.ToString(), $"'{item.Value?.ToString()}'");
                        }
                        string p = string.Join(",", pars?.Select(it => it.ParameterName + ":" + it.Value));
                        //SqlLogHandler handler = new SqlLogHandler(sql, DateTime.Now, db.Ado.SqlExecutionTime.TotalSeconds, p);
                        //handler.WriteLog();
                    };
                    return db;
                }
                catch (Exception ex)
                {
                    throw new Exception("连接数据库出错，请检查您的连接字符串和网络。 ex:" + ex.Message);
                }

            });
        }
        private static string GetParas(SugarParameter[] pars)
        {
            string key = "【SQL参数】：";
            foreach (var param in pars)
            {
                key += $"{param.ParameterName}:{param.Value}\n";
            }
            return key;
        }
    }
}
