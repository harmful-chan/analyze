using analyze.core.Clients;
using analyze.core.Models.Daily;
using analyze.core.Models.Manage;
using analyze.core.Models.Sheet;
using analyze.core.Options;
using analyze.core.Outputs;
using analyze.Models.Manage;
using ConsoleTables;
using IPinfo.Models;
using NPOI.HSSF.Record;
using NPOI.OpenXmlFormats.Shared;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using static analyze.core.Analyzer;
using static analyze.core.Clients.SheetClient;
using static System.Formats.Asn1.AsnWriter;

namespace analyze.core
{


    public delegate void LogDelegate(string arg="", bool isOnline = false);
    public class Analyzer
    {
        #region 内部类
        public class Profit
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public string CN { get; set; }
            public string CompanyName { get; set; }
            public string Nick { get; set; }
            public double Lend { get; set; }
            public double Cost { get; set; }
            public double Value { get; set; }
            public double Rate { get; set; }
        }
        #endregion


        #region 属性

        public bool IsRunning { get; private set; } = false;
        public string Root { get; set; }
        public string ShopInfoFileName { get; private set; }
        public string NewestBinFileName { get; private set; }
        public string NewestOrderDirectory { get; private set; }
        public string ShopRecordFileName { get; private set; }
        public string OrderRecordFileName { get; private set; }
        public string UsPruchasRecordFileName { get; private set; }
        public string BrPruchasRecordFileName { get; private set; }


        public string NewestDeductionDirectory { get; private set; }
        public string NewestProfitDirectory { get; private set; }
        public string NewestDailyDirectory { get; private set; }
        public string NewestReparationDirectory { get; private set; } 
        public string NewestTotalDirectory { get; private set; }
        public string NewestResourcesDirectory { get; private set; }
        #endregion

        #region 实例化
        SheetClient _sheetClient = null;

        private IOutput _output;

        public IOutput Output
        {
            get { return _output; }
            set 
            {
                _sheetClient.Output = value;
                _output = value; 
            }
        }
        public Analyzer()
        {
            _sheetClient = new SheetClient();
        }

        #endregion




        #region 根据输入的订单行为匹配不同订单处理动作 
        public void OrderRun(OrderOptions o)
        {
            if (o.action.Equals("refund"))
            {
                //RefundRun(o);
            }else if (o.action.Equals("lend"))
            {
                //LendRun(o);
            }
        }

        #endregion

        #region 放款
        double[] randoms = [0.8753,   0.8352,   0.6807,   0.6998,   0.7895,   0.7809,   0.5627,   0.5281,   0.4465,   0.8804,   0.7130,   0.6523,   0.5964,   0.6810,   0.6780,   0.8286,  0.6819,   0.6674,   0.4785,   0.6083];
        static int randomCount = 0;
        public double GetRandom()
        {
            return randoms[randomCount++%randoms.Length];
        }
        public void ResetRandom()
        {
            randomCount = 0;
        }

        public void FillProfitForOneMonth(int year, int month, ShopRecord shop)
        {
            ResetRandom();
            DateTime t1, t2;
            DateRange(year, month, out t1, out t2);
            ShopLend[] sureLends = shop.ShopLendList.Where(f => f.SettlementTime <= t2 && f.SettlementTime >= t1).ToArray();
            foreach (var r in sureLends)  // 每一个订单
            {
                bool flag1 = true;
                bool flag2 = true;
                bool flag3 = true;
                bool flag4 = true;

                ShopOrder[] shopOrders = shop.ShopOrderList.Where(t => t.OrderId.Equals(r.OrderId)).ToArray();
                Models.Sheet.Order[] torders = _sheetClient.TotalOrders.Where(t => t.OrderId != null && t.OrderId.Contains(r.OrderId)).ToArray();
                Purchase[] porder = _sheetClient.TotalPurchases.Where(t => t.OrderId != null && t.OrderId.Equals(r.OrderId)).ToArray();

                string ot = shopOrders.FirstOrDefault()?.OrderTime.ToString("yyyy-MM-dd HH:mm:ss");
                string pt = shopOrders.FirstOrDefault()?.PaymentTime.ToString("yyyy-MM-dd HH:mm:ss");
                string st = shopOrders.FirstOrDefault()?.ShippingTime.ToString("yyyy-MM-dd HH:mm:ss");
                string rt = shopOrders.FirstOrDefault()?.ReceiptTime.ToString("yyyy-MM-dd HH:mm:ss");
                if (shopOrders.FirstOrDefault()?.OrderTime == DateTime.MinValue) ot = "";
                if (shopOrders.FirstOrDefault()?.PaymentTime == DateTime.MinValue) pt = "";
                if (shopOrders.FirstOrDefault()?.ShippingTime == DateTime.MinValue) st = "";
                if (shopOrders.FirstOrDefault()?.ReceiptTime == DateTime.MinValue) rt = "";

                string str = (flag1 ? "" : "1") + (flag2 ? "" : "2") + (flag3 ? "" : "3") + (flag4 ? "" : "4");
                if (torders.Length > 0)
                {
                    double deductionAmount = torders.FirstOrDefault().DeductionAmount;
                    if (deductionAmount < 1)
                    {
                        double lend = r.Lend;
                        deductionAmount = lend * GetRandom();
                    }
                    r.Cost = Math.Round(deductionAmount, 2);

                }
                else
                {
                    r.Cost = 0.0;
                }

                r.Profit = Math.Round(r.Lend - r.Cost, 2);
                r.Rate = Math.Round((r.Profit) / r.Cost, 2);

            }
        }
        public Profit StatisticalProfit(int year, int month, ShopRecord shop)
        {
            DateTime t1, t2;
            DateRange(year, month, out t1, out t2);
            ShopLend[] sureLends = shop.ShopLendList.Where(f => f.SettlementTime <= t2 && f.SettlementTime >= t1).ToArray();
            double lend = 0.0, cost = 0.0, profit = 0.0, rate = 0.0;
            System.Array.ForEach(sureLends, a =>
            {
                lend += a.Lend;
                cost += a.Cost;
                profit += a.Profit;
                rate += a.Rate;
            });
            int len = sureLends.Length;
            return new Profit()
            {
                Lend = lend,
                Cost = cost,
                Value = profit,
                Rate = profit / cost

            };
        }
        public ShopLend[] GetOneMonthLend(int year, int month, ShopRecord shop)
        {
            DateTime t1, t2;
            DateRange(year, month, out t1, out t2);
            ShopLend[] sureLends = shop.ShopLendList.Where(f => f.SettlementTime <= t2 && f.SettlementTime >= t1).ToArray();
            return sureLends;
        }

