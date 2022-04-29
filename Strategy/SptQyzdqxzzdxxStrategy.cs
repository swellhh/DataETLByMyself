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
    public class SptQyzdqxzzdxxStrategy : BaseStrategy, IStrategy
    {
        private readonly ILogger<SptQyzdqxzzdxxStrategy> _logger;
        private readonly StationExcepts _stationExcepts;

        public SptQyzdqxzzdxxStrategy(ILoggerFactory loggerFac, IDbConnectionFactory dbFactory, IConfiguration appSettings, IDataLoopUtil loopUtil, StationExcepts stationExcepts) : base(dbFactory, appSettings, loopUtil)
        {
            _stationExcepts = stationExcepts;
            _logger = loggerFac.CreateLogger<SptQyzdqxzzdxxStrategy>();
        }

        public virtual async Task Exeute(EntitiesUrl configEntity)
        {
            using var db = _dbFactory.OpenDbConnection();

            var dwd_spt_qyzdqxzzdxxs = await _loopUtil.GetDataFromInters<dwd_spt_qyzdqxzzdxx>(configEntity.url);

            dwd_spt_qyzdqxzzdxxs = dwd_spt_qyzdqxzzdxxs.GroupBy(w => new { w.iiiii }).Select(w => w.FirstOrDefault()).ToList();
            dwd_spt_qyzdqxzzdxxs.RemoveAll(w => _stationExcepts.Select(w => w.code).Contains(w.iiiii));

            await db.InsertAllAsync(dwd_spt_qyzdqxzzdxxs, command => command.OnConflictIgnore());

        }

        public virtual async Task GetDataBehindSeveralDay(EntitiesUrl configEntity, DateTime date)
        {
            using var db = _dbFactory.OpenDbConnection();

            if (db.Count<dwd_spt_qyzdqxzzdxx>() == 0)
            {
                var dwd_spt_qyzdqxzzdxxs = await _loopUtil.GetDataFromInters<dwd_spt_qyzdqxzzdxx>(configEntity.url);
                dwd_spt_qyzdqxzzdxxs.RemoveAll(w => _stationExcepts.Select(w => w.code).Contains(w.iiiii));

                await db.InsertAllAsync(dwd_spt_qyzdqxzzdxxs, command => command.OnConflictIgnore());

            }


        }
    }

}
