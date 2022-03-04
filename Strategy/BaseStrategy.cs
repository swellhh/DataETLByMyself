using DataETLViaHttp.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceStack.Data;


namespace DataETLViaHttp.Strategy
{
    public class BaseStrategy
    {
        protected readonly IDbConnectionFactory _dbFactory;
        protected readonly IConfiguration _appSettings;
        protected readonly IDataLoopUtil _loopUtil;

        public BaseStrategy(IDbConnectionFactory dbFactory, IConfiguration appSettings, IDataLoopUtil loopUtil)
        {
            _dbFactory = dbFactory;
            _appSettings = appSettings;
            _loopUtil = loopUtil;
        }
    }
}
