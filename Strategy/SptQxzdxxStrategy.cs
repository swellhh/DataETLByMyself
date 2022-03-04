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

        public async Task Exeute(EntitiesUrl configEntity)
        {
            _logger.LogInformation("{0}实时数据导入策略开始", "气象站点信息");

            using var db = _dbFactory.OpenDbConnection();

            var max = db.Scalar<int>(db.From<dwd_spt_qxzdxx>().Select(w => new { dsc_biz_record_id = Sql.Max("dsc_biz_record_id") }));
            var dwd_spt_qxzdxxs = await _loopUtil.GetDataFromInters<dwd_spt_qxzdxx>(configEntity.url
                , new Dictionary<string, object> { { "dsc_biz_record_id", max } });

            dwd_spt_qxzdxxs = dwd_spt_qxzdxxs.GroupBy(w => new { w.dsc_biz_record_id, w.iiiii, w.dsc_biz_operation }).Select(w => w.FirstOrDefault()).ToList();
            var tableData = db.Select<dwd_spt_qxzdxx>();
            dwd_spt_qxzdxxs.RemoveAll(w => tableData.FindAll(x => x.dsc_biz_record_id == w.dsc_biz_record_id &&
                x.dsc_biz_operation == w.dsc_biz_operation && x.iiiii == w.iiiii).Count > 0);

            await db.InsertAllAsync(dwd_spt_qxzdxxs);
        }

        public async Task GetDataBehindSeveralDay(EntitiesUrl configEntity, DateTime date)
        {
            using var db = _dbFactory.OpenDbConnection();

            if (db.Count<dwd_spt_qxzdxx>() == 0)
            {
                var dwd_spt_qxzdxxs = await _loopUtil.GetDataFromInters<dwd_spt_qxzdxx>(configEntity.url);
                await db.InsertAllAsync(dwd_spt_qxzdxxs);

                _logger.LogInformation("{0}初始化成功\r\n", "自动气象站站点信");
            }


        }
    }

}
