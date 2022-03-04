using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataETLViaHttp.Model
{
    //省平台-浙江省级区县面雨量小时实况信息
    public class dwd_spt_zjsjqxmylxsskxx
    {
        /// <summary>
        /// 
        /// </summary>
        [Ignore]
        public string id { get; set; }
        /// <summary>
        /// 嘉兴
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 平湖
        /// </summary>
        public string county { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string observtimes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 嘉兴市
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
        /// 浙江省气象信息综合分析和处理系统（MICAPS）
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
