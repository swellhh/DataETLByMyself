using Castle.DynamicProxy;
using DataETLViaHttp.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataETLViaHttp.Aop
{
    public class LoggerInterceptor : IInterceptor
    {
        private readonly ILogger<LoggerInterceptor> _logger;
         
        public LoggerInterceptor(ILogger<LoggerInterceptor> logger)
        {
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            var methodName = invocation.Method.Name;

            switch (methodName)
            {
                case "GetDataBehindSeveralDay":
                    _logger.LogTrace("{0}初始化开始", ((EntitiesUrl)invocation.Arguments[0]).zw);

                    break;
                case "Exeute":
                    _logger.LogTrace("{0}实时数据导入策略开始", ((EntitiesUrl)invocation.Arguments[0]).zw);

                    break;
                default:
                    _logger.LogError("{0}找不到方法", ((EntitiesUrl)invocation.Arguments[0]).zw);
                    break;

            }

            invocation.Proceed();

            switch (methodName)
            {
                case "GetDataBehindSeveralDay":
                    _logger.LogTrace("{0}初始化完成\r\n", ((EntitiesUrl)invocation.Arguments[0]).zw);

                    break;
                case "Exeute":
                    _logger.LogTrace("{0}实时数据导入策略完成\r\n", ((EntitiesUrl)invocation.Arguments[0]).zw);

                    break;
                default:
                    _logger.LogError("{0}找不到方法\r\n", ((EntitiesUrl)invocation.Arguments[0]).zw);
                    break;

            }
        }
    }
}
