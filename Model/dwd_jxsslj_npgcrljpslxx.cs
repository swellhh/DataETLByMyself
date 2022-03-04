using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataETLViaHttp.Model
{
    //南排工程日累计排水量信息
    public class dwd_jxsslj_npgcrljpslxx
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string stnm { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string tm { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string psl { get; set; }

        public DateTime etl_timestamp { get; set; }

        public string etl_oper_type { get; set; }

        public string etl_update_time { get; set; }

    }
}
