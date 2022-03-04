using System;
using System.Collections.Generic;
using System.Text;

namespace DataETLViaHttp.Model
{
    //省平台-地面自动气象站观测信息
    public class dwd_spt_dmzdqxzgcxx
    {
        /// <summary>
        /// 站号
        /// </summary>
        public string stationnum { get; set; }
        /// <summary>
        /// 观测时间
        /// </summary>
        public string observtimes { get; set; }
        /// <summary>
        /// 气温
        /// </summary>
        public string drybultemp { get; set; }
        /// <summary>
        /// 气压
        /// </summary>
        public string stationpress { get; set; }
        /// <summary>
        /// 湿度
        /// </summary>
        public string relhumidity { get; set; }
        /// <summary>
        /// 风向
        /// </summary>
        public string winddirect { get; set; }
        /// <summary>
        /// 风速
        /// </summary>
        public string windvelocity { get; set; }
        /// <summary>
        /// 水汽压
        /// </summary>
        public string vapourpress { get; set; }
        /// <summary>
        /// 降水量
        /// </summary>
        public string precipition { get; set; }
        /// <summary>
        /// 所属地市
        /// </summary>
        public string dsc_city { get; set; }
        /// <summary>
        /// 所需区/县
        /// </summary>
        public string dsc_adm_region { get; set; }
        /// <summary>
        /// 数源单位代码
        /// </summary>
        public string dsc_sydep_code { get; set; }
        /// <summary>
        /// 数源单位
        /// </summary>
        public string dsc_sydep_name { get; set; }
        /// <summary>
        /// 数据所属系统名称
        /// </summary>
        public string dsc_sydep_sys { get; set; }
        /// <summary>
        /// 数源单位表名
        /// </summary>
        public string dsc_sydep_tblname { get; set; }
        /// <summary>
        /// 唯一自增序列号
        /// </summary>
        public string dsc_biz_record_id { get; set; }
        /// <summary>
        /// I 插入 U 更新 D 删除
        /// </summary>
        public string dsc_biz_operation { get; set; }
        /// <summary>
        /// 源表数据同步时间
        /// </summary>
        public DateTime dsc_biz_timestamp { get; set; }
        /// <summary>
        /// 数据来源表名(清洗库或基础库 表名)
        /// </summary>
        public string dsc_datasr_tblname { get; set; }
        /// <summary>
        /// 业务主键MD5值（清洗增加）
        /// </summary>
        public string dsc_hash_unique { get; set; }
        /// <summary>
        /// 清洗时间（清洗增加）
        /// </summary>
        public DateTime dsc_clean_timestamp { get; set; }
        /// <summary>
        /// 地市仓数据入库时间
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
