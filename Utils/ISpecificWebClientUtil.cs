using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataETLViaHttp.Utils
{
    public interface ISpecificWebClientUtil
    {
        Task<IList> GetDataList(Type type, string url, JObject paramMap);

        Task<List<T>> GetDataList<T>(string url, JObject paramMap);
    }
}