        public void ShowOneMonthLend(int year, int month, ShopRecord shop, Profit profit)
        {

            _output.WriteLine($"---------------------{year}-{month:D2}-{Math.Round(profit.Lend, 2)}-{Math.Round(profit.Cost, 2)}-{Math.Round(profit.Value, 2)}-{Math.Round(profit.Rate, 2)}-----------------");
            DateTime t1, t2;
            DateRange(year, month, out t1, out t2);
            ShopLend[] sureLends = shop.ShopLendList.Where(f => f.SettlementTime <= t2 && f.SettlementTime >= t1).ToArray();
            int i = 1;
            foreach (var lend in sureLends)
            {
                _output.Write(i++.ToString("D2"), false);
                _output.Write(" "+lend.OrderId, false);
                _output.Write($" {Math.Round(lend.Lend, 2)}", false);
                _output.Write(" " + Math.Round(lend.Cost, 2), false);
                _output.Write(" " + Math.Round(lend.Profit, 2), false);
                _output.Write(" " + Math.Round(lend.Rate, 2), false);
                _output.Write("\r\n", false);
            }
        }
        public void SaveProfit(string filename, ShopLend[] shopLends)
        {
            System.Array.Sort(shopLends, (x, y) => { return x.SettlementTime.CompareTo(y.SettlementTime); });
            _sheetClient.SaveShopLend(filename, shopLends, Path.Combine(NewestResourcesDirectory, "lend.xlsx"),  true);
        }

        #endregion

        #region 退款


        public void FillRefundForOneMonth(int year, int month, ShopRecord shop)
        {
            DateTime t1, t2;
            int index = 1;
            DateRange(year, month, out t1, out t2);
            ShopRefund[] sureRefunds = shop.ShopRefundList.Where(f => f.RefundTime <= t2 && f.RefundTime >= t1).ToArray();
            foreach (var r in sureRefunds)
            {
                bool flag1 = true;
                bool flag2 = true;
                bool flag3 = true;
                bool flag4 = true;

                ShopOrder[] shopOrders = shop.ShopOrderList.Where(t => t.OrderId.Equals(r.OrderId)).ToArray();
                Models.Sheet.Order[] torders = _sheetClient.TotalOrders.Where(t => t.OrderId != null && t.OrderId.Equals(r.OrderId)).ToArray();
                Purchase[] porder = _sheetClient.TotalPurchases.Where(t => t.OrderId != null && t.OrderId.Equals(r.OrderId)).ToArray();
                if (r.Turnover != r.Refund)    // 放款退款不同
                {
                    flag1 = false;
                }
                if (torders.Length > 0 && torders.First().Cost != torders.First().DeductionAmount)  // 分销扣款不同
                {
                    flag2 = (Math.Round(torders.First().Cost, 1) == Math.Round(torders.First().DeductionAmount, 1));
                }

                if (torders.Length > 0)  // 交易号不相同
                {
                    flag3 = !(torders.GroupBy(t => t.TradeId).Count() > 1);
                }
                if (porder.Length > 0)  // 交易号不相同
                {
                    flag4 = !(porder.Where(p => p.Status != null && p.Status.Contains("已发货")).Count() > 0);
                }

                string a = r.RefundTime.ToString("yyyy-MM-dd HH:mm:ss");
                string ot = shopOrders.FirstOrDefault()?.OrderTime.ToString("yyyy-MM-dd HH:mm:ss");
                string pt = shopOrders.FirstOrDefault()?.PaymentTime.ToString("yyyy-MM-dd HH:mm:ss");
                string st = shopOrders.FirstOrDefault()?.ShippingTime.ToString("yyyy-MM-dd HH:mm:ss");
                string rt = shopOrders.FirstOrDefault()?.ReceiptTime.ToString("yyyy-MM-dd HH:mm:ss");
                if (shopOrders.FirstOrDefault()?.OrderTime == DateTime.MinValue) ot = "";
                if (shopOrders.FirstOrDefault()?.PaymentTime == DateTime.MinValue) pt = "";
                if (shopOrders.FirstOrDefault()?.ShippingTime == DateTime.MinValue) st = "";
                if (shopOrders.FirstOrDefault()?.ReceiptTime == DateTime.MinValue) rt = "";

                string str = (flag1 ? "" : "1") + (flag2 ? "" : "2") + (flag3 ? "" : "3") + (flag4 ? "" : "4");
                if (torders.Length > 0)
                {

                    _output.AddRow(index++, r.OrderId, a, r.Turnover, r.Refund, torders.FirstOrDefault()?.Cost, torders.FirstOrDefault()?.DeductionAmount, torders.FirstOrDefault().TradeId, str);
                }
                else
                {
                    _output.AddRow(index++, r.OrderId, a, r.Turnover, r.Refund,
                        "", "", "", str);
                }
                r.Trade = str;
                // 退款理由：纠纷退款， 未发货退款，纠纷退款。
                if (torders.Length > 0)
                {
                    Models.Sheet.Order to = torders.FirstOrDefault();
                    r.RefundReason = "未发货退款";
                    if (!string.IsNullOrWhiteSpace(to.TradeId))  // 有交易号，有发货之间 ： 纠纷退款
                    {
                        ShopOrder so = shopOrders.FirstOrDefault();
                        if (so.ShippingTime > DateTime.MinValue)    // 有发货
                        {
                            r.RefundReason = "纠纷退款";
                        }
                    }

                    r.DebitOperation = "已扣款";
                    r.Deduction = to.DeductionAmount;
                }
                else
                {
                    r.RefundReason = "取消订单";
                    r.DebitOperation = "未扣款";
                }
            }
        }
        public ShopRefund[] GetOneMonthRefund(int year, int month, ShopRecord shop)
        {
            DateTime t1, t2;
            DateRange(year, month, out t1, out t2);
            ShopRefund[] sureRefunds = shop.ShopRefundList.Where(f => f.RefundTime <= t2 && f.RefundTime >= t1).ToArray();
            return sureRefunds;
        }
        public void ShowOneMonthRefund()
        {
            string[] objs = ["I", "Order", "RefundTime", "Turnover", "Refund", "Cost", "Deduction", "TradeId", "Status"];

            _output.Show(objs);
        }
        public void SaveRefund(string filename, ShopRefund[] sureRefunds)
        {

            System.Array.Sort(sureRefunds, (x, y) => { return x.RefundTime.CompareTo(y.RefundTime); });
            _sheetClient.SaveShopRefund(filename, sureRefunds, Path.Combine(NewestResourcesDirectory, "refund.xlsx"), true);
        }

        #endregion
         
        #region daily
        public enum OrderTypes
        {
            BeforeOneDay,
            Yesterday,
            Ready,
            Wait,
            NotPay,
            Cancel,
            Timeout,
            Dispute,
            Finish
        }

        public enum DisputeTypes
        {
            Talk,
            Platform,
            Close,
        }

        public Dictionary<DisputeTypes, IEnumerable<Dispute>> ClassifiedDispute(IEnumerable<Dispute> disputes)
        {
            var talk = disputes.Where(o => o.Status.Contains("方案协商中"));
            var plat = disputes.Where(o => o.Status.Contains("平台介入处理中"));
            var close = disputes.Where(o => o.Status.Contains("已结束"));

            var dic = new Dictionary<DisputeTypes, IEnumerable<Dispute>>();
            dic[DisputeTypes.Talk] = talk;
            dic[DisputeTypes.Platform] = plat;
            dic[DisputeTypes.Close] = close;

            return dic;
        }

