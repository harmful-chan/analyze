using analyze.core.Models.Manage;
using analyze.Models.Manage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.XWPF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Clients
{
    public class ManageClient
    {
        public Dictionary<string, Dictionary<string, string>> dicc = new Dictionary<string, Dictionary<string, string>>();
        CookieContainer cookies = new CookieContainer();

        public bool IsLoginAdmin { get; private set; } = false;
        public string CurrentClientID { get; private set; }


        private string Request(string method, string url,string body = "")
        {
            string backMsg;

            HttpWebRequest httpRquest = (HttpWebRequest)WebRequest.Create(url);
            //httpRquest.Proxy = new WebProxy("http://localhost:8866");
            httpRquest.Method = method;
            httpRquest.KeepAlive = true;
            httpRquest.AllowAutoRedirect = true;
            httpRquest.Headers.Add("X-Requested-With", "XMLHttpRequest");
            //httpRquest.Accept = @"*/*";
            httpRquest.Accept = @"application/json, text/javascript, */*; q=0.01";
            httpRquest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36";

            // 设置cookies
            if (dicc.ContainsKey(httpRquest.RequestUri.Host))
            {
                string str = "";
                foreach (var item in dicc[httpRquest.RequestUri.Host])
                {
                    str += $"{item.Key}={item.Value};";
                }
                httpRquest.Headers.Add("Cookie", str);
            }

            // 写body 
            if (method.Equals("POST") && !string.IsNullOrWhiteSpace(body))
            {
                httpRquest.ContentType = "application/x-www-form-urlencoded";
                byte[] b = Encoding.Default.GetBytes(body);
                Stream stream = httpRquest.GetRequestStream();
                stream.Write(b, 0, b.Length);
                stream.Close();
            }

            httpRquest.CookieContainer = new CookieContainer();

            var response = (HttpWebResponse)httpRquest.GetResponseAsync().Result;

            // 记录cookies
            if (response.Cookies.Count > 0)
            {
                if (!dicc.ContainsKey(httpRquest.RequestUri.Host))
                {
                    dicc[httpRquest.RequestUri.Host] = new Dictionary<string, string>();
                }
                foreach (Cookie cook in response.Cookies)
                {
                    dicc[httpRquest.RequestUri.Host][cook.Name] = cook.Value;
                    //Console.WriteLine("{0}={1};Domain={2};Path={3}", cook.Name, cook.Value, cook.Domain, cook.Path);
                }
            }
            Stream responsestream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responsestream, Encoding.UTF8);
            backMsg = reader.ReadToEnd();

            reader.Close();
            reader.Dispose();
            responsestream.Close();
            responsestream.Dispose();
            return backMsg;

        }

        private Task<string> RequestAsync(string method, string url, string body = "")
        {
            return Task.Run(() =>
            {
                return Request(method, url, body);
            });
        }

        private string Request2(string url, string body = "")
        {



            // 创建HttpClientHandler并启用HTTP/2
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            handler.DefaultProxyCredentials = CredentialCache.DefaultCredentials;
            //handler.UseProxy = true;
            //handler.Proxy = new WebProxy("http://127.0.0.1:8866");
            handler.CookieContainer = new CookieContainer();

            using (var httpClient = new HttpClient(handler))
            {
                httpClient.DefaultRequestVersion = HttpVersion.Version20;
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36");
                httpClient.DefaultRequestHeaders.Add("Accept", @"application/json, text/javascript, */*; q=0.01");
                httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                httpClient.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9,en-US;q=0.8,en;q=0.7");
                //httpClient.DefaultRequestHeaders.Add("Referer", @"https://gzbf-admin.goodhmy.com/payment/present/list?quick=135&menu_id=27");
                httpClient.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");
                httpClient.DefaultRequestHeaders.Add("dnt", "1");
                Uri uri = new Uri(url);
                // 设置cookies
                if (dicc.ContainsKey(uri.Host))
                {
                    string str = "";
                    foreach (var item in dicc[uri.Host])
                    {
                        str += $"{item.Key}={item.Value};";
                        // 添加Cookie
                        var cookie = new Cookie(item.Key, item.Value, "/", uri.Host);
                        handler.CookieContainer.Add(uri, cookie);
                    }
                }

                // 创建HTTP请求
                var content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");

                // 发送POST请求
                var response = httpClient.PostAsync(url, content).Result;

                // 处理响应
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = response.Content.ReadAsStringAsync().Result;
                    return responseBody;
                }
                else
                {
                    return null;
                }
            }
        }

        public void LoginAdmin()
        {
            if (!IsLoginAdmin)
            {
                //登录后台
                Request("POST", "https://gzbf-admin.goodhmy.com/default/index/login", "userName=globaltradeez&userPass=gzbf_aaabbb123456");
                IsLoginAdmin = true;
            }
        }

        public void LoginUser(string company_code)
        {
            if (!string.IsNullOrWhiteSpace(company_code) && !company_code.Equals(CurrentClientID))
            {
                // 获取登录链接
                string raw = Request("POST", "https://gzbf-admin.goodhmy.com/customer/distributor/get-login-sass-url/", $"company_code={company_code}&site_code=all_platform");
                JObject jo = (JObject)JsonConvert.DeserializeObject(raw);

                //登录saas
                string url = jo["data"].ToString();
                raw = Request("GET", url);


                //登录用户
                string code = url.Substring(url.LastIndexOf("=") + 1);
                raw = Request("GET", @"https://gzbf-shop.goodhmy.com/login.html?code=" + code + @"&redirect_url=https://erp.globaltradeez.com");
                CurrentClientID = company_code;
            }
        }

        public async Task<User[]> ListUsersAsync(string clientId = "")
        {
            int page = 0, page_size = 20, total = 0;

            string raw = await RequestAsync("POST", @"https://gzbf-admin.goodhmy.com/customer/distributor/list", EncodeUser(1, 2000, clientId));
            var jo = (JObject)JsonConvert.DeserializeObject(raw);

            string p = jo["data"]["page"].ToString();
            string ps = jo["data"]["page_size"].ToString();
            string to = jo["data"]["total"].ToString();

            int.TryParse(p, out page);
            int.TryParse(ps, out page_size);
            int.TryParse(to, out total);

            var rows = jo["data"]["rows"].ToString();
            User[] users = JsonConvert.DeserializeObject<User[]>(rows);
            return users;
        }

        public User[] ListUsers(string clientId = "")
        {
            int page = 0, page_size = 20, total = 0;

            string raw = Request("POST", @"https://gzbf-admin.goodhmy.com/customer/distributor/list", EncodeUser(1, 2000, clientId));
            var jo = (JObject)JsonConvert.DeserializeObject(raw);

            string p = jo["data"]["page"].ToString();
            string ps = jo["data"]["page_size"].ToString();
            string to = jo["data"]["total"].ToString();

            int.TryParse(p, out page);
            int.TryParse(ps, out page_size);
            int.TryParse(to, out total);

            var rows = jo["data"]["rows"].ToString();
            User[] users = JsonConvert.DeserializeObject<User[]>(rows);
            return users;
        }



        public Order[] ListOrder(params string[] orders)
        {
            List<Order> list = new List<Order>();
            string raw = Request("POST", @"https://gzbf-shop.goodhmy.com/auth/order/manage", EncodeOrder(1, orders));
            var jo = (JObject)JsonConvert.DeserializeObject(raw);
            var counts = jo["data"]["counts"].ToString();
            OrderHeader orderHeader = JsonConvert.DeserializeObject<OrderHeader>(counts);
            if(orderHeader.T1.Count > 0)
            {
                var rows = jo["data"]["rows"].ToString();
                Order[] orderList= JsonConvert.DeserializeObject<Order[]>(rows);
                foreach (var item in orderList)
                {
                    item.Status = 1;
                    list.Add(item);
                }

            }

            string raw3 = Request("POST", @"https://gzbf-shop.goodhmy.com/auth/order/manage", EncodeOrder(3, orders));
            var jo3 = (JObject)JsonConvert.DeserializeObject(raw3);
            var counts3 = jo["data"]["counts"].ToString();
            OrderHeader orderHeader3 = JsonConvert.DeserializeObject<OrderHeader>(counts3);
            if (orderHeader3.T3.Count > 0)
            {
                var rows = jo3["data"]["rows"].ToString();
                Order[] orderList = JsonConvert.DeserializeObject<Order[]>(rows);
                foreach (var item in orderList)
                {
                    item.Status = 3;
                    list.Add(item);
                }

            }

            
            return list.ToArray();
        }

        public DebitRecord[] ListDebitRecord(string clientId="", string starTime = "", string endTime = "")
        {
            string str = $"arn_status=&customer_code=&cu_name=&cu_type=&start_add_time=&end_add_time=&start_finish_time={starTime}&end_finish_time={endTime}&page=1&limit=2000&transaction_no=";

            string raw = Request2(@"https://gzbf-admin.goodhmy.com/payment/present/list", str);
            var jo = (JObject)JsonConvert.DeserializeObject(raw);
            var rows = jo["data"].ToString();
            DebitRecord[] debitRecords = JsonConvert.DeserializeObject<DebitRecord[]>(rows);

            return debitRecords;
        }

        #region 发货操作
        public string Deduction(Order order)
        {
            string str = $"payment_method=BANK&payment_date={DateTime.Now.ToString("yyyy-MM-dd+HH:mm:ss")}&payment_amount={order.Cost}&beneficiary_name=Wal-Mart+(China)+Inv&beneficiary_bank=中国建设银行股份有限&beneficiary_sub_bank=Community&beneficiary_account_number=44050110069700001060&payment_currency=RMB";
            string raw = Request("POST", @"https://gzbf-shop.goodhmy.com/fee/account-refund-note/save", str);
            var jo = (JObject)JsonConvert.DeserializeObject(raw);
            var rows = jo["message"].ToString();
            return rows;
        }

        public string DeductionYes(int checkId)
        {
            string raw = Request("POST", @"https://gzbf-admin.goodhmy.com/payment/present/yes?status=1", $"checkId={checkId}");
            var jo = (JObject)JsonConvert.DeserializeObject(raw);
            var rows = jo["message"].ToString();
            return rows;
        }
        public string DeductionPassed(int checkId)
        {
            string raw = Request("POST", @"https://gzbf-admin.goodhmy.com/payment/present/passed", $"checkId={checkId}");
            var jo = (JObject)JsonConvert.DeserializeObject(raw);
            var rows = jo["message"].ToString();
            return rows;
        }

        public string Shipments(Order order)
        {
            string raw = Request("POST", @"https://gzbf-shop.goodhmy.com/auth/order/manual-shipped", $"send_data[ids][]={order.Id}");
            var jo = (JObject)JsonConvert.DeserializeObject(raw);
            var rows = jo["data"]["success_num"].ToString();
            return rows;
        }

        public MarkOrder[] ListMarkOrders(params string[] orderIds)
        {
            string raw = Request("POST", @"https://gzbf-shop.goodhmy.com/auth/shipping/index", EncodeMarkOrder(orderIds));
            var jo = (JObject)JsonConvert.DeserializeObject(raw);
            var rows = jo["data"]["rows"].ToString();
            MarkOrder[] orderList = JsonConvert.DeserializeObject<MarkOrder[]>(rows);
            return orderList;
        }

        public string MarkOrder(int id, int step, string carrier, string trackingNumber, string carrierOld, string trackingNumberOld)
        {
            string param = "";
            string raw = null;
            if (step == 1)
            {
                param = $"data[orders][0][id]={id}&" +
                    $"data[orders][0][platform_carrier_code]={carrier.ToLower()}&" +
                    $"data[orders][0][send_date]={DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}&" +
                    $"data[orders][0][shipping_url]=https://t.17track.net/en#nums={trackingNumber}&" +
                    $"data[orders][0][tracking_number]={trackingNumber}";

                raw = Request("POST", @"https://gzbf-shop.goodhmy.com/auth/shipping/save-service", param);
                var jo = (JObject)JsonConvert.DeserializeObject(raw);
                raw = jo["success_num"].ToString();
            }
            else if (step == 2)
            {
                param = $"data[orders][0][id]={id}&" +
                    $"data[orders][0][platform_carrier_code]={carrier.ToLower()}&" +
                    $"data[orders][0][send_date]={DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}&" +
                    $"data[orders][0][shipping_url]=https://t.17track.net/en#nums={trackingNumber}&" +
                    $"data[orders][0][tracking_number]={trackingNumber}&" +
                    $"data[orders][0][is_all]=0&" +
                    $"data[orders][0][platform_carrier_code_old]={carrierOld.ToLower()}&" +
                    $"data[orders][0][tracking_number_old]={trackingNumberOld}";

                raw = Request("POST", @"https://gzbf-shop.goodhmy.com/auth/shipping/update-service", param);
                var jo = (JObject)JsonConvert.DeserializeObject(raw);
                raw = jo["success_num"].ToString();
            }

            return raw;
        }

        #endregion

        public async Task<Recharge[]> ListAllRechargeAsync()
        {

            string str = $"cu_type=&cu_id=&reference_no=&pm_code=&pn_fee_type=&pn_status=&customer_code=&cu_code=&cu_type=&pn_add_time=&pn_verify_time=&pending=&page=1&limit=2000";
            string raw = await RequestAsync("POST", @"https://gzbf-admin.goodhmy.com/payment/payment/list", str);
            var jo = (JObject)JsonConvert.DeserializeObject(raw);
            var rows = jo["data"]["rows"].ToString();
            var recharge = JsonConvert.DeserializeObject<Recharge[]>(rows);
            return recharge;

        }



        private string EncodeMarkOrder(params string[] orders)
        {
            string raw = "";
            Array.ForEach(orders, o => raw += $" {o}");
            if (orders.Length > 0)
            {
                raw = raw.Remove(0, 1);
            }
            string para = $"data[0][name]=platform_id&data[0][value]=&data[1][name]=account_id&data[1][value]=&" +
                $"data[2][name]=sync_status&data[2][value]=&data[3][name]=buyer_message&data[3][value]=&" +
                $"data[4][name]=has_tracking_number&data[4][value]=&data[5][name]=is_split&data[5][value]=&" +
                $"data[6][name]=is_update_mark&data[6][value]=&data[7][name]=status&data[7][value]=&" +
                $"data[8][name]=search_type&data[8][value]=1&data[9][name]=search_val&data[9][value]={raw}&" +
                $"data[10][name]=page&data[10][value]=1&data[11][name]=page_size&data[11][value]=20&" +
                $"data[12][name]=paid_time_start&data[12][value]=&data[13][name]=paid_time_end&data[13][value]=&" +
                $"data[14][name]=over_time_left_start&data[14][value]=&data[15][name]=over_time_left_end&data[15][value]=&" +
                $"data[16][name]=is_sub&data[16][value]=0&data[17][name]=sub_map_ids";
            return para;
        }

        private string EncodeOrder(int status, params string[] orders)
        {
            string raw = "";
            Array.ForEach(orders, o => raw += $" {o}");
            if (orders.Length > 0)
            {
                raw  = raw.Remove(0, 1);
            }
            string para = $"data[0][name]=search_type&data[0][value]=1&data[1][name]=country&data[1][value]=&" +
                $"data[2][name]=search_val&data[2][value]={raw}&data[3][name]=order_code&data[3][value]=&" +
                $"data[4][name]=time_type&data[4][value]=1&data[5][name]=time_start&data[5][value]=&" +
                $"data[6][name]=time_end&data[6][value]=&data[7][name]=platform_id&data[7][value]=&" +
                $"data[8][name]=platform_account_id&data[8][value]=&data[9][name]=type&data[9][value]=&" +
                $"data[10][name]=ot_id&data[10][value]=&data[11][name]=quantity_type&data[11][value]=&" +
                $"data[12][name]=can_combine&data[12][value]=&data[13][name]=buyer_message&data[13][value]=&" +
                $"data[14][name]=has_tracking_number&data[14][value]=&data[15][name]=is_cancel&data[15][value]=&" +
                $"data[16][name]=purchase_order_status&data[16][value]=&data[17][name]=is_ebay_message&data[17][value]=&" +
                $"data[18][name]=exception_code&data[18][value]=&data[19][name]=platform_site_id&data[19][value]=&" +
                $"data[20][name]=page&data[20][value]=1&data[21][name]=page_size&data[21][value]=2000&" +
                $"data[22][name]=status&data[22][value]={status}&data[23][name]=tag_id&data[23][value]=&" +
                $"data[24][name]=is_submit&data[24][value]=&data[25][name]=return_1688_order_status&data[25][value]=&" +
                $"data[26][name]=is_sub&data[26][value]=0&data[27][name]=sub_map_ids";
            return para;
        }

        private string EncodeUser(int page, int pagesize, string clientId = "")
        {

            string para = $"search_type=company_code_arr&search_val={clientId}&cu_type=1&recommended_code=&referrer_code=&company_code=&role_type=&sales_agent_id=&business_person_id=&account_manager_id=&email=&phone=&company_name=&client_grade_id=&company_status=&createStartTime=&createEndTime=&verifyStartTime=&verifyEndTime=&loginStartTime=&loginEndTime=&cb_value_start=&cb_value_end=&erp_balance_value_from=&erp_balance_value_to=&content_desc_like=&page={page}&pageSize={pagesize}";
            //return WebUtility.UrlEncode(para);
            return para;
        }


    }
}
