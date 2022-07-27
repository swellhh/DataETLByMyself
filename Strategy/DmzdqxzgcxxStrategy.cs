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
    public class DmzdqxzgcxxStrategy : BaseStrategy, IStrategy
    {
        public const string NAME = "dwd_spt_dmzdqxzgcxx";

        public DmzdqxzgcxxStrategy(ILoggerFactory loggerFac, IDbConnectionFactory dbFactory, IConfiguration appSettings, IDataLoopUtil loopUtil) : base(dbFactory, appSettings, loopUtil)
        {
        }

        public virtual async Task Exeute(EntitiesUrl configEntity)
        {
            using var db = _dbFactory.OpenDbConnection();

            var max = db.Scalar<string>(db.From<dwd_spt_dmzdqxzgcxx>().Select(w => new { max = Sql.Max("observtimes") }));
            var dwd_spt_dmzdqxzgcxxs = await _loopUtil.GetDataFromInters<dwd_spt_dmzdqxzgcxx>(configEntity,
                new Dictionary<string, object> { { "observtimes", max } });
            dwd_spt_dmzdqxzgcxxs = dwd_spt_dmzdqxzgcxxs.GroupBy(w => new { w.stationnum, w.observtimes, w.etl_oper_type }).Select(w => w.FirstOrDefault()).ToList();
            var tableData = db.Select<dwd_spt_dmzdqxzgcxx>();
            dwd_spt_dmzdqxzgcxxs.RemoveAll(w => tableData.FindAll(x => x.stationnum == w.stationnum && x.observtimes==w.observtimes).Count > 0);
            db.InsertAll(dwd_spt_dmzdqxzgcxxs);

        }

        public virtual async Task GetDataBehindSeveralDay(EntitiesUrl configEntity, DateTime date)
        {
            using var db = _dbFactory.OpenDbConnection();

            if (db.Count<dwd_spt_dmzdqxzgcxx>() == 0)
            {
                var dwd_spt_dmzdqxzgcxxs = await _loopUtil.GetDataFromInters<dwd_spt_dmzdqxzgcxx>(
                    configEntity,
                    new Dictionary<string, object> {
                            { "observtimes", date.ToString("yyyyMMddHH") }
                    });

                await db.InsertAllAsync(dwd_spt_dmzdqxzgcxxs);
            }

        }
    }
}
