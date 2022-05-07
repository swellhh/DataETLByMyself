using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DataETLViaHttp.Utils
{
    public abstract class BaseSpecificWebClientUtil
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger _logger;
        public abstract string AccessKey { get; set; }
        public abstract string SecretKey { get; set; }
        public abstract string ProjectKey { get; set; }
        public abstract string baseIp { get; set; }

        public BaseSpecificWebClientUtil(ILogger logger, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IList> GetDataList(Type type, string url, JObject paramMap)
        {
            var res = default(IList);
            try
            {
                var timestmp = ((DateTime.Now.Ticks - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0)).Ticks) / 1000).ToString();
                var sign = EncryptionUtil.GenerateSign(SecretKey, paramMap, url, timestmp);
                using var client = _httpClientFactory.CreateClient("proxy");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                _logger.LogInformation("url:{0}", (baseIp + url + "?" + EncryptionUtil.TransJsonToSpecific(paramMap)));

                using (var request = new HttpRequestMessage())
                {
                    request.Headers.TryAddWithoutValidation("Authorization", AccessKey + ":" + sign);
                    request.Headers.TryAddWithoutValidation("Date", timestmp.ToString());
                    request.Headers.TryAddWithoutValidation("ProjectKey", ProjectKey);
                    request.Headers.TryAddWithoutValidation("Content-Type", "application/json");


                    request.Method = HttpMethod.Get;
                    request.RequestUri = new Uri(baseIp + url + "?" + EncryptionUtil.TransJsonToSpecific(paramMap));
                    request.Content = new StringContent("", System.Text.Encoding.UTF8);
                    request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");

                    var stringTask = await client.SendAsync(request);

                    if (stringTask.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("请求成功");

                        var resStr = await stringTask.Content.ReadAsStringAsync();

                        var payload = JsonConvert.DeserializeObject<JObject>(resStr);

                        _logger.LogInformation("数据条数:{0}", payload["data"]["total"]);

                        res = payload["data"]["result"].ToObject(type) as IList;
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "请求失败");

            }


            return res;
        }

        public async Task<List<T>> GetDataList<T>(string url, JObject paramMap)
        {
            var res = default(List<T>);
            JObject payload = null;

            var timestmp = ((DateTime.Now.Ticks - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0)).Ticks) / 1000).ToString();
            var sign = EncryptionUtil.GenerateSign(SecretKey, paramMap, url, timestmp);
            using var client = _httpClientFactory.CreateClient("proxy");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            _logger.LogInformation("url:{0}", (baseIp + url + "?" + EncryptionUtil.TransJsonToSpecific(paramMap)));

            HttpResponseMessage stringTask;

            using (var request = new HttpRequestMessage())
            {
                request.Headers.TryAddWithoutValidation("Authorization", AccessKey + ":" + sign);
                request.Headers.TryAddWithoutValidation("Date", timestmp.ToString());
                request.Headers.TryAddWithoutValidation("ProjectKey", ProjectKey);
                request.Headers.TryAddWithoutValidation("Content-Type", "application/json");


                request.Method = HttpMethod.Get;
                request.RequestUri = new Uri(baseIp + url + "?" + EncryptionUtil.TransJsonToSpecific(paramMap));
                request.Content = new StringContent("", System.Text.Encoding.UTF8);
                request.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");


                stringTask = await client.SendAsync(request);
            }

            if (stringTask.IsSuccessStatusCode)
            {
                _logger.LogInformation("请求成功");

                var resStr = await stringTask.Content.ReadAsStringAsync();

                payload = JsonConvert.DeserializeObject<JObject>(resStr);

                _logger.LogInformation("数据条数:{0}", payload["data"]["total"]);

                res = payload["data"]["result"].ToObject<List<T>>();
            }
            else
            {
                throw new Exception("请求失败，无数据返回");
            }

            return res;
        }
    }
}
