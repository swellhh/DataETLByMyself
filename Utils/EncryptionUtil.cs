using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DataETLViaHttp.Utils
{
    public static class EncryptionUtil
    {
        public static string GenerateSign(string SecretKey,JObject paramMap,string uri,string timestamp)
        {
            var StringToSign = $"GET\n{GetContentMD5(paramMap,false)}\napplication/json\n{timestamp}\n{uri}";

            var Signature = EncryptHMACSHA1String(SecretKey, StringToSign);

            return Signature;
        }

        private static string EncryptHMACSHA1String(string secretKey, string stringToSign)
        {
            HMACSHA1 hmacsha1 = new HMACSHA1();

            hmacsha1.Key = Encoding.ASCII.GetBytes(secretKey);
            byte[] dataBuffer = Encoding.UTF8.GetBytes(stringToSign);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// 只有POST方式请求才需要
        /// </summary>
        /// <param name="paramMap"></param>
        /// <returns></returns>
        private static string GetContentMD5(JObject paramMap,bool isPost)
        {
            string contentMD5 = "";
            if (isPost)
            {
                var paramsData = TransJsonToSpecific(paramMap);

                contentMD5 = Convert.ToBase64String(ConvertStringToMD5(paramsData));
            }

            return contentMD5;
        }

        public static string TransJsonToSpecific(JObject paramMap)
        {
            var p = GetRequestParams(paramMap);

            var str = p
                .Where(x => !string.IsNullOrEmpty(x.Value))
                .Select(x => string.Format("{0}={1}", x.Key, x.Value));

            return string.Join("&", str);
        }

        private static byte[] ConvertStringToMD5(string ClearText)
        {

            byte[] ByteData = Encoding.UTF8.GetBytes(ClearText);
            //MD5 creating MD5 object.
            MD5 oMd5 = MD5.Create();
            //Hash de??erini hesaplayal?±m.
            byte[] HashData = oMd5.ComputeHash(ByteData);


            return HashData;
        }

        /// <summary>
        /// 将请求体变成`key=val&`形式
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Dictionary<string, string> GetRequestParams(JObject request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            var allKeys = request.Properties();
            var values = request.Properties();
            var j = 0;

            foreach (JProperty text in allKeys)
            {
                dictionary[text.Name] = values.ToArray()[j].Value.ToString();
                j++;
            }

            return dictionary;
        }

    }
}
