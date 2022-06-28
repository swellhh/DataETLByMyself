using Castle.Core;
using Castle.DynamicProxy;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
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
using System.Text;

namespace DataETLViaHttp
{
    public class Startup
    {
        public delegate IStrategy StrategyServiceResolver(string key);
        public delegate ISpecificWebClientUtil SpecificClientResolver(string key);


        public static void Init(IServiceCollection services, IConfiguration configuration)
        {
            RegisterInterfaceConfigInfo(services, configuration);
            RegisterSpecificWebClient(services);
            RegisterChooseWebClient(services);
            RegisterChooseStrategy(services);

            var provider = BuildDi(services, configuration);

            //CreateTables(provider);
        }

        private static IServiceProvider BuildDi(IServiceCollection services,IConfiguration config)
        {
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
                .AddHostedService<ThreadLoopService>()
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

        }
                
        private static void RegisterChooseStrategy(IServiceCollection services)
        {
            DependencyResolver.Register(new Aop.ComponentRegistration());

            services.AddSingleton<StrategyServiceResolver>(serviceProvider => key =>
            {

                switch (key)
                {
                    case "dwd_jxsqxj_station":
                        var target = serviceProvider.GetService<JxsqxjStationStrategy>();
                        return target;
                    case "dwd_spt_zjsjqx24xsljmylybxx":
                        return serviceProvider.GetService<SptZjsjqx24xsljmylybxxStrategy>();

                    case "dwd_spt_zjsjqxmylxsskxx":
                        return serviceProvider.GetService<SptZjsjqxmylxsskxxStrategy>();

                    case "dwd_spt_1kmwgjsybxx":
                        return serviceProvider.GetService<Spt1kmwgjsybxxStrategy>();

                    case "dwd_spt_5kmwgybxx":
                        return serviceProvider.GetService<Spt5kmwgybxxStrategy>();

                    case "dwd_spt_qxzdxx":
                        return serviceProvider.GetService<SptQxzdxxStrategy>();

                    case "dwd_jxsslj_npgqjcsjxx":
                        return serviceProvider.GetService<JxssljNpgqjcsjxxStrategy>();

                    case "dwd_jxsslj_npgcrljpslxx":
                        return serviceProvider.GetService<JxssljNpgcrljpslxxStrategy>();

                    case "dwd_spt_qyzdqxzzdxx":
                        return serviceProvider.GetService<SptQyzdqxzzdxxStrategy>();

                    case "dwd_spt_dmzdqxzgcxx":
                        return serviceProvider.GetService<DmzdqxzgcxxStrategy>();

                    case "dwd_spt_qyzdqxzgcxx":
                        return serviceProvider.GetService<SptQyzdqxzgcxxStrategy>();

                    default:
                        throw new KeyNotFoundException();
                }
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
