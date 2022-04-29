using Castle.Windsor;
using DataETLViaHttp.BackgroundService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static DataETLViaHttp.Startup;

namespace DataETLViaHttp.Utils
{
    public class DataLoopUtil : IDataLoopUtil
    {
        private readonly ILogger<DataLoopUtil> _logger;
        private readonly IConfiguration _appSettings;
        private readonly SpecificClientResolver _clientChoose;
        private readonly int _pageSize;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ISpecificWebClientUtil _defaultClient;

        public DataLoopUtil(ILogger<DataLoopUtil> logger, IConfiguration appSettings, SpecificClientResolver clientChoose, IHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _appSettings = appSettings;
            _clientChoose = clientChoose;
            _pageSize = _appSettings.GetValue<int>("Application:SyncBatch");
            _hostEnvironment = hostEnvironment;
            _defaultClient = clientChoose("Yjtyxm");
        }

        private string BuildParamJson(Dictionary<string, object> kvs,int pageIndex)
        {
            kvs.TryAdd("pageSize", _pageSize);
            kvs.TryAdd("pageIndex", pageIndex);
            kvs["pageIndex"] = pageIndex;

            return JsonConvert.SerializeObject(kvs);
        }


        public async Task<List<T>> GetDataFromInters<T>(string url)
        {
            Random random = new Random();

            int pageIndex = 1, total;
            List<T> res = new List<T>();

            do
            {
                _logger.LogInformation("正在获取数据,页码为{0}", pageIndex);
                var inputJson = BuildParamJson(new Dictionary<string, object>(), pageIndex);
                var list = await _defaultClient.GetDataList<T>(url
                    , JsonConvert.DeserializeObject<JObject>(inputJson));
                list = list == null ? new List<T>() : list;
                total = list.Count;
                res.AddRange(list);


                pageIndex++;

                if (_hostEnvironment.IsProduction())
                {
                    Thread.Sleep(TimeSpan.FromSeconds(random.Next(10, 20)));
                }

            } while (_pageSize == total && pageIndex < 200);


            return res;
        }

        public async Task<List<T>> GetDataFromInters<T>(string url, Dictionary<string, object> kv)
        {
            Random random = new Random();

            int pageIndex = 1, total;
            List<T> res = new List<T>();

            do
            {
                _logger.LogInformation("正在获取数据,页码为{0}", pageIndex);
                var inputJson = BuildParamJson(new Dictionary<string, object>(), pageIndex);
                var list = await _defaultClient.GetDataList<T>(url
                    , JsonConvert.DeserializeObject<JObject>(inputJson));
                list = list == null ? new List<T>() : list;
                total = list.Count;
                res.AddRange(list);


                pageIndex++;

                if (_hostEnvironment.IsProduction())
                {
                    Thread.Sleep(TimeSpan.FromSeconds(random.Next(10, 20)));
                }

            } while (_pageSize == total && pageIndex < 200);


            return res;
        }

        public async Task<List<T>> GetDataFromInters<T>(EntitiesUrl configEntity, Dictionary<string, object> kv = null)
        {
            int pageIndex =1, total;
            List<T> res = new List<T>();
            kv = kv ?? new Dictionary<string, object>();

            do
            {
                _logger.LogInformation("正在获取数据,页码为{0}", pageIndex);
                var inputJson = BuildParamJson(kv, pageIndex);
                var list = await _clientChoose(configEntity.client).GetDataList<T>(configEntity.url
                    , JsonConvert.DeserializeObject<JObject>(inputJson));
                list = list == null ? new List<T>() : list;
                total = list.Count;
                res.AddRange(list);


                pageIndex++;

            } while (_pageSize == total && pageIndex < 200);


            return res;
        }


        public async Task<List<T>> GetDataFromInters<T>(string url, Action<List<T>> insertAct, Dictionary<string, object> kv = null)
        {
            Random random = new Random();

            int pageIndex = 1, total;
            List<T> res = new List<T>();
            do
            {
                _logger.LogInformation("正在获取数据,页码为{0}", pageIndex);
                var inputJson = BuildParamJson(kv, pageIndex);
                var list = await _defaultClient.GetDataList<T>(url
                    , JsonConvert.DeserializeObject<JObject>(inputJson));
                list = list == null ? new List<T>() : list;
                total = list.Count;
                res.AddRange(list);


                pageIndex++;

                if (res.Count == 10000)
                {
                    insertAct(res);
                    res = new List<T>();
                }

                if (_hostEnvironment.IsProduction())
                {
                    Thread.Sleep(TimeSpan.FromSeconds(random.Next(0, 10)));
                }

            } while (_pageSize == total && pageIndex < 200);

            return res;
        }


        public async Task<List<T>> GetDataFromInters<T>(Action<List<T>> insertAct, EntitiesUrl configEntity, Dictionary<string, object> kv = null)
        {
            Random random = new Random();

            int pageIndex = 1, total;
            List<T> res = new List<T>();
            kv = kv ?? new Dictionary<string, object>();

            do
            {
                var inputJson = BuildParamJson(kv, pageIndex);
                var list = await _clientChoose(configEntity.client).GetDataList<T>(configEntity.url
                    , JsonConvert.DeserializeObject<JObject>(inputJson));
                list = list == null ? new List<T>() : list;
                total = list.Count;
                res.AddRange(list);


                pageIndex++;

                if (res.Count == 10000)
                {
                    insertAct(res);
                    res = new List<T>();
                }

                if (_hostEnvironment.IsProduction())
                {
                    Thread.Sleep(TimeSpan.FromSeconds(random.Next(0, 10)));
                }

            } while (_pageSize == total && pageIndex < 200);


            return res;
        }

    }
}
