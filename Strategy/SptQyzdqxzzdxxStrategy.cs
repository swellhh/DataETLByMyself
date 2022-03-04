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

        public SptQyzdqxzzdxxStrategy(ILoggerFactory loggerFac, IDbConnectionFactory dbFactory, IConfiguration appSettings, IDataLoopUtil loopUtil) : base(dbFactory, appSettings, loopUtil)
        {
            _logger = loggerFac.CreateLogger<SptQyzdqxzzdxxStrategy>();
        }

        public async Task Exeute(EntitiesUrl configEntity)
        {
            _logger.LogInformation("{0}实时数据导入策略开始", "区域自动气象站站点信息");

            using var db = _dbFactory.OpenDbConnection();

            var max = db.Scalar<int>(db.From<dwd_spt_qyzdqxzzdxx>().Select(w => new { dsc_biz_record_id = Sql.Max("dsc_biz_record_id") }));
            var dwd_spt_qyzdqxzzdxxs = await _loopUtil.GetDataFromInters<dwd_spt_qyzdqxzzdxx>(configEntity.url
                , new Dictionary<string, object> { { "dsc_biz_record_id", max } });

            dwd_spt_qyzdqxzzdxxs = dwd_spt_qyzdqxzzdxxs.GroupBy(w => new { w.dsc_biz_record_id, w.iiiii, w.dsc_biz_operation }).Select(w => w.FirstOrDefault()).ToList();
            var tableData = db.Select<dwd_spt_qyzdqxzzdxx>();
            dwd_spt_qyzdqxzzdxxs.RemoveAll(w => tableData.FindAll(x => x.dsc_biz_record_id == w.dsc_biz_record_id &&
                x.dsc_biz_operation == w.dsc_biz_operation && x.iiiii == w.iiiii).Count > 0);

            await db.InsertAllAsync(dwd_spt_qyzdqxzzdxxs);
        }

        public async Task GetDataBehindSeveralDay(EntitiesUrl configEntity, DateTime date)
        {
            using var db = _dbFactory.OpenDbConnection();

            if (db.Count<dwd_spt_qyzdqxzzdxx>() == 0)
            {
                var dwd_spt_qyzdqxzzdxxs = await _loopUtil.GetDataFromInters<dwd_spt_qyzdqxzzdxx>(configEntity.url);
                await db.InsertAllAsync(dwd_spt_qyzdqxzzdxxs);

                _logger.LogInformation("{0}初始化成功\r\n", "省平台-区域自动气象站站点信息");
            }


        }
    }

}
