﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace analyze.core
{
    public class WebhookClient
    {
        public string Webhook(string url, List<KeyValuePair<string, string>>  kvs)
        {
            string backMsg;

            System.Net.HttpWebRequest httpRquest = (HttpWebRequest)HttpWebRequest.Create(url);
            httpRquest.Method = "POST";
            httpRquest.KeepAlive = true;
            httpRquest.AllowAutoRedirect = false;
            httpRquest.Accept = @"application/json, text/javascript, */*; q=0.01";
            httpRquest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";

            // 写body 
            string message = "";
            foreach (var item in kvs)
            {
                message += $"{item.Key} {item.Value}\n";
            }

            string param = "{\"msgtype\":\"text\",\"text\":{\"content\":\"" + message + "\"}}";

            if (!string.IsNullOrWhiteSpace(param))
            {
                httpRquest.ContentType = "application/json";
                byte[] b = Encoding.Default.GetBytes(param);
                Stream stream = httpRquest.GetRequestStream();
                stream.Write(b, 0, b.Length);
                stream.Close();
            }

            httpRquest.CookieContainer = new CookieContainer();
            HttpWebResponse response = (HttpWebResponse)httpRquest.GetResponse();
            Stream responsestream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responsestream, Encoding.UTF8);
            backMsg = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();
            responsestream.Close();
            responsestream.Dispose();
            return backMsg;

        }
    }
}
