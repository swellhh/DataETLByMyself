using System;
using System.Collections.Generic;
using System.Text;

namespace DataETLViaHttp.Cache
{
    public interface ICacheClientExtended : ICacheClient
    {
        TimeSpan? GetTimeToLive(string key);

        IEnumerable<string> GetKeysByPattern(string pattern);

        void RemoveExpiredEntries();
    }
}
