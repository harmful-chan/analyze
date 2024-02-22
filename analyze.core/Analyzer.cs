using analyze.core.Clients;
using analyze.core.Models.Daily;
using analyze.core.Models.Manage;
using analyze.core.Models.Sheet;
using analyze.core.Options;
using analyze.Models.Manage;
using ConsoleTables;
using NPOI.HPSF;
using NPOI.HSSF.Record;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static analyze.core.Clients.SheetClient;
using static ICSharpCode.SharpZipLib.Zip.ExtendedUnixData;
using static NPOI.HSSF.Util.HSSFColor;

namespace analyze.core
{
    public class Analyzer
    {
        public Analyzer()
        {
            if(SheetClient.Log == null)
            {
                SheetClient.Log = (str) => {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write($"{str}");
                };
            }
        }

        #region 订单数据处理 
        public void OrderRun(OrderOptions o)
        {
            if (o.action.Equals("refund"))
            {
                RefundRun(o);
            }else if (o.action.Equals("lend"))
            {
                LendRun(o);
            }
        }

        #endregion

        #region 放款数据 lend
        double[] randoms = new double[] { 0.8753,   0.8352,   0.6807,   0.6998,   0.7895,   0.7809,   0.5627,   0.5281,   0.4465,   0.8804,   0.7130,   0.6523,   0.5964,   0.6810,   0.6780,   0.8286,  0.6819,   0.6674,   0.4785,   0.6083 };
        static int randomCount = 0;
        public double GetRandom()
        {
            return randoms[randomCount++%randoms.Length];
        }
        public void ResetRandom()
        {
            randomCount = 0;
        }

        public void LendRun(OrderOptions o)
        {
            SheetClient client = new SheetClient();
            Collect(client, o.RowDir, o.DataDir, o.DirPrefix);
            Console.WriteLine();
            foreach (var shop in client.ShopRecords) // 每一个店铺
            {
                
                ResetRandom();
                // 年月获取 开始结束日期
                if (o.Year > 0 && o.Moon > 0)
                {
                    DateTime t1, t2;
                    DateRange(o.Year, o.Moon, out t1, out t2);
                    o.StartDate = t1;
                    o.EndDate = t2;

                }

                ShopLend[] sureLends = shop.ShopLendList.Where(f => f.SettlementTime <= o.EndDate && f.SettlementTime >= o.StartDate).ToArray();
                foreach (var r in sureLends)  // 每一个订单
                {
                    bool flag1 = true;
                    bool flag2 = true;
                    bool flag3 = true;
                    bool flag4 = true;

                    ShopOrder[] shopOrders = shop.ShopOrderList.Where(t => t.OrderId.Equals(r.OrderId)).ToArray();
                    TotalOrder[] torders = client.TotalOrders.Where(t => t.OrderId != null && t.OrderId.Contains(r.OrderId)).ToArray();
                    TotalPurchase[] porder = client.TotalPurchases.Where(t => t.OrderId != null && t.OrderId.Equals(r.OrderId)).ToArray();

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
                    r.Rate = Math.Round((r.Profit)/r.Cost, 2);

                }

                // 列出表格
                if (o.IsList)
                {
                    
                    int index = 1;  // 序号
                    double lend=0.0, cost = 0.0, profit = 0.0, rate = 0.0;
                    ConsoleTable table = new ConsoleTable("I", "Order", "Quantity", "Turnover", "Lend", "Cost", "Profit", "Rate", "SettlementTime");
                    //Array.ForEach(sureLends, a => table.AddRow(index++, a.OrderId, a.Quantity, a.Turnover, a.Lend,  a.Cost, a.Profit, a.Rate, a.SettlementTime.ToString("yyyy-MM-dd HH:mmss")));
                    System.Array.ForEach(sureLends, a => 
                    {
                        lend += a.Lend;
                        cost += a.Cost;
                        profit += a.Profit;
                        rate += a.Rate;
                    });
                    int len = sureLends.Length;
                    Console.WriteLine($"{shop.Shop.CompanyNumber}{shop.Shop.CN}{shop.Shop.CompanyName}{shop.Shop.Nick} " + $" Lend:{Math.Round(lend, 2)} Cost:{Math.Round(cost, 2)} Profit:{Math.Round(profit, 2) }  Rate:{Math.Round(profit / cost, 2)}");
                    //table.Write(Format.MarkDown);
                    
                }

                // 生成目录
                if (Directory.Exists(o.OutputFile)) // 输入的是一个目录
                {
                    if (sureLends.Length <= 0)
                    {
                        continue;
                    }
                    System.Array.Sort(sureLends, (x, y) => { return x.SettlementTime.CompareTo(y.SettlementTime); });

                    string filename = Path.Combine(o.OutputFile, $"{o.Year}{o.Moon.ToString("D2")}{shop.Shop.CN}{shop.Shop.CompanyName}{shop.Shop.Nick}.xlsx");
                    client.SaveShopLend(filename, sureLends, true);

                }

            }


        }


