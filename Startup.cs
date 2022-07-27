using Castle.Core;
using Castle.MicroKernel.Registration;
using DataETLViaHttp.Aop;
using DataETLViaHttp.BackgroundService;
using DataETLViaHttp.Cache;
using DataETLViaHttp.Model;
using DataETLViaHttp.Strategy;
using DataETLViaHttp.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Linq;
using System.Collections.Concurrent;

namespace DataETLViaHttp
{
    public class Startup
    {
        public delegate IStrategy StrategyServiceResolver(string key);
        public delegate ISpecificWebClientUtil SpecificClientResolver(string key);
        private static ConcurrentDictionary<string, IStrategy> _dic = null;
        private const string _strategySign = "NAME";

        public static void Init(IServiceCollection services, IConfiguration configuration)
        {
            RegisterInterfaceConfigInfo(services, configuration);
            RegisterSpecificWebClient(services);
            RegisterChooseWebClient(services);
            RegisterChooseStrategy(services);

            var provider = BuildDi(services, configuration);

            CreateTables(provider);
        }

        private static IServiceProvider BuildDi(IServiceCollection services,IConfiguration config)
        {
            DependencyResolver.Register(new Aop.ComponentRegistration());

            var dbHost = config.GetValue<string>("Application:AimDatabaseHost");
            var dbPort = config.GetValue<string>("Application:AimDatabasePort");
            var dbUser = SecurityHelper.DecryptDES(config.GetValue<string>("Application:AimDatabaseUser"));
            var dbPassword = SecurityHelper.DecryptDES(config.GetValue<string>("Application:AimDatabasePassword"));
            var dataBase = config.GetValue<string>("Application:AimDatabase");
            var connectionString = $"Server={dbHost};Port={dbPort};Uid={dbUser};Pwd={dbPassword};Database={dataBase};SslMode=None;CharSet=utf8;";
            var dbFactory = new OrmLiteConnectionFactory(
                connectionString,
                MySqlDialect.Provider);


            var proxy = new WebProxy(new Uri("http://127.0.0.1:8888"));
            services.AddHttpClient("proxy").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                Proxy = proxy
            });
            services
                .AddHostedService<DataETLService>()
                .AddSingleton<IDataLoopUtil,DataLoopUtil>()
                .AddSingleton<DataETLRetryInterceptor>()
                .AddSingleton<ICacheClient,MemoryCacheClient>()
                .AddSingleton<IDbConnectionFactory>(dbFactory);

            return services.BuildServiceProvider();
        }

        private static void CreateTables(IServiceProvider container)
        {
            var logger = container.GetService<ILogger<Startup>>();
            var dbFactory = container.GetService<IDbConnectionFactory>();

            using var db = dbFactory.Open();

            logger.LogInformation("数据库连接成功\n");

            db.CreateTableIfNotExists<dwd_jxsqxj_station>();

            db.CreateTableIfNotExists<dwd_spt_zjsjqx24xsljmylybxx>();
            db.CreateTableIfNotExists<dwd_spt_zjsjqxmylxsskxx>();
            db.CreateTableIfNotExists<dwd_spt_1kmwgjsybxx>();
            db.CreateTableIfNotExists<dwd_spt_5kmwgybxx>();
            db.CreateTableIfNotExists<dwd_spt_qxzdxx>();
            db.CreateTableIfNotExists<dwd_jxsslj_npgqjcsjxx>();
            db.CreateTableIfNotExists<dwd_jxsslj_npgcrljpslxx>();
            db.CreateTableIfNotExists<dwd_spt_qyzdqxzzdxx>();
            db.CreateTableIfNotExists<dwd_jxsqxj_observations>();
            db.CreateTableIfNotExists<dwd_jxsslj_hdswxx>();
            db.CreateTableIfNotExists<dwd_jxsslj_ddtzdxx>();

        }

        private static void GetChooseStrategyDic(IServiceProvider container)
        {
            _dic = new ConcurrentDictionary<string, IStrategy>();

            var types = typeof(BaseStrategy).GetTypeInfo().Assembly.GetTypes().Where(x => typeof(IStrategy).IsAssignableFrom(x) && !x.IsInterface);

            foreach (var type in types)
            {
                var field = type.GetField(_strategySign);

                _dic.TryAdd(field.GetRawConstantValue() as string, container.GetService(type) as IStrategy);

            }
        }


        private static void RegisterChooseStrategy(IServiceCollection services)
        {

            services.AddSingleton<StrategyServiceResolver>(serviceProvider => key =>
            {
                if (_dic == null)
                {
                    GetChooseStrategyDic(serviceProvider);
                }
                return _dic.ContainsKey(key) ? _dic[key] : null;
            });

        }

        private static void RegisterSpecificWebClient(IServiceCollection services)
        {
            services.AddSingleton<YjfxftWebClientUtil>();
            services.AddSingleton<YjtyxmWebClientUtil>();
        }

        private static void RegisterChooseWebClient(IServiceCollection services)
        {
            services.AddSingleton<SpecificClientResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "Yjfxft":
                        return serviceProvider.GetService<YjfxftWebClientUtil>();

                    case "Yjtyxm":
                        return serviceProvider.GetService<YjtyxmWebClientUtil>();

                    default:
                        throw new KeyNotFoundException();
                }
            });

        }



        private static void RegisterInterfaceConfigInfo(IServiceCollection services, IConfiguration configuration)
        {
            var exceptStFilePath = configuration.GetValue<string>("Application:StationExcept");
            var exceptStJsonStr = File.ReadAllText(exceptStFilePath);
            var stationExcepts = JsonConvert.DeserializeObject<StationExcepts>(exceptStJsonStr);
            services.AddSingleton(stationExcepts);


            var entitiesUrlFilePath = configuration.GetValue<string>("DataUrl:EntitiesPath");
            var entitiesUrlJsonStr = File.ReadAllText(entitiesUrlFilePath);
            var entitiesUrl = JsonConvert.DeserializeObject<EntitiesUrls>(entitiesUrlJsonStr);
            services.AddSingleton(entitiesUrl);

        }

    }

}