        public Dictionary<OrderTypes, IEnumerable<OrderDetail>> ClassifiedOrders(IEnumerable<OrderDetail> orderDetails)
        {
            var yesterday = orderDetails.Where(o => o.OrderTime.Date == DateTime.Now.AddDays(-1).Date );
            var before = orderDetails.Where(o => o.OrderTime.Date == DateTime.Now.AddDays(-2).Date);
            var ready = orderDetails.Where(o => o.Status.Contains("等待您发货") || o.Status.Contains("等待发货"));
            var wait = orderDetails.Where(o => o.Status.Contains("等待买家收货"));
            var notpay = orderDetails.Where(o => o.Status.Contains("等待买家付款"));
            var cancel = orderDetails.Where(o=> o.After == null ? false: o.After.Equals("已取消"));
            var timeout = orderDetails.Where(o => o.After == null && o.Status.Equals("订单关闭"));
            var dispute = orderDetails.Where(o => o.After != null && o.After.Contains("纠纷"));
            var finish = orderDetails.Where(o => o.Status.Contains("交易完成"));

            Dictionary<OrderTypes, IEnumerable<OrderDetail>> dic = new Dictionary<OrderTypes, IEnumerable<OrderDetail>>();
            dic[OrderTypes.BeforeOneDay] = before;
            dic[OrderTypes.Yesterday] = yesterday;
            dic[OrderTypes.Wait] = wait;
            dic[OrderTypes.Ready] = ready;
            dic[OrderTypes.NotPay] = notpay;
            dic[OrderTypes.Cancel] = cancel;
            dic[OrderTypes.Timeout] = timeout;
            dic[OrderTypes.Dispute] = dispute;
            dic[OrderTypes.Finish] = finish;

            return dic;
        }

        public IEnumerable<KeyValuePair<string, string>> CreateDetail(params IEnumerable<OrderDetail>[] orderDetailss)
        {
            List<KeyValuePair<string, string>> dic = new List<KeyValuePair<string, string>>();
            foreach (var orderDetails in orderDetailss)
            {
                foreach (var item in orderDetails)
                {
                    var kv = KeyValuePair.Create(item.OrderTime.ToString("yyyy-MM-dd"), $"{item.Symbol} {item.Amount}");
                    dic.Add(kv);
                }
            }
            return dic;
        }
        public KeyValuePair<string, string> CreateOverview(string name, params IEnumerable<OrderDetail>[] orderDetailss)
        {
            double total = 0.0;
            double count = 0;
            foreach (var orderDetails in orderDetailss)
            {
                double t = 0.0;
                foreach (var item in orderDetails)
                {
                    if (item.Symbol.Contains("R"))
                    {
                        t += item.Amount;
                        count++;
                    }
                }
                total += t;

            }
            return  KeyValuePair.Create(name, $"{count} 单 总R$ {total:0.00}");
        }

        public KeyValuePair<int, double> CreateNumber(params IEnumerable<OrderDetail>[] orderDetailss)
        {
            double total = 0.0;
            int count = 0;
            foreach (var orderDetails in orderDetailss)
            {
                double t = 0.0;
                foreach (var item in orderDetails)
                {
                    if (item.Symbol.Contains("R"))
                    {
                        t += item.Amount;
                        count++;
                    }
                }
                total += t;
            }
            return KeyValuePair.Create(count, total);
        }


