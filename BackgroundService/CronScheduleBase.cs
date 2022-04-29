using Cronos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataETLViaHttp.BackgroundService
{
    public abstract class CronScheduleBase : IHostedService, IDisposable
    {
        public CancellationTokenSource source = new CancellationTokenSource();

        public Task task;

        public abstract string _cronExpression { get; }

        public abstract CronFormat _format { get; }


        private readonly ILogger _logger;


        private readonly IConfiguration _appSettings;

        private bool IsOpen;

        private bool IsPostBack { get; set; } = true;

        public CronScheduleBase(ILoggerFactory loggerFac, IConfiguration appSettings)
        {
            _logger = loggerFac.CreateLogger("CronScheduleBase");
            _appSettings = appSettings;
            IsOpen = _appSettings.GetValue<bool>("HostService:IsOpen");

        }

        public virtual Task SetOnce() { return Task.CompletedTask; }

        public abstract Task Execute(CancellationToken cancellationToken);

        public virtual void Dispose()
        {
            source.Cancel();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            source = source.IsCancellationRequested ? new CancellationTokenSource() : source;

            if (IsOpen)
            {
                CronExpression expression = CronExpression.Parse(_cronExpression, _format);
                DateTime? nextUtc = expression.GetNextOccurrence(DateTime.UtcNow);

                _ = Task.Run(() =>
                {
                    _logger.LogInformation("\n\rNext at {nextUtc} \n\rNow at {now},\n\rThreadId:{threadId}",
                                nextUtc.Value.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss"),
                                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                Thread.CurrentThread.ManagedThreadId);


                    while (!source.Token.IsCancellationRequested && nextUtc.HasValue)
                    {
                        if (IsPostBack)
                        {

                            SetOnce();
                            IsPostBack = false;
                        }
                        if (nextUtc.HasValue && DateTime.Now >= nextUtc.Value.AddHours(8))
                        {
                            task = Execute(source.Token);

                            nextUtc = expression.GetNextOccurrence(DateTime.UtcNow);
                            Task.Delay(nextUtc.Value - DateTime.UtcNow);
                        }
                        else
                        {
                            Task.Delay(TimeSpan.FromSeconds(1));
                        }

                    }

                }, source.Token);
            }

            if (task != null && task.IsCompleted)
            {
                return task;
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            source.Cancel(false);

            _logger.LogInformation("{0}定时器停止成功", _cronExpression);

            Dispose();

            return Task.CompletedTask;
        }
    }
}
