using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DataETLViaHttp.Utils
{
    public class YjtyxmWebClientUtil : BaseSpecificWebClientUtil, ISpecificWebClientUtil
    {
        public override string AccessKey { get; set; }
        public override string SecretKey { get; set; }
        public override string ProjectKey { get; set; }
        public override string baseIp { get; set; }

        public YjtyxmWebClientUtil(ILogger<YjtyxmWebClientUtil> logger, IHttpClientFactory httpClientFactory, IConfiguration conf) : base(logger, httpClientFactory)
        {
            AccessKey = conf.GetValue<string>("Encryption:Yjtyxm:AK");
            SecretKey = conf.GetValue<string>("Encryption:Yjtyxm:SK");
            ProjectKey = conf.GetValue<string>("Encryption:Yjtyxm:ProjectKey");
            baseIp = conf.GetValue<string>("DataUrl:baseIp");
        }

        
    }
}
