﻿using analyze.core.Models.Rola;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocksSharp.Proxy;
using SocksSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using IPinfo;
using IPinfo.Models;
using Newtonsoft.Json.Schema;
using analyze.Models.Manage;
using System.Xml;
using NPOI.SS;
using HtmlAgilityPack;
using NPOI.OpenXmlFormats.Wordprocessing;

namespace analyze.core.Clients
{
    public class RolaClient
    {

        private static string[][] CSC;

        public Position RolaCheck(string country, string region = "", string city = "") 
        {
            if (CSC == null)
            {
                string filename = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                filename = Path.Combine(filename, "resources", "country-state-city.txt");
                var lines = File.ReadLines(filename).ToArray();
                CSC = new string[lines.Length][];
                for (int i = 0; i < lines.Length; i++)
                {
                    CSC[i] = lines[i].Split(',');
                }
            }
            Position position = new Position();
            if (!string.IsNullOrWhiteSpace(country))
            {
                position.Country = CSC.Where(c => country.ToLower().Equals(c[0])).First()[0];
            }

            if (!string.IsNullOrWhiteSpace(region))
            {
                position.Region = CSC.Where(c => region.ToLower().Equals(c[2])).First()[1];
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                position.City = CSC.Where(c => city.ToLower().Equals(c[3])).First()[3];
            }

            return position;
        }

        public async Task<string> RolaRefresh(string user, string country, string region = "", string city = "")
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // Make the GET request to the API endpoint
                    var response = await client.GetAsync($"http://refresh.rola.vip/refresh?user={user}&country={country}&state={region}&city={city}");

                    // Read the response as a stream
                    var responseString = await response.Content.ReadAsStringAsync();
                    var jo = (JObject)JsonConvert.DeserializeObject(responseString);
                    string ret = jo["Ret"].ToString();
                    string msg = jo["Msg"].ToString();
                    return ret;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Position> GetPosition(string proxyHost, int proxyPort, string username, string password)
        {
            return await Task.Run(() =>
            {
                try
                {
                    Process process = null;
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    processStartInfo.FileName = "curl.exe";
                    processStartInfo.Arguments = $"-x socks5h://{username}:{password}@{proxyHost}:{proxyPort} http://www.ip234.in/ip.json";
                    processStartInfo.UseShellExecute = false;
                    processStartInfo.RedirectStandardInput = true;
                    processStartInfo.RedirectStandardOutput = true;
                    processStartInfo.RedirectStandardError = false;
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.StandardOutputEncoding = Encoding.UTF8;
                    processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                    process = Process.Start(processStartInfo);

                    string resaultValue = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    process.Close();

                    var jo = (JObject)JsonConvert.DeserializeObject(resaultValue);
                    Position position = new Position();

                    if(jo.Property("data") != null)
                    {
                        jo = (JObject)JsonConvert.DeserializeObject(jo["data"].ToString());
                    }
                    position.IP = jo["ip"].ToString();
                    position.City = jo["city"].ToString();
                    position.Region = jo["region"].ToString();
                    position.Country = jo["country"].ToString();
                    return position;


                }
                catch (Exception ex)
                {
                    return null;
                }
            });

        }

        public async Task<Position> CheckIpPosition(string ip)
        {
            string token = "f646bfa38eb321";
            IPinfoClient client = new IPinfoClient.Builder()
                .AccessToken(token)
                .Build();

            IPResponse ipResponse = await client.IPApi.GetDetailsAsync(ip);
            Position position = new Position();
            position.IP = ip;
            position.City = ipResponse.City;
            position.Region = ipResponse.Region;
            position.Country = ipResponse.CountryName;

            return position;
        }

        public async Task<KeyValuePair<string, string>> CheckPost(string country, string post, string[] lines)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string uri = "";
                    if ("fr".Equals(country))
                    {
                        uri = $"https://www.nowmsg.com/findzip/fr_postal_code.asp?CityName={post}";
                    }
                    
                    //

                    // Make the GET request to the API endpoint
                    var response = await client.GetAsync(uri);

                    // Read the response as a stream
                    var responseString = await response.Content.ReadAsStringAsync();

                    HtmlDocument doc = new HtmlDocument();
          
                    doc.LoadHtml(responseString.Replace("\r\n", ""));

                    HtmlNode region = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[2]/div[4]/div[3]/div/table/tbody/tr/td[3]");
                    HtmlNode city = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[2]/div[4]/div[3]/div/table/tbody/tr/td[5]");
                    string regionText = region?.InnerText;
                    string cityText = city?.InnerText;

                    if( !string.IsNullOrWhiteSpace(regionText))
                    {
                        regionText = lines.Where(x => x.IndexOf(regionText, StringComparison.OrdinalIgnoreCase) >= 0).FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(regionText))
                        {
                            regionText = regionText.Split(",")[2];
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(cityText))
                    {
                        string ct = cityText.Replace("-", "");
                        cityText = lines.Where(x => x.IndexOf(ct, StringComparison.OrdinalIgnoreCase) >= 0 && x.Split(",")[3].Length == ct.Length).FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(cityText))
                        {
                            cityText = cityText.Split(",")[3];
                        }
                    }
                    if(!string.IsNullOrWhiteSpace(regionText) && !string.IsNullOrWhiteSpace(cityText))
                    {
                        return KeyValuePair.Create(regionText, cityText);
                    }
                    else
                    {
                        return KeyValuePair.Create("", "");
                    }
                }
            }
            catch (Exception ex)
            {
                return KeyValuePair.Create("", ""); ;
            }
        }
    }
}
