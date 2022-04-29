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
    public class SptZjsjqxmylxsskxxStrategy : BaseStrategy, IStrategy
    {
        private readonly ILogger<SptZjsjqxmylxsskxxStrategy> _logger;

        public SptZjsjqxmylxsskxxStrategy(ILoggerFactory loggerFac, IDbConnectionFactory dbFactory, IConfiguration appSettings, IDataLoopUtil loopUtil) : base(dbFactory, appSettings, loopUtil)
        {
            _logger = loggerFac.CreateLogger<SptZjsjqxmylxsskxxStrategy>();
        }

        public virtual async Task Exeute(EntitiesUrl configEntity)
        {
            using var db = _dbFactory.OpenDbConnection();

            var max = db.Scalar<DateTime>(db.From<dwd_spt_zjsjqxmylxsskxx>().Select(w => new { observtimes = Sql.Max("observtimes") }));
            var dwd_spt_zjsjqxmylxsskxxs = await _loopUtil.GetDataFromInters<dwd_spt_zjsjqxmylxsskxx>(configEntity,
                new Dictionary<string, object> { { "observtimes", max.ToString("yyyy-MM-dd HH:mm:ss") } });

            dwd_spt_zjsjqxmylxsskxxs = dwd_spt_zjsjqxmylxsskxxs.GroupBy(w => new { w.observtimes, w.dsc_biz_record_id, w.dsc_biz_operation }).Select(w => w.FirstOrDefault()).ToList();
            var tableData = db.Select<dwd_spt_zjsjqxmylxsskxx>();
            dwd_spt_zjsjqxmylxsskxxs.RemoveAll(w => tableData.FindAll(x => x.dsc_biz_record_id == w.dsc_biz_record_id &&
                x.observtimes == w.observtimes &&
                x.dsc_biz_operation == w.dsc_biz_operation).Count > 0);

            await db.InsertAllAsync(dwd_spt_zjsjqxmylxsskxxs);

        }

        public virtual async Task GetDataBehindSeveralDay(EntitiesUrl configEntity, DateTime date)
        {
            using var db = _dbFactory.OpenDbConnection();

            if (db.Count<dwd_spt_zjsjqxmylxsskxx>() == 0)
            {
                var dwd_spt_zjsjqxmylxsskxxs = await _loopUtil.GetDataFromInters<dwd_spt_zjsjqxmylxsskxx>(configEntity,
                    new Dictionary<string, object> { { "observtimes", date.ToString("yyyy-MM-dd HH:mm:ss") } });

                await db.InsertAllAsync(dwd_spt_zjsjqxmylxsskxxs);
            }
        }
    }
}
