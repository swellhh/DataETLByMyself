using Castle.DynamicProxy;
using DataETLViaHttp.Cache;
using DataETLViaHttp.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using static DataETLViaHttp.Startup;

namespace DataETLViaHttp.BackgroundService
{
    public class DataETLRetryInterceptor : IInterceptor
    {
        private readonly ILogger<DataETLRetryInterceptor> _logger;
        private readonly IConfiguration _appSettings;
        private readonly ICacheClient _cache;
        private readonly int _retryCountLimit;

        public DataETLRetryInterceptor(ILoggerFactory loggerFac, IConfiguration appSettings, ICacheClient cache)
        {
            _logger = loggerFac.CreateLogger<DataETLRetryInterceptor>();
            _appSettings = appSettings;
            _cache = cache;
            _retryCountLimit = _appSettings.GetValue<int>("Application:RetryCountLimit");
        }


        public Task RetryWhenExceptionThrow(MethodInfo retryDelegate, object? target, object?[]? parameters)
        {
            var item = (EntitiesUrl)parameters[0];

            _cache.Increment(item.name, 1);

            _logger.LogInformation("{0}开始重试抓,次数{1} \r\n", item.zw, _cache.Get<long>(item.name));

            Task.Delay(TimeSpan.FromSeconds(_cache.Get<int>(item.name) * 60)).ContinueWith(t => retryDelegate.Invoke(target, parameters));

            return Task.CompletedTask;
        }

        public void Intercept(IInvocation invocation)
        {
            var target = (EntitiesUrl)invocation.Arguments[0];


            try
            {
                invocation.Proceed();

                if (invocation.ReturnValue is Task returnValueTask)
                {
                    returnValueTask.GetAwaiter().GetResult();
                }
                if (invocation.ReturnValue is Task task && task.Exception != null)
                {
                    throw task.Exception;
                }
            }
            catch (Exception ex)
            {
                if (_cache.Get<int>(target.name) < _retryCountLimit)
                {
                    RetryWhenExceptionThrow(invocation.Method, invocation.InvocationTarget, invocation.Arguments).GetAwaiter().GetResult();
                }

                _logger.LogTrace("重试次数{0}", _cache.Get<int>(target.name));
                _logger.LogError(ex, "出大问题");

            }

        }
    }
}
