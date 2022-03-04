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
    public class DmzdqxzgcxxStrategy : BaseStrategy, IStrategy
    {
        private readonly ILogger<DmzdqxzgcxxStrategy> _logger;

        public DmzdqxzgcxxStrategy(ILoggerFactory loggerFac, IDbConnectionFactory dbFactory, IConfiguration appSettings, IDataLoopUtil loopUtil) : base(dbFactory, appSettings, loopUtil)
        {
            _logger = loggerFac.CreateLogger<DmzdqxzgcxxStrategy>();
        }

        public async Task Exeute(EntitiesUrl configEntity)
        {
            using var db = _dbFactory.OpenDbConnection();

            var max = db.Scalar<int>(db.From<dwd_spt_dmzdqxzgcxx>().Select(w => new { max = Sql.Max("observtimes") }));
            var dwd_spt_dmzdqxzgcxxs = await _loopUtil.GetDataFromInters<dwd_spt_dmzdqxzgcxx>(configEntity,
                new Dictionary<string, object> { { "observtimes", max } });
            dwd_spt_dmzdqxzgcxxs = dwd_spt_dmzdqxzgcxxs.GroupBy(w => new { w.stationnum, w.observtimes, w.etl_oper_type }).Select(w => w.FirstOrDefault()).ToList();
            var tableData = db.Select<dwd_spt_dmzdqxzgcxx>();
            dwd_spt_dmzdqxzgcxxs.RemoveAll(w => tableData.FindAll(x => x.stationnum == w.stationnum && x.observtimes==w.observtimes && 
                    x.etl_oper_type == w.etl_oper_type).Count > 0);
            db.InsertAll(dwd_spt_dmzdqxzgcxxs);

            _logger.LogInformation("{0}实时数据导入策略成功", "地面自动气象站观测信息");

        }

        public async Task GetDataBehindSeveralDay(EntitiesUrl configEntity, DateTime date)
        {
            using var db = _dbFactory.OpenDbConnection();

            try
            {
                if (db.Count<dwd_spt_dmzdqxzgcxx>() == 0)
                {
                    var dwd_spt_dmzdqxzgcxxs = await _loopUtil.GetDataFromInters<dwd_spt_dmzdqxzgcxx>(
                        async list => { await db.InsertAllAsync(list); },
                        configEntity,
                        new Dictionary<string, object> {
                            { "observtimes", date.ToString("yyyy-MM-dd HH:mm:ss") }
                        });

                    await db.InsertAllAsync(dwd_spt_dmzdqxzgcxxs);
                    _logger.LogInformation("{0}初始化完成\r\n", "地面自动气象站观测信息");
                }
            }
            catch (Exception)
            {
                _logger.LogInformation("{0}报错SQL：｛1｝\r\n", "地面自动气象站观测信息", db.GetLastSql());

            }

        }
    }
}
