using analyze.core.Clients;
using analyze.core.Models.Daily;
using analyze.core.Models.Sheet;
using analyze.core.Options;
using analyze.core.Options;
using ConsoleTables;
using NPOI.HSSF.Record;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using static NPOI.HSSF.Util.HSSFColor;

namespace analyze.core
{
    internal class Analyzer
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
        #region 退款数据RefundRun
        public void Collect(string rawdir, string datadir, IEnumerable<string> prefixs)
        {
            if (prefixs != null && prefixs.Count() > 0)
            {
                // 获取文件夹下的子文件夹列表
                foreach (var p in prefixs)
                {
                    new SheetClient().Collect(rawdir, datadir, p);
                }
            }
            else
            {
                new SheetClient().Collect(rawdir, datadir);
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
        public void RefundRun(RefundOptions o)
        {
            SheetClient client = new SheetClient();
            Collect(o.RowDir, o.DataDir, o.DirPrefix);

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
                    Array.Sort(sureRefunds, (x, y) => { return x.RefundTime.CompareTo(y.RefundTime); });

                    string filename = Path.Combine(o.OutputFile, $"{shop.Shop.CompanyNumber}{shop.Shop.CN}{shop.Shop.CompanyName}{shop.Shop.Nick}.xlsx");
                    client.SaveShopRefund(filename, sureRefunds, true);

                }

            }


        }
        #endregion

        #region 采集的订单数据
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
        public Dictionary<OrderTypes, IEnumerable<OrderDetail>> ClassifiedOrders(IEnumerable<OrderDetail> orderDetails)
        {
            var yesterday = orderDetails.Where(o => o.OrderTime.Year == DateTime.Now.Year && o.OrderTime.Month == DateTime.Now.Month && o.OrderTime.Day == DateTime.Now.Day - 1);
            var before = orderDetails.Where(o => o.OrderTime.Year == DateTime.Now.Year && o.OrderTime.Month == DateTime.Now.Month && o.OrderTime.Day == DateTime.Now.Day - 2);
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
            List<Daily> dailys = new List<Daily>();
            SheetClient client = new SheetClient();

            // 获取目录文件
            if (Directory.Exists(o.FileDir))
            {
                string[] files = Directory.GetFiles(o.FileDir);
                o.DailyFiles = files;
            }

            // 循环读取文件
            if(o.DailyFiles.Count() > 0)
            {
                foreach (string filename in o.DailyFiles)
                {
                    Daily daily = client.ReadDaily(filename);
                    dailys.Add(daily);
                }
            }


            Console.WriteLine();
            string[] ss = new string[] { "Company", "Shop", "Opera", "UP", "Check", "Down", "IM24", "Good", "Dispute", "WrongGoods", "Lead", "Freeze", "OnWay", "Arrears", "AllReady", "New", "Ready", "Notpay" };
            ConsoleTable consoleTable = new ConsoleTable(ss);
            List<KeyValuePair<string, string>> dic = new List<KeyValuePair<string, string>>();
            double total1 = 0.0;
            int count1 = 0;
            double total2 = 0.0;
            int count2 = 0;

            foreach (var daily in dailys)
            {
                // 公司简称
                string name = daily.Company.Replace("市", "").Replace("县", "").Substring(2, 4) + daily.Nick;


                Dictionary<OrderTypes, IEnumerable<OrderDetail>> orderDic = ClassifiedOrders(daily.OrderDetails);

                // 昨天等待发货及等待收货
                var r1 = ClassifiedOrders(orderDic[OrderTypes.Yesterday])[OrderTypes.Ready];
                var w1 = ClassifiedOrders(orderDic[OrderTypes.Yesterday])[OrderTypes.Wait];
                var kvs1 = CreateDetail(r1, w1);

                object[] os = new object[] { name, daily.Nick, daily.Operator, daily.InSrockNumber, daily.ReviewNumber, daily.RemovedNumber, 
                    $"{daily.IM24:P2}", $"{daily.GoodReviews:P2}", $"{daily.Dispute:P2}", $"{daily.WrongGoods:P2}", 
                    daily.Lend, daily.Freeze, daily.OnWay, daily.Arrears,
                    orderDic[OrderTypes.Ready].Count(), orderDic[OrderTypes.Yesterday].Count(), r1.Count(),w1.Count() };
                consoleTable.AddRow(os);



                // 前天等待发货及等待收货
                var r2 = ClassifiedOrders(orderDic[OrderTypes.BeforeOneDay])[OrderTypes.Ready];
                var w2 = ClassifiedOrders(orderDic[OrderTypes.BeforeOneDay])[OrderTypes.Wait];
                var kvs2 = CreateDetail(r2, w2);


                var kv = CreateOverview(name, r1, w1, r2, w2);
                var n1 = CreateNumber(r1, w1);
                var n2 = CreateNumber(r2, w2);

                if ( n1.Key >0 || n2.Key > 0)
                {
                    count1 += n1.Key;
                    total1 += n1.Value;

                    count2 += n2.Key;
                    total2 += n2.Value;


                    dic.Add(kv);
                    dic.AddRange(kvs1);
                    dic.AddRange(kvs2);

                    dic.Add(KeyValuePair.Create("", ""));

                }
            }

            var kvt = KeyValuePair.Create($"{DateTime.Now.ToString("yyyy-MM-dd")}", $"巴西订单");
            var kvt1 = KeyValuePair.Create($"昨天总计 {count1}单", $"R$ {total1:0.00}");
            var kvt2 = KeyValuePair.Create($"前天总计 {count2}单", $"R$ {total2:0.00}");
            dic.Add(kvt);
            dic.Add(kvt1);
            dic.Add(kvt2);

            if (o.IsList)
            {
                consoleTable.Write(Format.Minimal);
            }

            // 显示
            if (o.IsUpload)
            {

                string url = "https://qyapi.weixin.qq.com/cgi-bin/webhook/send?key=8af72f8c-1f27-48b0-832e-dcfb8b7f17d2";
                new WebhookClient().Webhook(url, dic);
            }

            
            
        }
        #endregion

    }
}
