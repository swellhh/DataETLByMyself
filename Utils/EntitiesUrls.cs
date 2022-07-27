using System;
using System.Collections.Generic;
using System.Text;

namespace DataETLViaHttp.Utils
{
    public class EntitiesUrls : List<EntitiesUrl>
    {

    }

    public class EntitiesUrl
    {
        public string name { get; set; }

        public string url { get; set; }

        public string client { get; set; }


        public bool isStop { get; set; }

        public string zw { get; set; }

        public DateTime? syncDate { get; set; }


    }
}
