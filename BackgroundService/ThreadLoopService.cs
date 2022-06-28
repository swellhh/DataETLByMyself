using Cronos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataETLViaHttp.BackgroundService
{
    public class ThreadLoopService : CronScheduleBase
    {
        private readonly ILogger<DataETLService> _logger;
        private readonly IConfiguration _appSettings;

        public ThreadLoopService(ILoggerFactory loggerFac, IConfiguration appSettings) : base(loggerFac, appSettings)
        {
            _logger = loggerFac.CreateLogger<DataETLService>();
            _appSettings = appSettings;
        }

        public override string _cronExpression => "*/5 * * * * *";

        public override CronFormat _format => CronFormat.IncludeSeconds;

        public override Task Execute(CancellationToken cancellationToken)
        {
            while (true)
            {
                _logger.LogWarning("出大问题{0}", Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}
