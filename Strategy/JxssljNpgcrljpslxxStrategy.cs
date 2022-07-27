using Castle.Windsor;
using DataETLViaHttp.Model;
using DataETLViaHttp.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataETLViaHttp.Strategy
{
    public class JxssljNpgcrljpslxxStrategy : BaseStrategy,IStrategy
    {
        public const string NAME = "dwd_jxsslj_npgcrljpslxx";

        private readonly ILogger<JxssljNpgcrljpslxxStrategy> _logger;

        public JxssljNpgcrljpslxxStrategy(ILoggerFactory loggerFac, IDbConnectionFactory dbFactory, IConfiguration appSettings, IDataLoopUtil loopUtil) : base(dbFactory, appSettings, loopUtil)
        {
            _logger = loggerFac.CreateLogger<JxssljNpgcrljpslxxStrategy>();
        }

        public virtual async Task Exeute(EntitiesUrl configEntity)
        {

            using var db = _dbFactory.OpenDbConnection();

            var max = db.Scalar<DateTime>(db.From<dwd_jxsslj_npgcrljpslxx>().Select(w => new { TM = Sql.Max("TM") }));
            var dwd_jxsslj_npgcrljpslxxs = await _loopUtil.GetDataFromInters<dwd_jxsslj_npgcrljpslxx>(configEntity,
                new Dictionary<string, object> { { "TM", max.ToString("yyyy-MM-dd HH:mm:ss") } });

            dwd_jxsslj_npgcrljpslxxs = dwd_jxsslj_npgcrljpslxxs.GroupBy(w => new { w.id,w.tm, w.etl_oper_type }).Select(w => w.FirstOrDefault()).ToList();
            var tableData = db.Select(db.From<dwd_jxsslj_npgcrljpslxx>().Select(w => new { w.id, w.tm, w.etl_oper_type }));
            dwd_jxsslj_npgcrljpslxxs.RemoveAll(w => tableData.FindAll(x => x.tm == w.tm && x.id == w.id && x.etl_oper_type == w.etl_oper_type).Count > 0);

            await db.InsertAllAsync(dwd_jxsslj_npgcrljpslxxs);

            _logger.LogInformation("{0}实时数据导入策略成功\n\r", "南排工程日累计排水量信息");
        }

        public virtual async Task GetDataBehindSeveralDay(EntitiesUrl configEntity, DateTime date)
        {
            using var db = _dbFactory.OpenDbConnection();

            if (db.Count<dwd_jxsslj_npgcrljpslxx>() == 0)
            {
                var dwd_jxsslj_npgcrljpslxxs = await _loopUtil.GetDataFromInters<dwd_jxsslj_npgcrljpslxx>(configEntity
                                , new Dictionary<string, object> { { "TM", date.ToString("yyyy-MM-dd HH:mm:ss") } }); 
                await db.InsertAllAsync(dwd_jxsslj_npgcrljpslxxs);
                
            }

            

        }
    }

}
