using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataETLViaHttp.Model
{
    //区域自动气象站站点信息_24069
    public class dwd_spt_qyzdqxzzdxx
    {
        [Ignore]
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [PrimaryKey]
        public string iiiii { get; set; }
        /// <summary>
        /// 海宁
        /// </summary>
        public string stationname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string nnnn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string eeeee { get; set; }
        /// <summary>
        /// 博儒桥村
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 嘉兴
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 海宁
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 嘉兴
        /// </summary>
        public string dsc_city { get; set; }
        /// <summary>
        /// 海宁
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
