using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataETLViaHttp.Utils
{
    public static class RecordFileExtension
    {
        private static object locker = new object();

        public static bool RecordPageIndex(this IConfiguration configuration,string name,int pageIndex)
        {

            StreamWriter writer = null;

            lock (locker)
            {
                try
                {
                    var filePath = configuration.GetValue<string>("Application:RecordPageIndexFilePath");
                    var text = File.ReadAllText(filePath);
                    var obj = JsonConvert.DeserializeObject<Dictionary<string, int>>(text);

                    obj[name] = pageIndex;
                    var deserializeText = JsonConvert.SerializeObject(obj);
                    writer = File.CreateText(filePath);
                    writer.Write(deserializeText);
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                }
                catch (Exception)
                {
                    writer.Close();
                    writer.Dispose();
                    return false;
                }

            }

            return true;
        }

        public static Dictionary<string,int> GetPageIndexRecord(this IConfiguration configuration)
        {

            var filePath = configuration.GetValue<string>("Application:RecordPageIndexFilePath");
            var text = File.ReadAllText(filePath);
            var obj = JsonConvert.DeserializeObject<Dictionary<string, int>>(text);


            return obj;
        }

    }
}
