using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataETLViaHttp.Model
{
    //省平台-5公里网格预报信息
    public class dwd_spt_5kmwgybxx
    {
        /// <summary>
        /// 
        /// </summary>
        [Ignore]
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ybjzsj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sjkd { get; set; }
        /// <summary>
        /// 降水
        /// </summary>
        public string ybys { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sjgs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ybgxsj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dsc_city { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dsc_adm_region { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dsc_sydep_code { get; set; }
        /// <summary>
        /// 省气象局
        /// </summary>
        public string dsc_sydep_name { get; set; }
        /// <summary>
        /// 无
        /// </summary>
        public string dsc_sydep_sys { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dsc_sydep_tblname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dsc_biz_record_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dsc_biz_operation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime dsc_biz_timestamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dsc_datasr_tblname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string dsc_hash_unique { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime dsc_clean_timestamp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime dsc_dw_rksj { get; set; }
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