        public void DailyRun(DailyOptions o)
        {
            List<Shop> shops = new List<Shop>();
            List<DailyDetail> dailys = new List<DailyDetail>();
            SheetClient client = new SheetClient();
            client.Output = _output;
            ManageClient manageClient = new ManageClient();
            IList<Purchase> brPurchases = null;
            // 获取目录文件
            if(Directory.Exists(o.FileDir))
            {
                string[] files = Directory.GetFiles(o.FileDir);
                o.DailyFiles = files;
            }


            User[] users = null;
            Recharge[] recharges = null;
            Task<bool> task = Task.Run(async () =>
            {
                manageClient.LoginAdmin();
                users = await manageClient.ListUsersAsync();
                recharges = await manageClient.ListAllRechargeAsync();
                return true;
            });

            // 循环读取文件
            if (o.DailyFiles.Count() > 0)
            {
                foreach (string filename in o.DailyFiles)
                {
                    if(!Path.GetFileName(filename).StartsWith("~"))
                    {
                        DailyDetail daily = client.ReadDaily(filename);
                        dailys.Add(daily);
                    }
                }
                dailys.Sort();

                if (File.Exists(o.ShopInfoFileName))
                {
                    shops = client.ReadShopInfo(o.ShopInfoFileName).ToList();

                    var runShop = shops.Where(s => s.Status.Equals("运营中")).ToArray();
                    _output.WriteLine($"运营中：{runShop.Count()} 读取数量：{o.DailyFiles.Count()}");

                    string[] arr = System.Array.ConvertAll(runShop.ToArray(), r => r.CN);

                    foreach (var shop in runShop)
                    {
                        bool flag = false;
                        foreach (string filename in o.DailyFiles)
                        {
                            if (filename.Contains(shop.CN))
                            {
                                flag = true;
                                break;
                            }
                        }
                        if (!flag)
                        {
                            _output.WriteLine(shop.CompanyNumber + shop.CN + shop.CompanyName + shop.Nick);

                        }
                    }
                }
                
            }

            // 判断店铺是否获取完毕
            if (o.DailyFiles.Count() > 0)
            {


            }

            if (File.Exists(o.BrPurchaseFilenmae))
            {
                brPurchases = client.ReadPurchase(1, o.BrPurchaseFilenmae);
            }

            task.Wait();

            _output.WriteLine();

            List<KeyValuePair<string, string>> uploadDic = new List<KeyValuePair<string, string>>();
            double total1 = 0.0;
            int count1 = 0;
            double total2 = 0.0;
            int count2 = 0;
            DailyDetail old = null;

            Dictionary<string, List<object[]>> objDic = new Dictionary<string, List<object[]>>();
            string[] listHeader = new string[] { "Company", "CN", "Opera", "UP", "Check", "Down",
                "IM24", "Good", "Dispute", "Wrong",
                "Dispute Line", "F30", "D30", "Exp30", "Fin", "Dis", "Close", "Talk", "Palt",
                "All", "Ready Line", "New", "Ready", "Wait" ,
                "Lead", "Freeze", "OnWay", "Arre", "Lose", "Get", "Reality", "Balance"
                };



            foreach (var daily in dailys)
            {
                // 公司简称
                string name = daily.Company.Replace("市", "").Replace("县", "").Substring(2, 4);

                bool flag = (old == null || !old.Company.Contains(name.Substring(0, 4)));


                var orderDic = ClassifiedOrders(daily.OrderDetails);
                var disputeDic = ClassifiedDispute(daily.DisputeOrders);

                var max = DateTime.Now;
                var min = DateTime.Now.AddMonths(-1);
                int disCount30 = daily.DisputeOrders.Where(o => o.DisputeTime.CompareTo(min) >= 0 && o.DisputeTime.CompareTo(max) <= 0).Count();
                int finCount30 = orderDic[OrderTypes.Finish].Where(o => o.OrderTime.CompareTo(min) >= 0 && o.OrderTime.CompareTo(max) <= 0).Count();
                int disCount = daily.DisputeOrders.Count();
                int finCount = orderDic[OrderTypes.Finish].Count();

                double exp30 = (disCount30 / 0.3) - disCount30 - finCount30; 

                // 在途纠纷及拒付订单
                var onwayLose = daily.OnWayOrders.Where(o => o.Reason.Contains("纠纷中") || o.Reason.Contains("拒付中"));
                var loseAmount = onwayLose.Sum(o => o.Amount);

                // 实际充值 不是索赔，不是返点
                double reality = recharges.Where( r => r.CompanyName.Contains(daily.Company) && !r.Mark.Contains("返点") && !r.Mark.Contains("索赔") && !r.Mark.Contains("海外仓")).Sum(o => o.Amount);

                // 云仓余额
                double balance = users.Where(u => u.CompanyName.Contains(daily.Company)).FirstOrDefault().Balance;

                // 实际提现
                double withdraws = daily.Withdraws.Sum(o => o.Amount);

                // 纠纷最小处理天数
                var disputeLasts = daily.DisputeOrders.Where(d => d.LastTime != null);
                var disputeTime = System.Array.ConvertAll(disputeLasts.ToArray(), d => d.LastTime).Min();

                // 待发货最小处理天数
                var orderLasts = daily.OrderDetails.Where(d => d.LastTime != null);
                var orderTime = System.Array.ConvertAll(orderLasts.ToArray(), d => d.LastTime).Min();

                
                // 昨天等待发货及等待收货
                var r1 = ClassifiedOrders(orderDic[OrderTypes.Yesterday])[OrderTypes.Ready];
                var w1 = ClassifiedOrders(orderDic[OrderTypes.Yesterday])[OrderTypes.Wait];
                var kvs1 = CreateDetail(r1, w1);

                // 前天等待发货及等待收货
                var r2 = ClassifiedOrders(orderDic[OrderTypes.BeforeOneDay])[OrderTypes.Ready];
                var w2 = ClassifiedOrders(orderDic[OrderTypes.BeforeOneDay])[OrderTypes.Wait];
                var kvs2 = CreateDetail(r2, w2);


                var kv = CreateOverview(name + daily.Nick, r1, w1, r2, w2);
                var n1 = CreateNumber(r1, w1);
                var n2 = CreateNumber(r2, w2);


                
                old = daily;

                object[] os = new object[] { name, daily.Nick, daily.Operator, daily.InSrockNumber, daily.ReviewNumber, daily.RemovedNumber,
                    $"{daily.IM24:P2}", $"{daily.GoodReviews:P2}", $"{daily.Dispute:P2}", $"{daily.WrongGoods:P2}",
                    disputeTime, finCount30, disCount30, Math.Round(exp30), finCount, disCount, disputeDic[DisputeTypes.Close].Count(), disputeDic[DisputeTypes.Talk].Count(), disputeDic[DisputeTypes.Platform].Count(),
                    orderDic[OrderTypes.Ready].Count(), orderTime, orderDic[OrderTypes.Yesterday].Count(), r1.Count(),w1.Count() ,
                    daily.Lend, daily.Freeze, daily.OnWay, daily.Arrears, Math.Round(loseAmount) ,  Math.Round(Math.Abs(withdraws)),
                    reality, balance
                    };
                if (o.IsListCompany || o.IsListProfit || o.IsUploadProfit)
                {
                    if (!objDic.ContainsKey(name))
                    {
                        objDic[name] = new List<object[]> { };
                    }
                    objDic[name].Add(os);
                }
                 if (o.IsListOpera)
                {
                    if (!objDic.ContainsKey(daily.Operator))
                    {
                        objDic[daily.Operator] = new List<object[]> { };
                    }
                    objDic[daily.Operator].Add(os);
                }


                // 上传订单
                if (o.IsUploadOrder)
                {
                    if (n1.Key > 0 || n2.Key > 0)
                    {
                        count1 += n1.Key;
                        total1 += n1.Value;

                        count2 += n2.Key;
                        total2 += n2.Value;


                        uploadDic.Add(kv);
                        uploadDic.AddRange(kvs1);
                        uploadDic.AddRange(kvs2);

                        uploadDic.Add(KeyValuePair.Create("", ""));

                    }
                }

            }



            if (o.IsListCompany || o.IsListOpera)
            {
                ConsoleTable consoleTable = new ConsoleTable(listHeader);
                foreach (var company in objDic)
                {
                    foreach (var item in company.Value)
                    {
                  
                        _output.AddRow(item);
                    }
                }
                _output.Show(listHeader);
            }


            Dictionary<string, List<object[]>> proDic = new Dictionary<string, List<object[]>>();
            string[] listCompanyHeader = new string[] { "Company", "Lead", "Freeze", "OnWay", "Arre",
                    "Lose", "Get", "Reality", "Balance", "Profit"
                };

            if (o.IsListProfit || o.IsUploadProfit)
            {
                foreach (var company in objDic)
                {
                    double lend = 0.0;
                    double freeze = 0.0;
                    double onWay = 0.0;
                    double arrears = 0.0;
                    double lose = 0.0;
                    double withdraws = 0.0;
                    double reality = 0.0;
                    double balance = 0.0;

                    foreach (var item in company.Value)
                    {
                        lend += (double)item[24];
                        freeze += (double)item[25];
                        onWay += (double)item[26];
                        arrears += (double)item[27];
                        lose += (double)item[28];
                        withdraws += (double)item[29];
                        reality = (double)item[30];
                        balance = (double)item[31];
                    }
                    object[] os1 = new object[] { company.Key, $"{lend:0.00}", $"{freeze:0.00}", $"{onWay:0.00}", $"{arrears:0.00}",
                        $"{lose:0.00}", $"{withdraws:0}", $"{reality:0}",$"{balance:0}", $"{lend + onWay + withdraws + balance - (arrears + lose + reality):0.00}"
                    };

                    proDic[company.Key] = new List<object[]> { os1 };

                    _output.AddRow(os1);
                }

                if (o.IsListProfit)
                {
                    _output.Show(listCompanyHeader);

                }


            }

            if (o.IsUploadProfit)
            {
                string[] listCompanyHeader1 = new string[] { "公司", "放款", "冻结", "在途", "欠款",
                    "损耗", "回款", "实充", "余额", "利润" };
                foreach (var company in proDic)
                {
                    object[] o1 = company.Value.First();
                    var upload = new List<KeyValuePair<string, string>>();

                    upload.Add(KeyValuePair.Create($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}", $""));
                    upload.Add(KeyValuePair.Create("-------------------------------", $""));
                    upload.Add(KeyValuePair.Create($"  公司", $"{o1[0]}"));
                    upload.Add(KeyValuePair.Create($"（加）放款", $"{o1[1]}"));
                    upload.Add(KeyValuePair.Create($"（减）冻结", $"{o1[2]}"));
                    upload.Add(KeyValuePair.Create($"（加）在途", $"{o1[3]}"));
                    upload.Add(KeyValuePair.Create($"（减）欠款", $"{o1[4]}"));
                    upload.Add(KeyValuePair.Create($"（减）损耗", $"{o1[5]}"));
                    upload.Add(KeyValuePair.Create($"（加）回款", $"{o1[6]}"));
                    upload.Add(KeyValuePair.Create($"（减）实充", $"{o1[7]}"));
                    upload.Add(KeyValuePair.Create($"（加）余额", $"{o1[8]}"));
                    upload.Add(KeyValuePair.Create($"（等）利润", $"{o1[9]}"));
                    foreach (var os in objDic[company.Key])
                    {
                        upload.Add(KeyValuePair.Create("-------------------------------", $""));
                        upload.Add(KeyValuePair.Create($"{os[0]}{os[1]}", $"{os[2]}"));
                        upload.Add(KeyValuePair.Create($"上架:{os[3]} 审核:{os[4]} 下架:{os[5]}", ""));
                        upload.Add(KeyValuePair.Create($"IM24:{os[6]} ", $"好评:{os[7]}"));
                        upload.Add(KeyValuePair.Create($"纠纷:{os[8]} ", $"错发:{os[9]}"));
                        upload.Add(KeyValuePair.Create($"纠纷处理:", $"{os[10]}"));
                        upload.Add(KeyValuePair.Create($"发货处理:", $"{os[20]}"));
                        upload.Add(KeyValuePair.Create($"F30:{os[11]} D30:{os[12]} ExpD30%:{os[13]}", $""));
                        upload.Add(KeyValuePair.Create($"完成:{os[14]} 纠纷:{os[15]} 结束:{os[16]}", $""));
                        upload.Add(KeyValuePair.Create($"协商:{os[17]} 介入:{os[18]} 总待:{os[19]}", $""));
                        upload.Add(KeyValuePair.Create($"新单:{os[21]} 待发:{os[22]} 已发:{os[23]}", $""));
                        upload.Add(KeyValuePair.Create($"放款:{os[24]} ", $"冻结:{os[25]}"));
                        upload.Add(KeyValuePair.Create($"在途:{os[26]} ", $"欠款:{os[27]}"));
                        upload.Add(KeyValuePair.Create($"损耗:{os[28]} ", $"回款:{os[29]}"));
                    }
                    string url = "https://qyapi.weixin.qq.com/cgi-bin/webhook/send?key=8af72f8c-1f27-48b0-832e-dcfb8b7f17d2";
                    new WebhookClient().Webhook(url, "text",  upload);
                }
            }


            // 显示
            if (o.IsUploadOrder)
            {
                var kvt = KeyValuePair.Create($"{DateTime.Now.ToString("yyyy-MM-dd")}", $"巴西订单");
                var kvt1 = KeyValuePair.Create($"昨天总计 {count1}单", $"R$ {total1:0.00}");
                var kvt2 = KeyValuePair.Create($"前天总计 {count2}单", $"R$ {total2:0.00}");
                uploadDic.Add(kvt);
                uploadDic.Add(kvt1);
                uploadDic.Add(kvt2);

                string url = "https://qyapi.weixin.qq.com/cgi-bin/webhook/send?key=8af72f8c-1f27-48b0-832e-dcfb8b7f17d2";

                List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();

                int len = 0;
                foreach (var item in uploadDic)
                {
                    keyValuePairs.Add(KeyValuePair.Create(item.Key, item.Value));
                    len += (item.Key.Length + item.Value.Length);
                    if(len >= 2048)
                    {

                        new WebhookClient().Webhook(url, "text", keyValuePairs);
                        keyValuePairs = new List<KeyValuePair<string, string>>();
                        len = 0;
                    }
                }
                new WebhookClient().Webhook(url, "text", keyValuePairs);
            }
            



        }



        public DailyDetail[] GetDailyDetails(string dirPath)
        {
            // 获取每天店铺数据
            List<DailyDetail> dailyDetailList = new List<DailyDetail>();
            string[] files = Directory.GetFiles(dirPath);
            foreach (string filename in files)
            {
                if (!Path.GetFileName(filename).StartsWith("~") && !Path.GetFileName(filename).EndsWith("txt"))
                {
                    DailyDetail daily = _sheetClient.ReadDaily(filename);
                    dailyDetailList.Add(daily);
                }
            }
            dailyDetailList.Sort();
            return dailyDetailList.ToArray();
        }

        public void ListMissingStores(DailyDetail[] dailyDetails)
        {
            List<Shop> shops = _sheetClient.ReadShopInfo(this.ShopInfoFileName).ToList();

            var runShop = shops.Where(s => s.Status.Equals("运营中")).ToArray();
            _output.WriteLine($"运营中：{runShop.Count()} 读取数量：{dailyDetails.Length}");

            string[] arr = System.Array.ConvertAll(runShop.ToArray(), r => r.CN);

            foreach (var shop in runShop)
            {

                bool flag = dailyDetails.Where(d => d.CN.Equals(shop.CN)).Count() > 0;

                if (!flag)
                {
                    _output.WriteLine(shop.CompanyNumber + shop.CN + shop.CompanyName + shop.Nick);

                }
            }
        }

        private async Task<StoreOverview[]> GetStoreOverviews(DailyDetail[] dailyDetails)
        {
            User[] users = null;
            Recharge[] recharges = null;
            List<StoreOverview> overs = new List<StoreOverview>();
            // 获取用户列表，充值列表

            ManageClient manageClient = new ManageClient();
            manageClient.LoginAdmin();
            users = manageClient.ListUsers();
            recharges = await manageClient.ListAllRechargeAsync();

            foreach (var daily in dailyDetails)
            {
                // 公司简称
                string name = daily.Company.Replace("市", "").Replace("县", "").Substring(2, 4);
                var orderDic = ClassifiedOrders(daily.OrderDetails);
                var disputeDic = ClassifiedDispute(daily.DisputeOrders);

                var max = DateTime.Now;
                var min = DateTime.Now.AddMonths(-1);
                int disCount30 = daily.DisputeOrders.Where(o => o.DisputeTime.CompareTo(min) >= 0 && o.DisputeTime.CompareTo(max) <= 0).Count();
                int finCount30 = orderDic[OrderTypes.Finish].Where(o => o.OrderTime.CompareTo(min) >= 0 && o.OrderTime.CompareTo(max) <= 0).Count();
                int disCount = daily.DisputeOrders.Count();
                int finCount = orderDic[OrderTypes.Finish].Count();

                double exp30 = (disCount30 / 0.3) - disCount30 - finCount30;

                // 在途纠纷及拒付订单
                var onwayLose = daily.OnWayOrders.Where(o => o.Reason.Contains("纠纷中") || o.Reason.Contains("拒付中"));
                var loseAmount = onwayLose.Sum(o => o.Amount);

                // 实际充值 不是索赔，不是返点
                double reality = recharges.Where(r => r.CompanyName.Contains(daily.Company) && !r.Mark.Contains("返点") && !r.Mark.Contains("索赔") && !r.Mark.Contains("海外仓")).Sum(o => o.Amount);

                // 云仓余额
                double balance = users.Where(u => u.CompanyName.Contains(daily.Company)).FirstOrDefault().Balance;

                // 实际提现
                double withdraws = daily.Withdraws.Sum(o => o.Amount);

                // 纠纷最小处理天数
                var disputeLasts = daily.DisputeOrders.Where(d => d.LastTime != null);
                var disputeTime = System.Array.ConvertAll(disputeLasts.ToArray(), d => d.LastTime).Min();

                // 待发货最小处理天数
                var orderLasts = daily.OrderDetails.Where(d => d.LastTime != null);
                var orderTime = System.Array.ConvertAll(orderLasts.ToArray(), d => d.LastTime).Min();


                // 昨天等待发货及等待收货
                var r1 = ClassifiedOrders(orderDic[OrderTypes.Yesterday])[OrderTypes.Ready];
                var w1 = ClassifiedOrders(orderDic[OrderTypes.Yesterday])[OrderTypes.Wait];


                StoreOverview over = new StoreOverview();
                // 再售数据
                over.Company = name;
                over.CN = daily.Nick;
                over.Opera = daily.Operator;
                over.UP = daily.InSrockNumber;
                over.Check = daily.ReviewNumber;
                over.Down = daily.RemovedNumber;
                // 服务数据
                over.IM24 = daily.IM24;
                over.Good = daily.GoodReviews;
                over.Dispute = daily.Dispute;
                over.Wrong = daily.WrongGoods;
                // 纠纷数据
                over.DisputeLine = disputeTime;
                over.F30 = finCount30;
                over.D30 = disCount30;
                over.Exp30 = (int)exp30;
                over.Fin = finCount;
                over.Dis = disCount;
                over.Close = disputeDic[DisputeTypes.Close].Count();
                over.Talk = disputeDic[DisputeTypes.Talk].Count();
                over.Palt = disputeDic[DisputeTypes.Platform].Count();
                // 订单数据
                over.All = orderDic[OrderTypes.Ready].Count();
                over.ReadyLine = orderTime;
                over.New = orderDic[OrderTypes.Yesterday].Count();
                over.Ready = r1.Count();
                over.Wait = w1.Count();
                // 资金数据
                Funds funds = new Funds();
                funds.Lend = daily.Lend;
                funds.Freeze = daily.Freeze;
                funds.OnWay = daily.OnWay;
                funds.Arre = daily.Arrears;
                funds.Lose = loseAmount;
                funds.Get = Math.Abs(withdraws);
                funds.Reality = reality;
                funds.Balance = balance;
                over.Funds = funds;

                overs.Add(over);
            }

            return overs.ToArray();
        }

        private Funds GetCompanyFunds(StoreOverview[] storeOverviews)
        {
            double lend = 0.0;
            double freeze = 0.0;
            double onWay = 0.0;
            double arrears = 0.0;
            double lose = 0.0;
            double withdraws = 0.0;
            double reality = 0.0;
            double balance = 0.0;
            foreach (var overview in storeOverviews)
            {
                lend += overview.Funds.Lend;
                freeze += overview.Funds.Freeze;
                onWay += overview.Funds.OnWay;
                arrears += overview.Funds.Arre;
                lose += overview.Funds.Lose;
                withdraws += overview.Funds.Get;
                reality = overview.Funds.Reality;
                balance = overview.Funds.Balance;

            }
            Funds funds = new Funds();
            funds.Lend = lend;
            funds.Freeze = freeze;
            funds.OnWay = onWay;
            funds.Arre = arrears;
            funds.Lose = lose;
            funds.Get = withdraws;
            funds.Reality = reality;
            funds.Balance = balance;


            return funds;
        }

        public async void BuildCompanyProfits(DailyDetail[] dailyDetails, string filename)
        {
            File.Create(filename);
            StoreOverview[] storeOverviews = await GetStoreOverviews(dailyDetails);
            var group = storeOverviews.GroupBy(x => x.Company);
            foreach (var item in group)
            {
                Funds funds = GetCompanyFunds(item.ToArray());
                var upload = new List<KeyValuePair<string, string>>();
                double total = funds.Lend + funds.OnWay + funds.Get + funds.Balance - (funds.Arre + funds.Lose + funds.Reality);
                upload.Add(KeyValuePair.Create($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}", $""));
                upload.Add(KeyValuePair.Create("-------------------------------", $""));
                upload.Add(KeyValuePair.Create($"  公司", $"{item.Key}"));
                upload.Add(KeyValuePair.Create($"（加）放款", $"{funds.Lend:0.00}"));
                upload.Add(KeyValuePair.Create($"（减）冻结", $"{funds.Freeze:0.00}"));
                upload.Add(KeyValuePair.Create($"（加）在途", $"{funds.OnWay:0.00}"));
                upload.Add(KeyValuePair.Create($"（减）欠款", $"{funds.Arre:0.00}"));
                upload.Add(KeyValuePair.Create($"（减）损耗", $"{funds.Lose:0.00}"));
                upload.Add(KeyValuePair.Create($"（加）回款", $"{funds.Get:0.00}"));
                upload.Add(KeyValuePair.Create($"（减）实充", $"{funds.Reality:0.00}"));
                upload.Add(KeyValuePair.Create($"（加）余额", $"{funds.Balance:0.00}"));
                upload.Add(KeyValuePair.Create($"（等）利润", $"{total:0.00}"));

                foreach (var os in item.ToArray())
                {
                    upload.Add(KeyValuePair.Create("-------------------------------", $""));
                    upload.Add(KeyValuePair.Create($"{os.Company}{os.CN}", $"{os.Opera}"));
                    upload.Add(KeyValuePair.Create($"上架:{os.UP} 审核:{os.Check} 下架:{os.Down}", ""));
                    upload.Add(KeyValuePair.Create($"IM24:{os.IM24:P} ", $"好评:{os.Good:P}"));
                    upload.Add(KeyValuePair.Create($"纠纷:{os.Dispute:P} ", $"错发:{os.Wrong:P}"));
                    upload.Add(KeyValuePair.Create($"纠纷处理:", $"{os.DisputeLine}"));
                    upload.Add(KeyValuePair.Create($"发货处理:", $"{os.ReadyLine}"));
                    upload.Add(KeyValuePair.Create($"F30:{os.F30} D30:{os.D30} ExpD30%:{os.Exp30}", $""));
                    upload.Add(KeyValuePair.Create($"完成:{os.Fin} 纠纷:{os.Dis} 结束:{os.Close}", $""));
                    upload.Add(KeyValuePair.Create($"协商:{os.Talk} 介入:{os.Palt} 总待:{os.All}", $""));
                    upload.Add(KeyValuePair.Create($"新单:{os.New} 待发:{os.Ready} 已发:{os.Wait}", $""));
                    upload.Add(KeyValuePair.Create($"放款:{os.Funds.Lend:0.00} ", $"冻结:{os.Funds.Freeze:0.00}"));
                    upload.Add(KeyValuePair.Create($"在途:{os.Funds.OnWay:0.00} ", $"欠款:{os.Funds.Arre:0.00}"));
                    upload.Add(KeyValuePair.Create($"损耗:{os.Funds.Lose:0.00} ", $"回款:{os.Funds.Get:0.00}"));
                }
                upload.Add(KeyValuePair.Create("", ""));
                string str = "";
                upload.ForEach(x => str += (x.Key+" "+x.Value+"\r\n"));
                
                File.AppendAllText(filename, str);
            }
        }
   

        public void BuildOrders(DailyDetail[] dailyDetails, string filename)
        {

            double total1 = 0.0;
            int count1 = 0;
            double total2 = 0.0;
            int count2 = 0;
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            foreach (var detail in dailyDetails)
            {
                
                var orderDic = ClassifiedOrders(detail.OrderDetails);
                // 昨天等待发货及等待收货
                var r1 = ClassifiedOrders(orderDic[OrderTypes.Yesterday])[OrderTypes.Ready];
                var w1 = ClassifiedOrders(orderDic[OrderTypes.Yesterday])[OrderTypes.Wait];
                var kvs1 = CreateDetail(r1, w1);

                // 前天等待发货及等待收货
                var r2 = ClassifiedOrders(orderDic[OrderTypes.BeforeOneDay])[OrderTypes.Ready];
                var w2 = ClassifiedOrders(orderDic[OrderTypes.BeforeOneDay])[OrderTypes.Wait];
                var kvs2 = CreateDetail(r2, w2);

                string name = detail.Company.Replace("市", "").Replace("县", "").Substring(2, 4);
                var kvs = CreateOverview(name + detail.Nick, r1, w1, r2, w2);
                var n1 = CreateNumber(r1, w1);
                var n2 = CreateNumber(r2, w2);

                if (n1.Key > 0 || n2.Key > 0)
                {
                    count1 += n1.Key;
                    total1 += n1.Value;
                    count2 += n2.Key;
                    total2 += n2.Value;
                    list.Add(kvs);
                    list.AddRange(kvs1);
                    list.AddRange(kvs2);
                    list.Add(KeyValuePair.Create("", ""));

                }
            }

            var kvt = KeyValuePair.Create($"{DateTime.Now.ToString("yyyy-MM-dd")}", $"巴西订单");
            var kvt1 = KeyValuePair.Create($"昨天总计 {count1}单", $"R$ {total1:0.00}");
            var kvt2 = KeyValuePair.Create($"前天总计 {count2}单", $"R$ {total2:0.00}");
            list.Add(kvt);
            list.Add(kvt1);
            list.Add(kvt2);

            string str = "";
            list.ForEach(x => str += (x.Key + " " + x.Value + "\r\n"));

            File.Delete(filename);
            File.AppendAllText(filename, str);
        }

        #endregion

        #region mamage 

        public List<KeyValuePair<string, string>> ReadNeedDeduction(string raw)
        {
            string[] strings = raw.Split("\r\n");
            var list = new List<KeyValuePair<string, string>>();
            foreach (string s in strings)
            {
                string[] v = s.Split(" ");
                if(v.Length == 2)
                {
                    list.Add(KeyValuePair.Create(v[0], v[1]));
                }
            }
            return list;
        }

        public void Deduction(IEnumerable<KeyValuePair<string, string>> list)
        {
            ManageClient client = new ManageClient();
            string msg = string.Empty;
            client.LoginAdmin();
            this.IsRunning = true;
            foreach (var item in list)
            {
                string clientId = item.Key;
                string orderId = item.Value;

                string ret = null;

                try
                {
                    msg += _output.Write("登录用户 ", false);
                    string v = client.LoginUser(clientId);
                    msg += _output.Write($"{clientId} ", false);
                    Models.Manage.Order[] orders = client.ListOrder(orderId);
                    msg += _output.Write($"{orders.Length} ", false);
                    if (orders.Length == 1)
                    {
                        Models.Manage.Order order = orders.First();
                        msg += _output.Write($"{order.OrderId} ", false);
                        double cost = order.Cost;
                        msg += _output.Write($"扣除金额：{cost} ", false);
                        ret = client.Deduction(order);
                        if ("Success.".Equals(ret))
                        {
                            DebitRecord[] debitRecords = client.ListDebitRecord();
                            DebitRecord debitRecord = debitRecords.First();
                            msg += _output.Write($"{debitRecord.RecordId} {debitRecord.TradeId}\t{debitRecord.Cost}RMB ", false);
                            if ((int)debitRecord.Cost == (int)cost)
                            {
                                ret = client.DeductionYes(debitRecord.RecordId);
                                msg += _output.Write($"{ret} ", false);
                                if ("审核成功".Equals(ret))
                                {
                                    ret = client.DeductionPassed(debitRecord.RecordId);
                                    msg += _output.Write($"{ret} ", false);
                                    if ("success".Equals(ret) && order.Status == 1)
                                    {
                                        ret = client.Shipments(order);
                                        msg += _output.Write($"{ret} ", false);
                                    }

                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    _output.WriteLine(ex.Message);
                }
                finally
                {
                    Save(orderId, msg);
                    _output.Write($"结束", false);
                    _output.WriteLine();
                }
            }
            this.IsRunning = false;
        }

        private void Save(string orderId, string msg)
        {
            string filename = Path.Combine(NewestDeductionDirectory, $"{orderId}.txt");
            File.WriteAllText(filename, msg);
        }

        public void ManageRun(ManageOptions o)
        {

            ManageClient client = new ManageClient();
            string msg = string.Empty;
            client.LoginAdmin();
            if (o.IsList)
            {
                User[] user = client.ListUsers();
                User[] richUser = user.Where(u => !string.IsNullOrWhiteSpace(u.CompanyName) && Regex.IsMatch($"{u.CompanyName[0]}", @"[\u4e00-\u9fa5]")).ToArray();
                for (int i = 0; i < richUser.Length; i++)
                {
                    Console.WriteLine($"{i:000} {richUser[i].ClientId} {richUser[i].CompanyName}");
                }
                return;
            }

            if(File.Exists(o.FileName))
            {
                string raw = File.ReadAllText(o.FileName);
                string[] strings = raw.Split("\r\n");
                List<string>  ClientId = new List<string>();
                List<string> DeductionId = new List<string>();
                foreach (string s in strings)
                {
                    string[] v = s.Split(" ");
                    ClientId.Add(v[0]);
                    DeductionId.Add(v[1]);
                }

                o.ClientId = ClientId;
                o.DeductionId = DeductionId;
            }

            if (o.ClientId.Count() > 0 && o.DeductionId.Count() > 0)
            {
                for (int i = 0; i < o.DeductionId.Count(); i++)
                {
                    string clientId = o.ClientId.ElementAt(i);
                    string orderId = o.DeductionId.ElementAt(i);
                    string ret = null;

                    try
                    {
                        _output.Write("登录用户 ", false);
                        string v = client.LoginUser(clientId);
                        _output.Write($"{clientId} ", false);
                        Models.Manage.Order[] orders = client.ListOrder(orderId);
                        _output.Write($"{orders.Length} ", false);
                        if (orders.Length == 1)
                        {
                            Models.Manage.Order order = orders.First();
                            _output.Write($"{order.OrderId} ", false);
                            double cost = order.Cost;
                            _output.Write($"扣除金额：{cost}", false);
                            ret = client.Deduction(order);
                            if ("Success.".Equals(ret))
                            {
                                DebitRecord[] debitRecords = client.ListDebitRecord();
                                DebitRecord debitRecord = debitRecords.First();
                                _output.Write($"{debitRecord.RecordId} {debitRecord.TradeId} {debitRecord.Cost}RMB ", false);
                                if ((int)debitRecord.Cost == (int)cost)
                                {
                                    ret = client.DeductionYes(debitRecord.RecordId);
                                    _output.Write($"{ret} ", false);
                                    if ("审核成功".Equals(ret))
                                    {
                                        ret = client.DeductionPassed(debitRecord.RecordId);
                                        _output.Write($"{ret} ", false);
                                        if ("success".Equals(ret) && order.Status == 1)
                                        {
                                            ret = client.Shipments(order);
                                            _output.Write($"{ret} ", false);
                                        }

                                    }
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _output.WriteLine(ex.Message);
                    }
                    finally
                    {
                        Save(orderId, msg);
                        _output.Write($"结束", false);
                        _output.WriteLine();
                    }
                }
            }
        }


        #endregion

        #region purchase
        public void ShowPurchase()
        {
            var surePurchases = _sheetClient.TotalPurchases.Where(p => p.SubmissionDate != DateTime.MinValue && p.Country.Equals("BR")).ToArray();


            int i = 1;
 
            var enumerable = surePurchases.GroupBy(x => x.SubmissionDate).OrderBy(y => y.Key).ToArray();
            foreach (var item in enumerable)
            {
                var processing = item.Where(e => "已下单".Equals(e.Status));
                var solved = item.Where(e => "已发货".Equals(e.Status));
                var cancel = item.Where(e => "砍单".Equals(e.Status));
                var cut = item.Where(e => "截单".Equals(e.Status));
                var pending = item.Where(e => string.IsNullOrWhiteSpace(e.Status) ||
                    !e.Status.Equals("已下单") && !e.Status.Equals("已发货") && !e.Status.Equals("砍单") && !e.Status.Equals("截单"));
                _output.AddRow( (DateTime.Now - item.Key).Days, item.Key.ToString("yyyy-MM-dd"), item.Count(),
                    pending.Count(), processing.Count(), solved.Count(), cancel.Count(), cut.Count());

            }
            _output.Show("I",  "date", "Order", "Pending", "Processing", "Solved", "Cancel", "Cut");


        }
        #endregion

        public void DateRange(int year, int moon, out DateTime start, out DateTime end)
        {
            DateTime t1;
            DateTime.TryParseExact($"{year}-{moon:D2}-01 00:00:00", "yyyy-MM-dd HH:mm:ss", new CultureInfo("zh-cn"), DateTimeStyles.None, out t1);

            DateTime t2;

            int y, m;
            if (moon == 12)
            {
                y = year + 1;
                m = (moon / 12) + (moon % 12);
            }
            else
            {
                y = year;
                m = (moon / 12) + (moon % 12) + 1;
            }

            DateTime.TryParseExact($"{y}-{m:D2}-01 00:00:00", "yyyy-MM-dd HH:mm:ss", new CultureInfo("zh-cn"), DateTimeStyles.None, out t2);
            start = t1;
            end = t2;
        }

        public KeyValuePair<DateTime, DateTime> DateRange(int year, int moon)
        {
            DateTime t1, t2;
            DateRange(year, moon, out t1, out t2);

            return KeyValuePair.Create(t1, t2);
        }


        string _oldRoot = "";
        public bool SetRootDirectorie(string root="")
        {
            this.IsRunning = true;
            if(!string.IsNullOrWhiteSpace(root) && root.Equals(_oldRoot))
            {
                IsRunning = false;
                return true;
            }
            if (!Directory.Exists(root))
            {
                root = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            }
            
            // 店铺信息表格
            string shopInfoFileName = Path.Combine(root, "店铺信息.xlsx");
            if (File.Exists(shopInfoFileName))
            {
                this.Root = root;
                ShopInfoFileName = shopInfoFileName;
            }
            else
            {
                return false;
            }
            _output.WriteLine($"店铺信息 {ShopInfoFileName}");

            // 总表数据
            string[] dirs = Directory.GetDirectories(Path.Combine(root, "总表数据"));
            Array.Sort(dirs);
            NewestTotalDirectory = dirs.LastOrDefault();
            OrderRecordFileName = Path.Combine(NewestTotalDirectory, "订单总表.xlsx");
            UsPruchasRecordFileName = Path.Combine(NewestTotalDirectory, "美国采购单.xlsx");
            BrPruchasRecordFileName = Path.Combine(NewestTotalDirectory, "巴西采购单.xlsx");
            _output.WriteLine($"总表数据 {NewestTotalDirectory}");
            

            // 订单数据
            string[] dirs1 = Directory.GetDirectories(Path.Combine(root, "订单数据"));
            Array.Sort(dirs1);
            NewestOrderDirectory = dirs1.LastOrDefault();
            _output.WriteLine($"订单数据 {NewestOrderDirectory}");

            // 利润统计
            string[] dirs2 =  Directory.GetDirectories(Path.Combine(root, "利润统计"));
            Array.Sort(dirs2);
            NewestProfitDirectory = dirs2.LastOrDefault();
            _output.WriteLine($"利润统计 {NewestProfitDirectory}");

            // 每日数据
            string[] dirs3 = Directory.GetDirectories(Path.Combine(root, "每日数据"));
            Array.Sort(dirs3);
            NewestDailyDirectory = dirs3.LastOrDefault();
            _output.WriteLine($"每日数据 {NewestDailyDirectory}");

            // 索赔记录
            string[] dirs4 = Directory.GetDirectories(Path.Combine(root, "索赔记录"));
            Array.Sort(dirs4);
            NewestReparationDirectory = dirs4.LastOrDefault();
            _output.WriteLine($"索赔记录 {NewestReparationDirectory}");


            // 索赔记录
            string[] dirs6 = Directory.GetDirectories(Path.Combine(root, "扣款记录"));
            Array.Sort(dirs6);
            NewestDeductionDirectory = dirs6.LastOrDefault();
            _output.WriteLine($"索赔记录 {NewestDeductionDirectory}");

            // bin
            string[] dirs5 = Directory.GetFiles(Path.Combine(root, "bin"));
            Array.Sort(dirs5);
            NewestBinFileName = dirs5.LastOrDefault();
            _output.WriteLine($"bin {NewestBinFileName}");

            // resources
            NewestResourcesDirectory = Path.Combine(root, "resources");
            _output.WriteLine($"resources {NewestResourcesDirectory}");

            _oldRoot = root;
            this.IsRunning = false;
            return true;
        }

        public void CollectShopRecords(params string[] prefixs)
        {

            // 没有指定文件夹。获取所有文件夹
            if (prefixs == null || prefixs.Count() == 0)
            {
                prefixs = Directory.GetDirectories(NewestOrderDirectory).Select(o => Path.GetFileName(o)).ToArray();
            }

            _sheetClient.CollectTotalInfo(ShopInfoFileName, OrderRecordFileName, UsPruchasRecordFileName, BrPruchasRecordFileName);
            _sheetClient.CollectOrderRecords(NewestOrderDirectory, prefixs);
        }

        public ShopRecord[] GetShopRecords(params string[] prefixs)
        {
            if (prefixs.Length != 0)
            {
                List<ShopRecord> shops = new List<ShopRecord>();
                // 返回指定的商店订单记录
                foreach (var dir in prefixs)
                {
                    List<Shop> shops1 = _sheetClient.SelectShop(dir);
                    IEnumerable<ShopRecord> enumerable = _sheetClient.ShopRecords.Where(r => shops1.Where(s1 => s1.CN.Equals(r.Shop.CN)).Count() > 0);
                    shops.AddRange(enumerable);
                }

                return shops.ToArray();
            }
            else
            {
                return _sheetClient.ShopRecords.ToArray();
            }

        }


    }
}
