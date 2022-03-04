using DataETLViaHttp.BackgroundService;
using DataETLViaHttp.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;


namespace DataETLViaHttp
{
    public class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureConfiguration()
                .ConfigureSystemServices();
        }
    }

    internal static class ServiceExtension
    {
        public static IHostBuilder ConfigureConfiguration(this IHostBuilder host)
        {
            return host.ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;

                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            });
        }

        public static IHostBuilder ConfigureSystemServices(this IHostBuilder host)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();

            return host.ConfigureServices((hostContext, services) =>
            {
                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddNLog(hostContext.Configuration);
                    NLog.LogManager.Configuration = new NLogLoggingConfiguration(hostContext.Configuration.GetSection("NLog"));

                });

                try
                {
                    Startup.Init(services,hostContext.Configuration);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Stopped program because of exception");
                    throw;
                }

            });
        }

    }

}
