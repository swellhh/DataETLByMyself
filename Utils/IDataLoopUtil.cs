using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataETLViaHttp.Utils
{
    public interface IDataLoopUtil
    {
        /// <summary>
        /// 不需要重试
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="kv"></param>
        /// <returns></returns>
        Task<List<T>> GetDataFromInters<T>(string url, Dictionary<string, object> kv);

        /// <summary>
        /// 不需要重试
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="kv"></param>
        /// <returns></returns>
        Task<List<T>> GetDataFromInters<T>(string url);

        /// <summary>
        /// 需要重试
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configEntity"></param>
        /// <param name="kv"></param>
        /// 
        /// <returns></returns>
        Task<List<T>> GetDataFromInters<T>(EntitiesUrl configEntity, Dictionary<string, object> kv);

        /// <summary>
        /// 不需要重试且带有一万条记录直接插入数据库的功能
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="retryName"></param>
        /// <param name="kv"></param>
        /// <returns></returns>
        Task<List<T>> GetDataFromInters<T>(string url, Action<List<T>> insertAct, Dictionary<string, object> kv);

        /// <summary>
        /// 需要重试且带有一万条记录直接插入数据库的功能
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="retryName"></param>
        /// <param name="kv"></param>
        /// <returns></returns>
        Task<List<T>> GetDataFromInters<T>(Action<List<T>> insertAct, EntitiesUrl configEntity, Dictionary<string, object> kv);

    }
}
