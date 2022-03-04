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
    public class JxssljNpgqjcsjxxStrategy : BaseStrategy, IStrategy
    {
        private readonly ILogger<JxssljNpgqjcsjxxStrategy> _logger;

        public JxssljNpgqjcsjxxStrategy(ILoggerFactory loggerFac, IDbConnectionFactory dbFactory, IConfiguration appSettings, IDataLoopUtil loopUtil) : base(dbFactory, appSettings, loopUtil)
        {
            _logger = loggerFac.CreateLogger<JxssljNpgqjcsjxxStrategy>();
        }

        public async Task Exeute(EntitiesUrl configEntity)
        {
            _logger.LogInformation("{0}实时数据导入策略开始", "南排工程日累计排水量信息");

            using var db = _dbFactory.OpenDbConnection();

            var max = db.Scalar<DateTime>(db.From<dwd_jxsslj_npgqjcsjxx>().Select(w => new { tm = Sql.Max("tm") }));
            var dwd_jxsslj_npgqjcsjxxs = await _loopUtil.GetDataFromInters<dwd_jxsslj_npgqjcsjxx>(configEntity,
                new Dictionary<string, object> { { "tm", max.ToString("yyyy-MM-dd HH:mm:ss") } });

            dwd_jxsslj_npgqjcsjxxs = dwd_jxsslj_npgqjcsjxxs.GroupBy(w => new { w.STCD, w.EQPTP, w.EQPNO }).Select(w => w.FirstOrDefault()).ToList();
            var tableData = db.Select<dwd_jxsslj_npgqjcsjxx>();
            dwd_jxsslj_npgqjcsjxxs.RemoveAll(w => tableData.FindAll(x => x.STCD == w.STCD && x.EQPTP == w.EQPTP && x.EQPNO == w.EQPNO).Count > 0);

            await db.InsertAllAsync(dwd_jxsslj_npgqjcsjxxs);
        }

        public async Task GetDataBehindSeveralDay(EntitiesUrl configEntity, DateTime date)
        {
            using var db = _dbFactory.OpenDbConnection();

            if (db.Count<dwd_jxsslj_npgqjcsjxx>() == 0)
            {
                var dwd_jxsslj_npgqjcsjxxs = await _loopUtil.GetDataFromInters<dwd_jxsslj_npgqjcsjxx>(configEntity,
                    new Dictionary<string, object> { { "tm", date.ToString("yyyy-MM-dd HH:mm:ss") } });
                await db.InsertAllAsync(dwd_jxsslj_npgqjcsjxxs);

                _logger.LogInformation("{0}初始化成功\r\n", "南排工情监测数据信息");
            }

        }
    }

}
