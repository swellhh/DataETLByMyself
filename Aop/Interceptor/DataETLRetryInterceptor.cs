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


            Task task =  Task.Delay(TimeSpan.FromSeconds(_cache.Get<long>(item.name) * 60))
                .ContinueWith(w => 
                {
                    _cache.Increment(item.name, 1);

                    _logger.LogTrace("{0}开始重试,次数{1} \r\n", item.zw, _cache.Get<long>(item.name));
                    return retryDelegate.Invoke(target, parameters);
                });

            if (task != null && task.IsCompleted)
            {
                return task;
            }

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
            catch (Exception)
            {
                if (_cache.Get<long>(target.name) < _retryCountLimit)
                {
                    RetryWhenExceptionThrow(invocation.Method, invocation.InvocationTarget, invocation.Arguments).GetAwaiter().GetResult();
                }
                else
                {
                    throw;
                }
            }

        }
    }
}
