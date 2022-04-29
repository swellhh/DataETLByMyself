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
    public class SptQxzdxxStrategy : BaseStrategy, IStrategy
    {
        private readonly ILogger<SptQxzdxxStrategy> _logger;

        public SptQxzdxxStrategy(ILoggerFactory loggerFac, IDbConnectionFactory dbFactory, IConfiguration appSettings, IDataLoopUtil loopUtil) : base(dbFactory, appSettings, loopUtil)
        {
            _logger = loggerFac.CreateLogger<SptQxzdxxStrategy>();
        }

        public virtual async Task Exeute(EntitiesUrl configEntity)
        {
            using var db = _dbFactory.OpenDbConnection();

            var dwd_spt_qxzdxxs = await _loopUtil.GetDataFromInters<dwd_spt_qxzdxx>(configEntity.url);

            dwd_spt_qxzdxxs = dwd_spt_qxzdxxs.GroupBy(w => new { w.iiiii }).Select(w => w.FirstOrDefault()).ToList();

            await db.InsertAllAsync(dwd_spt_qxzdxxs, command => command.OnConflictIgnore());

        }

        public virtual async Task GetDataBehindSeveralDay(EntitiesUrl configEntity, DateTime date)
        {
            using var db = _dbFactory.OpenDbConnection();

            if (db.Count<dwd_spt_qxzdxx>() == 0)
            {
                var dwd_spt_qxzdxxs = await _loopUtil.GetDataFromInters<dwd_spt_qxzdxx>(configEntity.url);
                await db.InsertAllAsync(dwd_spt_qxzdxxs);

            }


        }
    }

}
