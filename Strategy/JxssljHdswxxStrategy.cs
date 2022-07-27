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
    public class JxssljHdswxxStrategy : BaseStrategy, IStrategy
    {
        public const string NAME = "dwd_jxsslj_hdswxx";

        public JxssljHdswxxStrategy( IDbConnectionFactory dbFactory, IConfiguration appSettings, IDataLoopUtil loopUtil) : base(dbFactory, appSettings, loopUtil)
        {
        }

        public JxssljHdswxxStrategy()
        {
        }

        public virtual async Task Exeute(EntitiesUrl configEntity)
        {
            using var db = _dbFactory.OpenDbConnection();

            var max = db.Scalar<DateTime>(db.From<dwd_jxsslj_hdswxx>().Select(w => new { max = Sql.Max("tm") }));
            var dwd_jxsslj_hdswxxs = await _loopUtil.GetDataFromInters<dwd_jxsslj_hdswxx>(configEntity,
                new Dictionary<string, object> { { "tm", max.ToString("yyyy-MM-dd HH:mm:ss") } });
            dwd_jxsslj_hdswxxs = dwd_jxsslj_hdswxxs.GroupBy(w => new { w.tm, w.stcd }).Select(w => w.FirstOrDefault()).ToList();
            var tableData = db.Select<dwd_jxsslj_hdswxx>();
            dwd_jxsslj_hdswxxs.RemoveAll(w => tableData.FindAll(x => x.tm == w.tm && x.stcd == w.stcd).Count > 0);
            db.InsertAll(dwd_jxsslj_hdswxxs);
        }

        public virtual async Task GetDataBehindSeveralDay(EntitiesUrl configEntity, DateTime date)
        {
            using var db = _dbFactory.OpenDbConnection();

            if (db.Count<dwd_jxsslj_hdswxx>() == 0)
            {
                var dwd_jxsslj_hdswxxs = await _loopUtil.GetDataFromInters<dwd_jxsslj_hdswxx>(
                    async list => { await db.InsertAllAsync(list); },
                    configEntity,
                    new Dictionary<string, object> {
                            { "tm", date.ToString("yyyy-MM-dd HH:mm:ss") }
                    });

                await db.InsertAllAsync(dwd_jxsslj_hdswxxs);
            }
        }
    }
}
