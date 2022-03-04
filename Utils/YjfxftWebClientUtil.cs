using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace DataETLViaHttp.Utils
{
    public class YjfxftWebClientUtil : BaseSpecificWebClientUtil,ISpecificWebClientUtil
    {

        public override string AccessKey { get; set; }
        public override string SecretKey { get; set; }
        public override string ProjectKey { get; set; }
        public override string baseIp { get; set; }

        public YjfxftWebClientUtil(ILogger<YjfxftWebClientUtil> logger, IHttpClientFactory httpClientFactory, IConfiguration conf) : base(logger, httpClientFactory)
        {
            AccessKey = conf.GetValue<string>("Encryption:Yjfxft:AK");
            SecretKey = conf.GetValue<string>("Encryption:Yjfxft:SK");
            ProjectKey = conf.GetValue<string>("Encryption:Yjfxft:ProjectKey");
            baseIp = conf.GetValue<string>("DataUrl:baseIp");

        }
    }
}
