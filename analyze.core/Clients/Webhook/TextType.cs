using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Clients.Webhook
{
    internal static class TextType
    {
        internal static string RobotUrl { get; set; }
        internal static async Task<OperationResult> SendText(string text)
        {
            OperationResult result = new OperationResult();
            string recData = await PostAsync(RobotUrl, getTextTypeJson(text));
            dynamic json = JsonConvert.DeserializeObject<dynamic>(recData);
            int errorcode = json.errcode;
            string errmsg = json.errmsg;
            result.IsSuccess = errorcode == 0;
            result.ErrorCode = errorcode;
            result.Content = errmsg;
            return result;
        }

        internal static async Task<OperationResult> SendTextFile(string text)
        {
            OperationResult result = new OperationResult();
            string recData = await PostAsync(RobotUrl, getSendFileTypeJson(text));
            dynamic json = JsonConvert.DeserializeObject<dynamic>(recData);
            int errorcode = json.errcode;
            string errmsg = json.errmsg;
            result.IsSuccess = errorcode == 0;
            result.ErrorCode = errorcode;
            result.Content = errmsg;
            return result;
        }

        static string getTextTypeJson(string text)
        {
            var json = new
            {
                msgtype = "text",
                text = new
                {
                    content = text,
                }
            };
            return JsonConvert.SerializeObject(json);
        }

        static string getSendFileTypeJson(string media_id)
        {
            var json = new
            {
                msgtype = "file",
                file = new
                {
                    media_id
                }
            };
            return JsonConvert.SerializeObject(json);
        }

        static async Task<string> PostAsync(string url, string data)
        {
            byte[] postdata = Encoding.UTF8.GetBytes(data);
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = postdata.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(postdata, 0, postdata.Length);
            }
            string resData = "";
            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(responseStream))
                    {
                        resData = sr.ReadToEnd();
                    }
                }
            }
            return resData;
        }
    }

}
