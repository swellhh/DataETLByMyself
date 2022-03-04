using Cronos;
using DataETLViaHttp.Cache;
using DataETLViaHttp.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static DataETLViaHttp.Startup;

namespace DataETLViaHttp.BackgroundService
{
    public class DataETLRetryService
    {
        private readonly ILogger<DataETLRetryService> _logger;
        private readonly IConfiguration _appSettings;
        private Dictionary<string, int> _retryList;
        private readonly EntitiesUrls _urls;
        private readonly StrategyServiceResolver _serviceAccessor;
        public delegate Task<List<T>> RetryDelegate<T>(EntitiesUrl configEntity, Dictionary<string, object> kv = null); 
        private readonly ICacheClient _cache;
        private readonly int _retryCountLimit;

        public DataETLRetryService(StrategyServiceResolver serviceAccessor, ILoggerFactory loggerFac, IConfiguration appSettings, EntitiesUrls urls, ICacheClient cache)
        {
            _serviceAccessor = serviceAccessor;
            _logger = loggerFac.CreateLogger<DataETLRetryService>();
            _appSettings = appSettings;
            _urls = urls;
            _cache = cache;
            _retryCountLimit = _appSettings.GetValue<int>("Application:RetryCountLimit");
        }


        public Task RetryWhenExceptionThrow<T>(Dictionary<string,object> kv, RetryDelegate<T> func)
        {
            _retryList = _appSettings.GetPageIndexRecord();

            foreach (var item in _retryList.Where(w => w.Value != -1))
            {
                _cache.Increment(item.Key, 1);

                if (_cache.Get<long>(item.Key) < _retryCountLimit)
                {
                    _logger.LogInformation("{0}开始重试抓取嘉兴大数据平台接口数据,次数{1} \r\n", item.Key, _cache.Get<long>(item.Key));

                    var stra = _serviceAccessor(item.Key);
                    var url = _urls.Find(w => w.name == item.Key);
                    func(url, kv);


                }

            }


            return Task.CompletedTask;
        }

    }
}