        #endregion

        #region 退款数据 refund
        public void Collect(SheetClient clent, string rawdir, string datadir, IEnumerable<string> prefixs)
        {
            if (prefixs != null && prefixs.Count() > 0)
            {
                // 获取文件夹下的子文件夹列表
                foreach (var p in prefixs)
                {
                    clent.Collect(rawdir, datadir, p);
                }
            }
            else
            {
                clent.Collect(rawdir, datadir);
            }
        }
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
        public void RefundRun(OrderOptions o)
        {
            SheetClient client = new SheetClient();
            Collect(client, o.RowDir, o.DataDir, o.DirPrefix);

            foreach (var shop in client.ShopRecords)
            {
                int index = 1;  // 序号

                ConsoleTable table = new ConsoleTable("I", "Order", "Turnover", "Refund", "Cost", "TradeId", "Deduction", "Status", "Country", "RefundTime", "OrderTime", "PaymentTime", "ShippingTime", "ReceiptTime");

                // 年月获取 开始结束日期
                if (o.Year > 0 && o.Moon > 0)
                {
                    DateTime t1, t2;
                    DateRange(o.Year, o.Moon, out t1, out t2);
                    o.StartDate = t1;
                    o.EndDate = t2;

                }

                ShopRefund[] sureRefunds = shop.ShopRefundList.Where(f => f.RefundTime <= o.EndDate && f.RefundTime >= o.StartDate).ToArray();
                foreach (var r in sureRefunds)
                {
                    bool flag1 = true;
                    bool flag2 = true;
                    bool flag3 = true;
                    bool flag4 = true;

                    ShopOrder[] shopOrders = shop.ShopOrderList.Where(t => t.OrderId.Equals(r.OrderId)).ToArray();
                    TotalOrder[] torders = client.TotalOrders.Where(t => t.OrderId != null && t.OrderId.Equals(r.OrderId)).ToArray();
                    TotalPurchase[] porder = client.TotalPurchases.Where(t => t.OrderId != null && t.OrderId.Equals(r.OrderId)).ToArray();
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

                        table.AddRow(index++, r.OrderId, r.Turnover, r.Refund,
                            torders.FirstOrDefault()?.Cost, torders.FirstOrDefault().TradeId, torders.FirstOrDefault()?.DeductionAmount, str,
                            shopOrders.FirstOrDefault()?.Country, a, ot, pt, st, rt);
                    }
                    else
                    {
                        table.AddRow(index++, r.OrderId, r.Turnover, r.Refund,
                            "", "", "", str,
                            shopOrders.FirstOrDefault()?.Country, a, ot, pt, st, rt);
                    }
                    r.Trade = str;
                    // 退款理由：纠纷退款， 未发货退款，纠纷退款。
                    if (torders.Length > 0)
                    {
                        TotalOrder to = torders.FirstOrDefault();
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

                // 列出表格
                if (o.IsList)
                {
                    Console.WriteLine();
                    table.Write(Format.MarkDown);
                }

                // 生成目录
                if (Directory.Exists(o.OutputFile)) // 输入的是一个目录
                {
                    if (sureRefunds.Length <= 0)
                    {
                        continue;
                    }
                    System.Array.Sort(sureRefunds, (x, y) => { return x.RefundTime.CompareTo(y.RefundTime); });

                    string filename = Path.Combine(o.OutputFile, $"{shop.Shop.CompanyNumber}{shop.Shop.CN}{shop.Shop.CompanyName}{shop.Shop.Nick}.xlsx");
                    client.SaveShopRefund(filename, sureRefunds, true);

                }

            }


        }
        #endregion
         
        #region 采集的订单数据 daily
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
            var ready = orderDetails.Where(o => o.Status.Contains("等待您发货"));
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
            List<Daily> dailys = new List<Daily>();
            SheetClient client = new SheetClient();
            ManageClient manageClient = new ManageClient();
            IList<TotalPurchase> brPurchases = null;
            // 获取目录文件
            if(Directory.Exists(o.FileDir))
            {
                string[] files = Directory.GetFiles(o.FileDir);
                o.DailyFiles = files;
            }


            User[] users = null;
            Recharge[] recharges = null;
            Task<bool> task = Task.Run(() =>
            {
                manageClient.LoginAdmin();
                users = manageClient.ListUsers();
                recharges = manageClient.ListAllRecharge();
                return true;
            });

            // 循环读取文件
            if (o.DailyFiles.Count() > 0)
            {
                foreach (string filename in o.DailyFiles)
                {
                    if(!Path.GetFileName(filename).StartsWith("~"))
                    {
                        Daily daily = client.ReadDaily(filename);
                        dailys.Add(daily);
                    }
                }
                dailys.Sort();

                if (File.Exists(o.ShopInfoFileName))
                {
                    shops = client.ReadShopInfo(o.ShopInfoFileName).ToList();

                    var runShop = shops.Where(s => s.Status.Equals("运营中")).ToArray();
                    Console.WriteLine();
                    Console.WriteLine($"运营中：{runShop.Count()} 读取数量：{o.DailyFiles.Count()}");

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
                            Console.WriteLine(shop.CompanyNumber + shop.CN + shop.CompanyName + shop.Nick);

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
                brPurchases = client.TotalPurchase(1, o.BrPurchaseFilenmae);
            }

            task.Wait();

            Console.WriteLine();

            List<KeyValuePair<string, string>> uploadDic = new List<KeyValuePair<string, string>>();
            double total1 = 0.0;
            int count1 = 0;
            double total2 = 0.0;
            int count2 = 0;
            Daily old = null;

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
                        consoleTable.AddRow(item);
                    }
                }
                consoleTable.Write(Format.Minimal);
            }


