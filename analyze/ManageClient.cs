using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace analyze
{
    public class ManageClient
    {
        public Dictionary<string, Dictionary<string, string>> dicc = new Dictionary<string, Dictionary<string, string>>();

        private string Request(string method, string url, string body = "")
        {
            string backMsg;

            System.Net.HttpWebRequest httpRquest = (HttpWebRequest)HttpWebRequest.Create(url); 
            httpRquest.Method = method;
            httpRquest.KeepAlive = true;
            httpRquest.AllowAutoRedirect = false;
            httpRquest.Accept = "*/*";
            httpRquest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";
            
            // 设置cookies
            if ( dicc.ContainsKey(httpRquest.RequestUri.Host))
            {
                string str = "";
                foreach (var item in dicc[httpRquest.RequestUri.Host])
                {
                    str += $"{item.Key}={item.Value};";
                }
                httpRquest.Headers.Add("Cookie", str);
            }

            // 写body 
            httpRquest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            if( method.Equals("POST") && !string.IsNullOrWhiteSpace(body))
            {
                byte[] b = Encoding.Default.GetBytes(body);
                Stream stream = httpRquest.GetRequestStream();
                stream.Write(b, 0, b.Length);
                stream.Close();
            }


            httpRquest.CookieContainer = new CookieContainer();



            HttpWebResponse response = (HttpWebResponse)httpRquest.GetResponse();
            
            // 记录cookies
            if(response.Cookies.Count > 0)
            {
                if (!dicc.ContainsKey(httpRquest.RequestUri.Host)) 
                {
                    dicc[httpRquest.RequestUri.Host] = new Dictionary<string, string>();
                }
                foreach (Cookie cook in response.Cookies)
                {
                    dicc[httpRquest.RequestUri.Host][cook.Name] = cook.Value;
                    Console.WriteLine("{0}={1};Domain={2};Path={3}", cook.Name, cook.Value, cook.Domain, cook.Path);
                }
            }
            Stream responsestream = response.GetResponseStream(); 
            StreamReader reader = new StreamReader(responsestream, Encoding.UTF8);
            backMsg = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();
            responsestream.Close();
            responsestream.Dispose();
            return  backMsg;
        }
        public  void LoginAdminAsync()
        { 
            //登录后台
            Request("POST", "https://gzbf-admin.goodhmy.com/default/index/login", "userName=globaltradeez&userPass=gzbf_aaabbb123456");
        }

        public  void LoginUserAsync(string company_code)
        {

            // 获取登录链接
            string raw =  Request("POST", "https://gzbf-admin.goodhmy.com/customer/distributor/get-login-sass-url/", $"company_code={company_code}&site_code=all_platform");
            JObject jo = (JObject)JsonConvert.DeserializeObject(raw);

            //登录saas
            string url = jo["data"].ToString();
            raw = Request("GET", url);

            //登录用户
            string code = url.Substring(url.LastIndexOf("=")+1);
            raw = Request("GET", @"https://gzbf-shop.goodhmy.com/login.html?code=" + code + @"&redirect_url=https://erp.globaltradeez.com");
        }

        public void ListOrder(string status, params string[] orders)
        {
            string raw = Request("POST", @"https://gzbf-shop.goodhmy.com/auth/order/manage", Encode(status, orders));
        }
        private string Encode(string status, params string[] orders)
        {
            string raw = "";
            Array.ForEach(orders, o => raw += $"{o} ");
            string para = $"data[0][name]=search_type&data[0][value]=1&data[1][name]=country&data[1][value]=&data[2][name]=search_val&data[2][value]=&data[3][name]=order_code&data[3][value]={raw}&data[4][name]=time_type&data[4][value]=1&data[5][name]=time_start&data[5][value]=&data[6][name]=time_end&data[6][value]=&data[7][name]=platform_id&data[7][value]=&data[8][name]=platform_account_id&data[8][value]=&data[9][name]=type&data[9][value]=&data[10][name]=ot_id&data[10][value]=&data[11][name]=quantity_type&data[11][value]=&data[12][name]=can_combine&data[12][value]=&data[13][name]=buyer_message&data[13][value]=&data[14][name]=has_tracking_number&data[14][value]=&data[15][name]=is_cancel&data[15][value]=&data[16][name]=purchase_order_status&data[16][value]=&data[17][name]=is_ebay_message&data[17][value]=&data[18][name]=exception_code&data[18][value]=&data[19][name]=platform_site_id&data[19][value]=&data[20][name]=page&data[20][value]=1&data[21][name]=page_size&data[21][value]=2000&data[22][name]=status&data[22][value]={status}&data[23][name]=tag_id&data[23][value]=&data[24][name]=is_submit&data[24][value]=&data[25][name]=return_1688_order_status&data[25][value]=&data[26][name]=is_sub&data[26][value]=0&data[27][name]=sub_map_ids";
            return WebUtility.UrlEncode(para);
        }
    }
}
