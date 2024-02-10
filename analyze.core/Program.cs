using analyze.Models;
using analyze.Models.Manage;
using analyze.Options;
using CommandLine;
using ConsoleTables;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace analyze
{
    public class Program
    {

        public static void Main(string[] args)
        {
            try 
            {
                SheetHelper.Log = (str) => {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write($"{str}");
                };
                if (args[0].Equals("collect"))
                {

                }
                else if (args[0].Equals("manage"))
                {
                    Parser.Default.ParseArguments<ManageOptions>(args).WithParsed(ManageRun);
                }
                else if (args[0].Equals("refund")) 
                {
                    Parser.Default.ParseArguments<RefundOptions>(args).WithParsed(RefundRun);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        #region 退款数据

        private static void Collect(string rawdir, string  datadir, IEnumerable<string> prefixs)
        {
            if (prefixs != null && prefixs.Count() > 0)
            {
                // 获取文件夹下的子文件夹列表
                foreach (var p in prefixs)
                {
                    SheetHelper.Collect(rawdir, datadir, p);
                }
            }
            else
            {
                SheetHelper.Collect(rawdir, datadir);
            }
        }
        private static void DateRange(int year, int moon, out DateTime start, out DateTime end)
        {
            DateTime t1;
            DateTime.TryParseExact($"{year}-{moon:D2}-01 00:00:00", "yyyy-MM-dd HH:mm:ss", new CultureInfo("zh-cn"), DateTimeStyles.None, out t1);

            DateTime t2;

            int y, m;
            if(moon == 12)
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
        private static void RefundRun(RefundOptions o)
        {
            Collect(o.RowDir, o.DataDir, o.DirPrefix);

            foreach (var shop in SheetHelper.ShopRecords)
            {
                int index = 1;  // 序号

                ConsoleTable table = new ConsoleTable("I", "Order", "Turnover", "Refund", "Cost", "TradeId", "Deduction", "Status", "Country", "RefundTime", "OrderTime", "PaymentTime", "ShippingTime", "ReceiptTime");


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
                    TotalOrder[] torders = SheetHelper.TotalOrders.Where(t => t.OrderId!=null && t.OrderId.Equals(r.OrderId)).ToArray();
                    TotalPurchase[] porder = SheetHelper.TotalPurchases.Where(t => t.OrderId != null && t.OrderId.Equals(r.OrderId)).ToArray();
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
                        flag4 = !(porder.Where(p => p.Status!=null && p.Status.Contains("已发货")).Count() > 0);
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
                if (o.IsList)
                {
                    Console.WriteLine();
                    table.Write(Format.MarkDown);
                }

                if (Directory.Exists(o.OutputFile)) // 输入的是一个目录
                {
                    if (sureRefunds.Length <= 0)
                    {
                        continue;
                    }
                    Array.Sort(sureRefunds, (x, y) => { return x.RefundTime.CompareTo(y.RefundTime); });

                    string filename = Path.Combine(o.OutputFile, $"{shop.Shop.CompanyNumber}{shop.Shop.CN}{shop.Shop.CompanyName}{shop.Shop.Nick}.xlsx");
                    SheetHelper.SaveShopRefund(filename, sureRefunds, true);

                }

            }


        }
        #endregion

        private static void ManageRun(ManageOptions o)
        {

            if (o.IsList)
            {
                ManageClient manageClient = new ManageClient();
                manageClient.LoginAdminAsync();
                User[] user = manageClient.ListUsers();
                User[] richUser = user.Where(u => !string.IsNullOrWhiteSpace(u.CompanyName) && Regex.IsMatch($"{u.CompanyName[0]}", @"[\u4e00-\u9fa5]")).ToArray();
                for (int i = 0; i < richUser.Length; i++)
                {
                    Console.WriteLine($"{i:000} {richUser[i].ClientId} {richUser[i].CompanyName}");
                }
            }

            if (o.ClientId != null)
            {
                ManageClient manageClient = new ManageClient();
                manageClient.LoginAdminAsync();
                foreach (var id in o.ClientId)
                {

                    User[] user = manageClient.ListUsers(id);
                    manageClient.LoginUserAsync(id);
                    manageClient.ListOrder();
                }
            }
        }
        #region 采集数据


        #endregion


        /// <summary>
        /// args[0] 订单 xlsx路径
        /// args[1] 文件夹前缀   
        /// </summary>
        /// <param name="args"></param>
        public static void ListRefunds(params string[] args)
        {
            // 读订单总表
            IList<TotalOrder> totalOrders = SheetHelper.ReadTotalOrder(args[0]);

            // 获取当前目录下的文件夹
            string[] ds = Directory.GetDirectories(Path.GetDirectoryName(args[0]));
            if (!string.IsNullOrEmpty(args[1]))
            {
                ds = ds.Where(d => Path.GetFileName(d).StartsWith(args[1])).ToArray();
            }
            // 获取文件夹下的子文件夹列表
            foreach (var d in ds)
            {
                IList<ShopOrder> orders = SheetHelper.ReadShopOrder(Path.Combine(d, "订单.xlsx")).OrderBy(o => o.OrderTime).ToList();
                IList<ShopLend> lendings = SheetHelper.ReadShopLend(Path.Combine(d, "放款.xlsx")).OrderBy(o => o.SettlementTime).ToList();
                IList<ShopRefund> refunds = SheetHelper.ReadShopRefund(Path.Combine(d, "退款.xlsx")).OrderBy(o => o.RefundTime).ToList();

                int index = 1;  // 序号

                Dictionary<int, double> refunddic = new Dictionary<int, double>();
                foreach (var re in refunds)
                {
                    double cost = 0.0;
                    double deduction = 0.0;
                    TotalOrder[] torders = totalOrders.Where(t => t.OrderId.Equals(re.OrderId)).ToArray();
                    ShopOrder[] oorders = orders.Where(o => o.OrderId.Equals(re.OrderId)).ToArray();

                    Console.Write("{0:D2} ", index++);
                    Console.Write("{0} 成交:{1,-8} ", re.OrderId, re.Turnover);

                    Console.Write("退款:");
                    ConsoleC(re.Turnover != re.Refund ? ConsoleColor.Green : Console.ForegroundColor, string.Format("{0,-8} ", re.Refund));

                    if (torders.Length > 0)
                    {
                        Console.Write("分销:");
                        foreach (var i in torders)
                        {
                            cost += i.Cost;
                            ConsoleC(i.Cost < 1 ? ConsoleColor.Green : Console.ForegroundColor, string.Format("{0,-8} ", i.Cost));
                        }
                    }
                    else
                    {
                        Console.Write("              ");
                    }

                    if (torders.Length > 0)
                    {
                        Console.Write("扣款:");
                        foreach (var i in torders)
                        {
                            ConsoleC(i.DeductionAmount < 1 ? ConsoleColor.Green : Console.ForegroundColor, string.Format("{0,-8} ", i.DeductionAmount));
                            if (!refunddic.ContainsKey(re.RefundTime.Month))
                            {
                                refunddic[re.RefundTime.Month] = 0.0;
                            }
                            deduction += i.DeductionAmount;
                            refunddic[re.RefundTime.Month] += i.DeductionAmount;
                        }
                    }
                    else
                    {
                        Console.Write("              ");
                    }

                    if (re.Turnover != re.Refund || cost != deduction)
                    {
                        Console.Write("订单异常 ");
                    }
                    else
                    {
                        Console.Write("         ");
                    }

                    Console.Write("{0, -13:} ", oorders?.FirstOrDefault()?.Country);

                    Console.Write("退款时间:{0, -19:} ", re.RefundTime);

                    DateTime? oTime = oorders?.FirstOrDefault()?.OrderTime;
                    if (oTime != DateTime.MinValue)
                    {
                        Console.Write("订单时间:{0, -19} ", oTime);
                    }
                    else
                    {
                        Console.Write("                            ");
                    }

                    DateTime? pTime = oorders?.FirstOrDefault()?.PaymentTime;
                    if (pTime != DateTime.MinValue)
                    {
                        Console.Write("支付时间:{0, -19} ", pTime);
                    }
                    else
                    {
                        Console.Write("                            ");
                    }


                    DateTime? sTime = oorders?.FirstOrDefault()?.ShippingTime;
                    if (sTime != DateTime.MinValue)
                    {
                        Console.Write("发货时间:{0,-19} ", sTime);
                    }
                    else
                    {
                        Console.Write("                            ");
                    }

                    DateTime? rTime = oorders?.FirstOrDefault()?.ReceiptTime;
                    if (rTime != DateTime.MinValue)
                    {
                        Console.Write("收货时间:{0,-19} ", rTime);
                    }
                    else
                    {
                        Console.Write("                            ");
                    }


                    Console.WriteLine();
                }
                Console.Write($"{Path.GetFileName(d)} 总扣款 ");
                foreach (var item in refunddic.Keys)
                {
                    Console.Write($"{item}月 {refunddic[item]} ");
                }
                Console.WriteLine();

            }
        }
        public static void ConsoleC(ConsoleColor consoleColor, string str)
        {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = consoleColor;
            Console.Write(str);
            Console.ForegroundColor = old;
        }

 

        //public static void Do(DateTime start, DateTime end, string clientId = "", string cn = "")
        //{
        //    List<ShopRecord> sr = null;
        //
        //    if (!string.IsNullOrWhiteSpace(clientId))
        //    {
        //        sr = ShopRecords.Where(s => s.Shop.ClientId.Equals(clientId)).ToList();
        //    }
        //    if(!string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(cn))
        //    {
        //        sr = ShopRecords.Where(s => s.Shop.ClientId.Equals(clientId))
        //            .Where(s => s.Shop.CN.Equals(cn)).ToList();
        //    }
        //    if(string.IsNullOrWhiteSpace(clientId) && string.IsNullOrWhiteSpace(cn))
        //    {
        //        sr = ShopRecords;
        //    }
        //
        //
        //    if(sr!=null && sr.Count > 0)
        //    {
        //        List<ShopOrder> shopOrders = new List<ShopOrder>(); ;
        //        List<ShopLend> shopLend = new List<ShopLend>(); ;
        //        List<ShopRefund> shopRefund = new List<ShopRefund>(); ;
        //
        //        foreach (var item in sr)
        //        {
        //            shopOrders.AddRange(item.ShopOrderList.Where(o => start <= o.PaymentTime && o.PaymentTime <= end).ToList());
        //        }
        //        shopOrders = shopOrders.GroupBy(s => s.OrderId).Select(x => x.First()).ToList();
        //        foreach (var item in sr)
        //        {
        //            shopLend.AddRange(item.ShopLendList);
        //        }
        //        foreach (var item in sr)
        //        {
        //            shopRefund.AddRange(item.ShopRefundList);
        //        }
        //        int    pay = 0,     pay_p = 0, pay_m = 0, pay_p_m = 0, unknow = 0, unknow_p = 0,   allin=0,   allin_p=0,   @in=0,  in_p=0,   out1 = 0,   out1_p = 0,   out2 = 0,   out2_p = 0;
        //        double pay_c = 0.0, pay_p_c = 0.0, pay_m_c = 0.0, pay_p_m_c = 0.0, unknow_c = 0.0, unknow_p_c = 0.0, allin_c=0.0, allin_p_c = 0.0, in_c = 0.0, in_p_c = 0.0, out1_c = 0.0, out1_p_c = 0.0, out2_c = 0.0, out2_p_c = 0.0;
        //        int mark = 0, mark_p = 0;
        //        double mark_c = 0.0, mark_p_c = 0.0;
        //
        //        
        //        foreach (var o in shopOrders)
        //        {
        //            // 判断是否促销单
        //            bool ispro = false;
        //            bool isunknow = true;
        //            double amount = 0.0;
        //
        //
        //            TotalOrder[] tos = TotalOrders.Where(t => t.OrderId.Contains(o.OrderId)).ToArray();
        //            if (tos.Length > 0)
        //            {
        //                //pay_m++;
        //                // 判断是否促销单
        //                if ( tos.First().OrderStatus.Equals("促销单"))
        //                {
        //                    ispro = true;
        //                }
        //
        //                // 判断是否扣款
        //                for (int i = 0; i < tos.Length; i++)
        //                {
        //                    if (!string.IsNullOrWhiteSpace(tos[i].TradeId))
        //                    {
        //                        mark++;
        //                        if (ispro)
        //                        {
        //                            mark_p++;
        //                        }
        //                        break;
        //                    }
        //                }
        //
        //                // 设置扣款总额
        //                foreach (var item in tos.GroupBy(to => to.TradeId).Select(t => t.First()))
        //                {
        //                    mark_c += item.DeductionAmount;
        //                    if (ispro)
        //                    {
        //                        mark_p_c += item.DeductionAmount;
        //                    }
        //                    amount += item.DeductionAmount;
        //                }
        //            }
        //            else
        //            {
        //                if (o.ShippingTime != DateTime.MinValue)
        //                {
        //                    Console.WriteLine($"{o.OrderId} {o.PaymentTime} {o.FileName}");
        //                }
        //                    
        //            }
        //
        //            // 判断是否已发货
        //            pay++;
        //            pay_c += o.OrderAmount;
        //            if (ispro)
        //            {
        //                pay_p++;
        //                pay_p_c += o.OrderAmount;
        //            }
        //            if (o.ShippingTime == DateTime.MinValue)
        //            {
        //                pay_m++;
        //                pay_m_c += o.OrderAmount;
        //                if (ispro)
        //                {
        //                    pay_p_m++;
        //                    pay_p_m_c += o.OrderAmount;
        //                }
        //            }
        //
        //
        //            // 判断是否放款
        //            ShopLend lend = shopLend.Where(l=>l.OrderId.Equals(o.OrderId)).FirstOrDefault();
        //            if(lend != null)
        //            {
        //                isunknow = false;
        //                allin++;
        //                allin_c += lend.Lend;
        //                @in++;
        //                @in_c += lend.Lend - lend.Fee - lend.Affiliate - lend.Cashback;
        //                if (ispro)
        //                {
        //                    allin_p++;
        //                    allin_p_c += lend.Lend;
        //                    in_p++;
        //                    @in_p_c += lend.Lend - lend.Fee - lend.Affiliate - lend.Cashback;
        //                }
        //            }
        //
        //            // 判断是否退款单
        //            ShopRefund refund = shopRefund.Where(l => l.OrderId.Equals(o.OrderId)).FirstOrDefault();
        //            if (refund != null)
        //            {
        //                isunknow = false;
        //                if (refund.Turnover != refund.Refund)  // 退款金额与成交金额不相同
        //                {
        //                    out2++;
        //                    out2_c += refund.Refund;
        //                    if (ispro)
        //                    {
        //                        out2_p++;
        //                        out2_p_c += refund.Refund;
        //                    }
        //                }
        //                else
        //                {
        //                    out1++;
        //                    out1_c += refund.Refund;
        //                    if (ispro)
        //                    {
        //                        out1_p++;
        //                        out1_p_c += refund.Refund;
        //                    }
        //                }
        //            }
        //
        //            // 在途订单
        //            if (isunknow)
        //            {
        //                unknow++;
        //                unknow_c += amount;
        //                if (ispro)
        //                {
        //                    unknow_p++;
        //                    unknow_p_c += amount;
        //                }
        //            }
        //
        //        } // 结束订单循环
        //        string str = $"订单总数：{pay} 支付总数：{pay_m} 促销总数：{pay_p} 促销支付总数：{pay_p_m}\r\n"
        //        + $"订单总额：{pay_c:0.00} 促销总额：{pay_p_c:0.00}\r\n"
        //        + $"标发总数：{mark} 促销标发总数：{mark_p} 标发扣款总额：{mark_c:0.00} 促销扣款总额：{mark_p_c:0.00}\r\n"
        //        + $"在途总数：{unknow} 促销在途总数：{unknow_p} 在途总额：{unknow_c:0.00} 促销在途总额：{unknow_p_c:0.00}\r\n"
        //        + $"放款总数：{allin} 促销放款总数：{allin_p} 放款总额：{allin_c:0.00} 促销放款总额：{allin_p_c:0.00}\r\n"
        //        + $"入账总数：{@in} 促销入账总数：{in_p} 入账总额：{in_c:0.00} 促销入账总额：{in_p_c:0.00}\r\n"
        //        + $"退款总数：{out1} 促销退款总数：{out1_p} 退款总额：{out1_c:0.00} 促销退款总额：{out1_p_c:0.00}\r\n"
        //        + $"部分退款总数：{out2} 促销部分退款总数：{out2_p} 部分退款总额：{out2_c:0.00} 部分退款总额：{out2_p_c:0.00}\r\n";
        //        Console.WriteLine(str);
        //    }
        //}
    }
}
