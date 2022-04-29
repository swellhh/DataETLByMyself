using Castle.Windsor;
using DataETLViaHttp.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStack.Data;
using System;
using System.Threading.Tasks;

namespace DataETLViaHttp.Strategy
{
    public abstract class BaseStrategy
    {
        protected readonly IDbConnectionFactory _dbFactory;
        protected readonly IConfiguration _appSettings;
        protected readonly IDataLoopUtil _loopUtil;

        protected BaseStrategy(IDbConnectionFactory dbFactory, IConfiguration appSettings, IDataLoopUtil loopUtil)
        {
            _dbFactory = dbFactory;
            _appSettings = appSettings;
            _loopUtil = loopUtil;
        }

        protected BaseStrategy()
        {
        }
    }
}
