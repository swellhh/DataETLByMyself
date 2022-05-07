using Castle.Windsor;
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

        public override CronFormat _format => CronFormat.IncludeSeconds;

        public override async Task Execute(CancellationToken cancellationToken)
        {
            _logger.LogInformation("开始抓取嘉兴大数据平台接口数据");

            try
            {
                foreach (var item in _urls.Where(w => !w.isStop))
                {
                    var stra = _serviceAccessor(item.name);

                    //await stra.Exeute(item);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "定时任务出问题了");
            }

        }


        private async Task GetDataBehindSeveralDay(EntitiesUrl item)
        {

            var syncDate = _appSettings.GetValue<DateTime>("Application:SyncDate");
            
            try
            {

                var stra = _serviceAccessor(item.name);

                _logger.LogInformation("{0}开始同步{1}开始的数据", item.name, syncDate.ToString("yyyy年MM月dd日"));

                await stra.GetDataBehindSeveralDay(item, syncDate);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "数据初始化任务出问题了");
            }

        }

        public override async Task SetOnce()
        {
            foreach (var item in _urls.Where(w => !w.isStop))
            {
                _cacheClient.Set(item.name, 0L, DateTime.Now.AddHours(5));

                var stra = _serviceAccessor(item.name);

                await GetDataBehindSeveralDay(item);

                await stra.Exeute(item);

            }
        }
    }
}
