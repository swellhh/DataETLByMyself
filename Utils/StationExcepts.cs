using System;
using System.Collections.Generic;
using System.Text;

namespace DataETLViaHttp.Utils
{
    public class StationExcepts : List<StationExcept>
    {
    }

    public class StationExcept
    {
        public string name { get; set; }

        public string code { get; set; }
    }
}
