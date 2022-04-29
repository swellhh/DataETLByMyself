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
    public class SptQyzdqxzgcxxStrategy : BaseStrategy, IStrategy
    {
        private readonly ILogger<SptQyzdqxzgcxxStrategy> _logger;
        private readonly StationExcepts _stationExcepts;

        public SptQyzdqxzgcxxStrategy(IDbConnectionFactory dbFactory, ILoggerFactory loggerFac, IConfiguration appSettings, IDataLoopUtil loopUtil, StationExcepts stationExcepts) : base(dbFactory, appSettings, loopUtil)
        {
            _stationExcepts = stationExcepts;
            _logger = loggerFac.CreateLogger<SptQyzdqxzgcxxStrategy>(); ;
        }

        public virtual async Task Exeute(EntitiesUrl configEntity)
        {

            using var db = _dbFactory.OpenDbConnection();

            var max = db.Scalar<int>(db.From<dwd_spt_qyzdqxzgcxx>().Select(w => new { max = Sql.Max("observtimes") }));
            var dwd_spt_dmzdqxzgcxxs = await _loopUtil.GetDataFromInters<dwd_spt_qyzdqxzgcxx>(configEntity,
                new Dictionary<string, object> { { "observtimes", max } });
            dwd_spt_dmzdqxzgcxxs = dwd_spt_dmzdqxzgcxxs.GroupBy(w => new { w.stationnum, w.observtimes, w.etl_oper_type }).Select(w => w.FirstOrDefault()).ToList();
            var tableData = db.Select<dwd_spt_qyzdqxzgcxx>();
            dwd_spt_dmzdqxzgcxxs.RemoveAll(w => tableData.FindAll(x => x.stationnum == w.stationnum && x.observtimes == w.observtimes &&
                    x.etl_oper_type == w.etl_oper_type).Count > 0);

            dwd_spt_dmzdqxzgcxxs.RemoveAll(w => _stationExcepts.Select(w => w.code).Contains(w.stationnum));
            db.InsertAll(dwd_spt_dmzdqxzgcxxs);

        }

        public virtual async Task GetDataBehindSeveralDay(EntitiesUrl configEntity, DateTime date)
        {
            using var db = _dbFactory.OpenDbConnection();

            if (db.Count<dwd_spt_qyzdqxzgcxx>() == 0)
            {
                var dwd_spt_dmzdqxzgcxxs = await _loopUtil.GetDataFromInters<dwd_spt_qyzdqxzgcxx>(
                    async list => { await db.InsertAllAsync(list); },
                    configEntity,
                    new Dictionary<string, object> {
                            { "observtimes", date.ToString("yyyy-MM-dd HH:mm:ss") }
                    });

                dwd_spt_dmzdqxzgcxxs.RemoveAll(w => _stationExcepts.Select(w => w.code).Contains(w.stationnum));
                await db.InsertAllAsync(dwd_spt_dmzdqxzgcxxs);
            }

        }
    }
}
