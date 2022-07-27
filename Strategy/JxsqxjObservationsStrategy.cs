using DataETLViaHttp.Model;
using DataETLViaHttp.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataETLViaHttp.Strategy
{
    public class JxsqxjObservationsStrategy : BaseStrategy, IStrategy
    {
        public const string NAME = "dwd_jxsqxj_observations";

        public ILogger<JxsqxjObservationsStrategy> _logger;

        public JxsqxjObservationsStrategy(ILogger<JxsqxjObservationsStrategy> _logger,IDbConnectionFactory dbFactory, IConfiguration appSettings, IDataLoopUtil loopUtil) : base(dbFactory, appSettings, loopUtil)
        {
            this._logger = _logger;
        }

        public virtual async Task Exeute(EntitiesUrl configEntity)
        {
            using var db = _dbFactory.OpenDbConnection();

            //todo 查看连续间隔的时间的最大值，若有中断则从中断的时刻进行同步
            var max = db.Scalar<DateTime>(db.From<dwd_jxsqxj_observations>().Select(w => new { max = Sql.Max("observ_time") }));
            var dwd_jxsqxj_observations = await _loopUtil.GetDataFromInters<dwd_jxsqxj_observations>(
                async list => {
                    await db.InsertAllAsync(list, dbCmd => dbCmd.OnConflictIgnore());
                },
                configEntity,
                new Dictionary<string, object> {
                    { "observ_time", max.ToString("yyyy-MM-dd HH:mm:ss") }
                });
            dwd_jxsqxj_observations = dwd_jxsqxj_observations.GroupBy(w => new { w.station, w.observ_time }).Select(w => w.FirstOrDefault()).ToList();
            db.InsertAll(dwd_jxsqxj_observations, dbCmd => dbCmd.OnConflictIgnore());

        }

        public virtual async Task GetDataBehindSeveralDay(EntitiesUrl configEntity, DateTime date)
        {
            using var db = _dbFactory.OpenDbConnection();

            date = configEntity.syncDate ?? date;

            if (db.Count<dwd_jxsqxj_observations>() == 0)
            {
                var dwd_jxsqxj_observations = await _loopUtil.GetDataFromInters<dwd_jxsqxj_observations>(
                    async list => {
                        await db.InsertAllAsync(list, dbCmd => dbCmd.OnConflictIgnore()); },
                    configEntity,
                    new Dictionary<string, object> {
                            { "observ_time", date.ToString("yyyy-MM-dd HH:mm:ss") }
                    });

                await db.InsertAllAsync(dwd_jxsqxj_observations);
            }

        }
    }
}
