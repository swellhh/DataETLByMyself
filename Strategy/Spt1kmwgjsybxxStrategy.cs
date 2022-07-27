using DataETLViaHttp.Model;
using DataETLViaHttp.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
    public class Spt1kmwgjsybxxStrategy : BaseStrategy, IStrategy
    {
        public const string NAME = "dwd_spt_1kmwgjsybxx";

        private readonly ILogger<Spt1kmwgjsybxxStrategy> _logger;

        public Spt1kmwgjsybxxStrategy(ILoggerFactory loggerFac, IDbConnectionFactory dbFactory, IConfiguration appSettings, IDataLoopUtil loopUtil) : base(dbFactory, appSettings, loopUtil)
        {
            _logger = loggerFac.CreateLogger<Spt1kmwgjsybxxStrategy>();
        }

        public virtual async Task Exeute(EntitiesUrl configEntity)
        {
            using var db = _dbFactory.OpenDbConnection();

            var max = db.Scalar<DateTime>(db.From<dwd_spt_1kmwgjsybxx>().Select(w => new { qbsj = Sql.Max("qbsj") }));
            var dwd_spt_1kmwgjsybxxs = await _loopUtil.GetDataFromInters<dwd_spt_1kmwgjsybxx>(configEntity,
                new Dictionary<string, object> { { "qbsj", max.ToString("yyyy-MM-dd HH:mm:ss") } });

            dwd_spt_1kmwgjsybxxs = dwd_spt_1kmwgjsybxxs.GroupBy(w => new { w.dsc_biz_record_id, w.dsc_biz_operation }).Select(w => w.FirstOrDefault()).ToList();
            var tableData = db.Select<dwd_spt_1kmwgjsybxx>();
            dwd_spt_1kmwgjsybxxs.RemoveAll(w => tableData.FindAll(x => x.dsc_biz_record_id == w.dsc_biz_record_id && 
                x.dsc_biz_operation == w.dsc_biz_operation).Count > 0);

            await db.InsertAllAsync(dwd_spt_1kmwgjsybxxs);

        }

        public virtual async Task GetDataBehindSeveralDay(EntitiesUrl configEntity, DateTime date)
        {
            using var db = _dbFactory.OpenDbConnection();

            if (db.Count<dwd_spt_1kmwgjsybxx>() == 0)
            {
                var dwd_spt_1kmwgjsybxxs = await _loopUtil.GetDataFromInters<dwd_spt_1kmwgjsybxx>(configEntity,
                    new Dictionary<string, object> { { "qbsj", date.ToString("yyyy-MM-dd HH:mm:ss") } });
                await db.InsertAllAsync(dwd_spt_1kmwgjsybxxs);

            }

            
        }
    }

}
