using DataETLViaHttp.Model;
using DataETLViaHttp.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataETLViaHttp.Strategy
{
    public class JxssljDdtzdxxStrategy : BaseStrategy, IStrategy
    {
        public const string NAME = "dwd_jxsslj_ddtzdxx";
        
        public JxssljDdtzdxxStrategy(ILoggerFactory loggerFac, IDbConnectionFactory dbFactory, IConfiguration appSettings, IDataLoopUtil loopUtil) : base(dbFactory, appSettings, loopUtil)
        {
        }

        public virtual async Task Exeute(EntitiesUrl configEntity)
        {
            using var db = _dbFactory.OpenDbConnection();

            var max = db.Scalar<DateTime>(db.From<dwd_jxsslj_ddtzdxx>().Select(w => new { max = Sql.Max("dd_time") }));
            var dwd_jxsslj_ddtzdxxs = await _loopUtil.GetDataFromInters<dwd_jxsslj_ddtzdxx>(configEntity,
                new Dictionary<string, object> { { "dd_time", max.ToString("yyyy-MM-dd HH:mm:ss") } });
            dwd_jxsslj_ddtzdxxs = dwd_jxsslj_ddtzdxxs.GroupBy(w => new { w.dd_id, w.dd_time }).Select(w => w.FirstOrDefault()).ToList();
            var tableData = db.Select<dwd_jxsslj_ddtzdxx>();
            dwd_jxsslj_ddtzdxxs.RemoveAll(w => tableData.FindAll(x => x.dd_id == w.dd_id && x.dd_time == w.dd_time).Count > 0);
            db.InsertAll(dwd_jxsslj_ddtzdxxs);

        }

        public virtual async Task GetDataBehindSeveralDay(EntitiesUrl configEntity, DateTime date)
        {
            using var db = _dbFactory.OpenDbConnection();

            if (db.Count<dwd_jxsslj_ddtzdxx>() == 0)
            {
                var dwd_jxsslj_ddtzdxxs = await _loopUtil.GetDataFromInters<dwd_jxsslj_ddtzdxx>(
                    configEntity,
                    new Dictionary<string, object> {
                            { "dd_time", date.ToString("yyyy-MM-dd HH:mm:ss") }
                    });

                await db.InsertAllAsync(dwd_jxsslj_ddtzdxxs);
            }

        }
    }
}
