using System;
using System.Collections.Generic;
using System.Text;

namespace DataETLViaHttp.Model
{
    public class dwd_jxsqxj_warning
    {

        /// <summary>
        /// 预警城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string jcsjc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string stg_time { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string num { get; set; }
        /// <summary>
        /// 预警操作类型
        /// </summary>
        public string operation { get; set; }
        /// <summary>
        /// 预警发布时间
        /// </summary>
        public DateTime release_time { get; set; }
        /// <summary>
        /// 签发人
        /// </summary>
        public string signer { get; set; }
        /// <summary>
        /// 预警等级
        /// </summary>
        public string warningcolor { get; set; }
        /// <summary>
        /// 预警内容
        /// </summary>
        public string warningcontent { get; set; }
        /// <summary>
        /// 预警名称
        /// </summary>
        public string warningname { get; set; }
        /// <summary>
        /// 预警年份
        /// </summary>
        public string warn_year { get; set; }
    }


}
