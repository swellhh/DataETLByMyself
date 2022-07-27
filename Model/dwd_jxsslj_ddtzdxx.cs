using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataETLViaHttp.Model
{
    public class dwd_jxsslj_ddtzdxx
    {
        /// <summary>
        /// 编号
        /// </summary>
        [PrimaryKey]
        public string dd_id { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public string dd_year { get; set; }
        /// <summary>
        /// 期号
        /// </summary>
        public string dd_qishu { get; set; }
        /// <summary>
        /// 调度日期
        /// </summary>
        public DateTime dd_time { get; set; }
        /// <summary>
        /// 发送单位
        /// </summary>
        public string dd_name { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public string dd_stime { get; set; }
        /// <summary>
        /// 拟稿人
        /// </summary>
        public string dd_owen { get; set; }
        /// <summary>
        /// 签发人
        /// </summary>
        public string dd_qianfa { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string dd_tabtype { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string dd_note { get; set; }
        /// <summary>
        /// 抄送单位
        /// </summary>
        public string dd_chaosong { get; set; }
    }
}
