using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataETLViaHttp.Model
{
    /// <summary>
    /// 市级雨量表
    /// </summary>
    public class dwd_jxsqxj_observations
    {
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        public string id { get; set; }
        /// <summary>
        /// 台站名
        /// </summary>
        public string station { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime observ_time { get; set; }
        /// <summary>
        /// 风向
        /// </summary>
        public string wind_direction { get; set; }
        /// <summary>
        /// 风速
        /// </summary>
        public string wind_speed { get; set; }
        /// <summary>
        /// 降水
        /// </summary>
        public string rain { get; set; }
        /// <summary>
        /// 空气温度
        /// </summary>
        public string air_temp { get; set; }
        /// <summary>
        /// 相对湿度
        /// </summary>
        public string humidity { get; set; }
        /// <summary>
        /// 本站气压
        /// </summary>
        public string pressure { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ldsjc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string jcsjc { get; set; }
    }
}
