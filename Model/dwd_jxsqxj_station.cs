using System;
using System.Collections.Generic;
using System.Text;

namespace DataETLViaHttp.Model
{
    //气象站台信息
    public class dwd_jxsqxj_station
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 曹桥街道
        /// </summary>
        public string station_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string station_key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ldsjc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string jcsjc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string longitude { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string latitude { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string etl_oper_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime etl_timestamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string etl_update_time { get; set; }
    }
}
