using Cronos;
using DataETLViaHttp.Cache;
using DataETLViaHttp.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static DataETLViaHttp.Startup;

namespace DataETLViaHttp.BackgroundService
{
    public class DataETLService : CronScheduleBase
    {
        private readonly ILogger<DataETLService> _logger;
        private readonly IConfiguration _appSettings;
        private readonly EntitiesUrls _urls;
        private readonly StrategyServiceResolver _serviceAccessor;
        private readonly ICacheClient _cacheClient;

        public DataETLService(StrategyServiceResolver serviceAccessor, ILoggerFactory loggerFac,IConfiguration appSettings, EntitiesUrls urls, ICacheClient cacheClient) : base(loggerFac, appSettings)
        {
            _serviceAccessor = serviceAccessor;
            _logger = loggerFac.CreateLogger<DataETLService>();
            _appSettings = appSettings;
            _urls = urls;
            _cacheClient = cacheClient;
        }

        public override string _cronExpression => _appSettings.GetValue<string>("HostService:CronExpression");

        public override CronFormat _format => CronFormat.Standard;

        public override async Task Execute(CancellationToken cancellationToken)
        {
            _logger.LogInformation("开始抓取嘉兴大数据平台接口数据");

            try
            {
                foreach (var item in _urls.Where(w => !w.isStop))
                {
                    _cacheClient.Replace(item.name, 0);

                    var stra = _serviceAccessor(item.name);

                    await stra.Exeute(item);
                }
                

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "定时任务出问题了");
            }

        }


        private async Task GetDataBehindSeveralDay(EntitiesUrl item)
        {

            var days = _appSettings.GetValue<DateTime>("Application:SyncDate");
            _logger.LogInformation("{0}开始同步{1}开始的数据", item.name, days.ToString("yyyy年MM月dd日"));

            try
            {
                _cacheClient.Replace(item.name, 0);

                var stra = _serviceAccessor(item.name);

                await stra.GetDataBehindSeveralDay(item, days);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{0}{1}天数据初始化任务出问题了", item.name, days.ToString().Replace("-1", ""));
            }

        }

        public override async Task SetOnce()
        {
            EntitiesUrl obj = null;
            try
            {
                _logger.LogInformation("初始化抓取嘉兴大数据平台接口数据开始");

                //Parallel.ForEach(_urls, async (item, state) =>
                //{
                //    await GetDataBehindSeveralDay(item);
                //});

                foreach (var item in _urls.Where(w => !w.isStop))
                {
                    obj = item;
                    await GetDataBehindSeveralDay(item);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化数据库有问题");
            }


            _logger.LogInformation("初始化抓取嘉兴大数据平台接口数据结束");

        }
    }
}
