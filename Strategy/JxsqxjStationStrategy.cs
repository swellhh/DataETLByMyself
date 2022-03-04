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
    public class JxsqxjStationStrategy : BaseStrategy, IStrategy
    {
        private readonly ILogger<JxsqxjStationStrategy> _logger;

        public JxsqxjStationStrategy(ILoggerFactory loggerFac, IDbConnectionFactory dbFactory, IConfiguration appSettings, IDataLoopUtil loopUtil) : base(dbFactory, appSettings, loopUtil)
        {
            _logger = loggerFac.CreateLogger<JxsqxjStationStrategy>();
        }

        public async Task Exeute(EntitiesUrl configEntity)
        {
            _logger.LogInformation("{0}实时数据导入策略开始", "气象站台信息");

            using var db = _dbFactory.OpenDbConnection();

            var max = db.Scalar<int>(db.From<dwd_jxsqxj_station>().Select(w => new { max = Sql.Max("id") }));
            var dwd_jxsqxj_stations = await _loopUtil.GetDataFromInters<dwd_jxsqxj_station>(configEntity.url,
                new Dictionary<string, object> { { "id", max } });
            dwd_jxsqxj_stations = dwd_jxsqxj_stations.GroupBy(w => new { w.id, w.etl_oper_type }).Select(w => w.FirstOrDefault()).ToList();
            var tableData = db.Select<dwd_jxsqxj_station>();
            dwd_jxsqxj_stations.RemoveAll(w => tableData.FindAll(x => x.id == w.id && x.etl_oper_type == w.etl_oper_type).Count > 0);
            db.InsertAll(dwd_jxsqxj_stations);

        }

        public async Task GetDataBehindSeveralDay(EntitiesUrl configEntity, DateTime date)
        {
            using var db = _dbFactory.OpenDbConnection();

            if (db.Count<dwd_jxsqxj_station>() == 0)
            {
                var dwd_jxsqxj_stations = await _loopUtil.GetDataFromInters<dwd_jxsqxj_station>(configEntity.url);

                await db.InsertAllAsync(dwd_jxsqxj_stations);
                _logger.LogInformation("{0}初始化完成\r\n", "气象站台信息");
            }

        }
    }
}
