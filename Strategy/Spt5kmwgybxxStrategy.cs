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
    public class Spt5kmwgybxxStrategy : BaseStrategy, IStrategy
    {
        private readonly ILogger<Spt5kmwgybxxStrategy> _logger;

        public Spt5kmwgybxxStrategy(ILoggerFactory loggerFac, IDbConnectionFactory dbFactory, IConfiguration appSettings, IDataLoopUtil loopUtil) : base(dbFactory, appSettings, loopUtil)
        {
            _logger = loggerFac.CreateLogger<Spt5kmwgybxxStrategy>();
        }

        public async Task Exeute(EntitiesUrl configEntity)
        {
            _logger.LogInformation("{0}实时数据导入策略开始", "5公里网格预报信息");

            using var db = _dbFactory.OpenDbConnection();

            var max = db.Scalar<DateTime>(db.From<dwd_spt_5kmwgybxx>().Select(w => new { ybgxsj = Sql.Max("ybgxsj") }));
            var dwd_spt_5kmwgybxxs = await _loopUtil.GetDataFromInters<dwd_spt_5kmwgybxx>(configEntity,
                new Dictionary<string, object> { { "ybgxsj", max.ToString("yyyy-MM-dd HH:mm:ss") } });

            dwd_spt_5kmwgybxxs = dwd_spt_5kmwgybxxs.GroupBy(w => new { w.dsc_biz_record_id, w.dsc_biz_operation }).Select(w => w.FirstOrDefault()).ToList();
            var tableData = db.Select<dwd_spt_5kmwgybxx>();
            dwd_spt_5kmwgybxxs.RemoveAll(w => tableData.FindAll(x => x.dsc_biz_record_id == w.dsc_biz_record_id &&
                x.dsc_biz_operation == w.dsc_biz_operation).Count > 0);

            await db.InsertAllAsync(dwd_spt_5kmwgybxxs);
        }

        public async Task GetDataBehindSeveralDay(EntitiesUrl configEntity, DateTime date)
        {
            using var db = _dbFactory.OpenDbConnection();

            if (db.Count<dwd_spt_5kmwgybxx>() == 0)
            {
                var dwd_spt_5kmwgybxxs = await _loopUtil.GetDataFromInters<dwd_spt_5kmwgybxx>(configEntity,
                    new Dictionary<string, object> { { "ybgxsj", date.ToString("yyyy-MM-dd HH:mm:ss") } });
                await db.InsertAllAsync(dwd_spt_5kmwgybxxs);

                _logger.LogInformation("{0}初始化成功\r\n", "省平台-5公里网格预报信息");
            }

            

        }
    }

}
