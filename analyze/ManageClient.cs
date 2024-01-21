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
        public string CookiesStr { get; private set; }
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
            string str = "";
            if ( dicc.ContainsKey(httpRquest.RequestUri.Host))
            {
                
                foreach (var item in dicc[httpRquest.RequestUri.Host])
                {
                    str += $"{item.Key}={item.Value};";
                }
                httpRquest.Headers.Add("Cookie", str);
            }
            //这行代码很关键，不设置contentType将导致后台参数获取不到值
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
            if(response.Cookies.Count > 0)
            {
                if (!dicc.ContainsKey(httpRquest.RequestUri.Host)) 
                {
                    dicc[httpRquest.RequestUri.Host] = new Dictionary<string, string>();
                }
                foreach (Cookie cook in response.Cookies)
                {
                    dicc[httpRquest.RequestUri.Host][cook.Name] = cook.Value;
                    CookiesStr += $"{cook.Name}={cook.Value};";
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
            

            string para = @"data%5B0%5D%5Bname%5D=search_type&data%5B0%5D%5Bvalue%5D=1&data%5B1%5D%5Bname%5D=country&data%5B1%5D%5Bvalue%5D=&data%5B2%5D%5Bname%5D=search_val&data%5B2%5D%5Bvalue%5D=&data%5B3%5D%5Bname%5D=order_code&data%5B3%5D%5Bvalue%5D=&data%5B4%5D%5Bname%5D=time_type&data%5B4%5D%5Bvalue%5D=1&data%5B5%5D%5Bname%5D=time_start&data%5B5%5D%5Bvalue%5D=&data%5B6%5D%5Bname%5D=time_end&data%5B6%5D%5Bvalue%5D=&data%5B7%5D%5Bname%5D=platform_id&data%5B7%5D%5Bvalue%5D=&data%5B8%5D%5Bname%5D=platform_account_id&data%5B8%5D%5Bvalue%5D=&data%5B9%5D%5Bname%5D=type&data%5B9%5D%5Bvalue%5D=&data%5B10%5D%5Bname%5D=ot_id&data%5B10%5D%5Bvalue%5D=&data%5B11%5D%5Bname%5D=quantity_type&data%5B11%5D%5Bvalue%5D=&data%5B12%5D%5Bname%5D=can_combine&data%5B12%5D%5Bvalue%5D=&data%5B13%5D%5Bname%5D=buyer_message&data%5B13%5D%5Bvalue%5D=&data%5B14%5D%5Bname%5D=has_tracking_number&data%5B14%5D%5Bvalue%5D=&data%5B15%5D%5Bname%5D=is_cancel&data%5B15%5D%5Bvalue%5D=&data%5B16%5D%5Bname%5D=purchase_order_status&data%5B16%5D%5Bvalue%5D=&data%5B17%5D%5Bname%5D=is_ebay_message&data%5B17%5D%5Bvalue%5D=&data%5B18%5D%5Bname%5D=exception_code&data%5B18%5D%5Bvalue%5D=&data%5B19%5D%5Bname%5D=platform_site_id&data%5B19%5D%5Bvalue%5D=&data%5B20%5D%5Bname%5D=page&data%5B20%5D%5Bvalue%5D=1&data%5B21%5D%5Bname%5D=page_size&data%5B21%5D%5Bvalue%5D=20&data%5B22%5D%5Bname%5D=status&data%5B22%5D%5Bvalue%5D=3&data%5B23%5D%5Bname%5D=tag_id&data%5B23%5D%5Bvalue%5D=&data%5B24%5D%5Bname%5D=is_submit&data%5B24%5D%5Bvalue%5D=&data%5B25%5D%5Bname%5D=return_1688_order_status&data%5B25%5D%5Bvalue%5D=&data%5B26%5D%5Bname%5D=is_sub&data%5B26%5D%5Bvalue%5D=0&data%5B27%5D%5Bname%5D=sub_map_ids";
            raw = Request("POST", @"https://gzbf-shop.goodhmy.com/auth/order/manage", para);


        }

    }
}
