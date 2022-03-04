using DataETLViaHttp.Utils;
using DataETLViaHttp.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;

namespace DataETLViaHttp.Strategy
{
    public class SptZjsjqx24xsljmylybxxStrategy : BaseStrategy, IStrategy
    {
        private readonly ILogger<SptZjsjqx24xsljmylybxxStrategy> _logger;

        public SptZjsjqx24xsljmylybxxStrategy(ILoggerFactory loggerFac, IDbConnectionFactory dbFactory, IConfiguration appSettings, IDataLoopUtil loopUtil) : base(dbFactory, appSettings, loopUtil)
        {
            _logger = loggerFac.CreateLogger<SptZjsjqx24xsljmylybxxStrategy>();
        }

        public async Task Exeute(EntitiesUrl configEntity)
        {
            _logger.LogInformation("{0}实时数据导入策略开始", "浙江省级区县24小时累计面雨量预报信息");

            using var db = _dbFactory.OpenDbConnection();

            var max = db.Scalar<DateTime>(db.From<dwd_spt_zjsjqx24xsljmylybxx>().Select(w => new { reporttimes = Sql.Max("reporttimes") }));
            var dwd_spt_zjsjqx24xsljmylybxxs = await _loopUtil.GetDataFromInters<dwd_spt_zjsjqx24xsljmylybxx>(configEntity,
                new Dictionary<string, object> { { "reporttimes", max.ToString("yyyy-MM-dd HH:mm:ss") } });

            dwd_spt_zjsjqx24xsljmylybxxs = dwd_spt_zjsjqx24xsljmylybxxs.GroupBy(w => new { w.dsc_biz_record_id, w.dsc_biz_operation }).Select(w => w.FirstOrDefault()).ToList();
            var tableData = db.Select<dwd_spt_zjsjqx24xsljmylybxx>();
            dwd_spt_zjsjqx24xsljmylybxxs.RemoveAll(w => tableData.FindAll(x => x.dsc_biz_record_id == w.dsc_biz_record_id &&
                x.dsc_biz_operation == w.dsc_biz_operation).Count > 0);

            await db.InsertAllAsync(dwd_spt_zjsjqx24xsljmylybxxs);
        }

        public async Task GetDataBehindSeveralDay(EntitiesUrl configEntity, DateTime date)
        {
            using var db = _dbFactory.OpenDbConnection();

            if (db.Count<dwd_spt_zjsjqx24xsljmylybxx>() == 0)
            {
                var dwd_spt_zjsjqx24xsljmylybxxs = await _loopUtil.GetDataFromInters<dwd_spt_zjsjqx24xsljmylybxx>(configEntity,
                    new Dictionary<string, object> { { "reporttimes", date.ToString("yyyy-MM-dd HH:mm:ss") } });

                await db.InsertAllAsync(dwd_spt_zjsjqx24xsljmylybxxs);
                _logger.LogInformation("{0}初始化成功\r\n", "省平台-浙江省级区县24小时累计面雨量预报信息");
            }
                
        }
    }
}