            Dictionary<string, List<object[]>> proDic = new Dictionary<string, List<object[]>>();
            string[] listCompanyHeader = new string[] { "Company", "Lead", "Freeze", "OnWay", "Arre",
                    "Lose", "Get", "Reality", "Balance", "Profit"
                };

            if (o.IsListProfit || o.IsUploadProfit)
            {

                ConsoleTable consoleTable1 = new ConsoleTable(listCompanyHeader);
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
                    consoleTable1.AddRow(os1);
                }

                if (o.IsListProfit)
                {
                    consoleTable1.Write(Format.Minimal);

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
        #endregion

        #region 扣款 mamage 

        private void Save(string orderId, string msg)
        {
            if (!Directory.Exists("扣款记录"))
            {
                Directory.CreateDirectory("扣款记录");
            }
            string path = Path.Combine("扣款记录", $"{DateTime.Now.ToString("yyyy年MM月dd日")}");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = Path.Combine(path, $"{orderId}.txt");
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
                        msg += $"登录用户 ";
                        SheetClient.Log(msg);
                        string v = client.LoginUser(clientId);
                        msg += $"{clientId} ";
                        SheetClient.Log(msg);
                        Order[] orders = client.ListOrder(orderId);
                        msg += $"{orders.Length} ";
                        SheetClient.Log(msg);
                        if (orders.Length == 1)
                        {
                            Order order = orders.First();
                            msg += $"{order.OrderId} ";
                            SheetClient.Log(msg);
                            double cost = order.Cost;


                            msg += $"扣除金额：{cost} ";
                            SheetClient.Log(msg);
                            ret = client.Deduction(order);
                            if ("Success.".Equals(ret))
                            {
                                DebitRecord[] debitRecords = client.ListDebitRecord();
                                DebitRecord debitRecord = debitRecords.First();
                                msg += $"{debitRecord.RecordId} {debitRecord.TradeId} {debitRecord.Cost}RMB ";
                                SheetClient.Log(msg);
                                if ((int)debitRecord.Cost == (int)cost)
                                {
                                    ret = client.DeductionYes(debitRecord.RecordId);
                                    msg += $"{ret} ";
                                    SheetClient.Log(msg);
                                    if ("审核成功".Equals(ret))
                                    {
                                        ret = client.DeductionPassed(debitRecord.RecordId);
                                        msg += $"{ret} ";
                                        SheetClient.Log(msg);
                                        if ("success".Equals(ret) && order.Status == 1)
                                        {
                                            ret = client.Shipments(order);
                                            msg += $"{ret} ";
                                            SheetClient.Log(msg);
                                        }

                                    }
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SheetClient.Log(ex.Message);
                    }
                    finally
                    {
                        Save(orderId, msg);
                        msg += $"结束";
                        SheetClient.Log(msg);
                        SheetClient.Log("\r\n");
                        msg = "";
                    }
                }
                foreach (var item in o.DeductionId)
                {


                }
                

            }
        }
        #endregion

    }
}
