using analyze.Models;
using analyze.Models.Manage;
using analyze.Options;
using CommandLine;
using ConsoleTables;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
                SheetHelper.Log = Console.WriteLine;
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

        private static void RefundRun(RefundOptions o)
        {
            string datadir = @"D:\BaiduSyncdisk\公司\寰球易贸\利润分成\1月";
            string rawdir = @"D:\BaiduSyncdisk\公司\寰球易贸\利润分成\1月\原始数据\2024-01-25";

            if (o.IsList)
            {


                if (o.DirPrefix != null && o.DirPrefix.Count() > 0)
                {
                    // 获取文件夹下的子文件夹列表
                    foreach (var p in o.DirPrefix)
                    {
                        SheetHelper.Collect(rawdir, datadir, p);
                    }
                }
                else
                {
                    SheetHelper.Collect(rawdir, datadir);
                }
                ManageClient client = new ManageClient();
                client.LoginAdminAsync();
                User[] users = client.ListUsers();
                foreach (var shop in SheetHelper.ShopRecords)
                {
                    double t_refund = 0.0;
                    int index = 1;  // 序号
                    DebitRecord[] debitRecords = client.ListDebitRecord(shop.Shop.ClientId);
                    ConsoleTable table = new ConsoleTable("I", "Order", "Turnover", "Refund", "Cost", "Deduction", "Status", "Country", "RefundTime", "OrderTime", "PaymentTime",  "ShippingTime", "ReceiptTime");
                    foreach (var r in shop.ShopRefundList)
                    {
                        bool flag = true;

                        ShopOrder[] shopOrders = shop.ShopOrderList.Where(t => t.OrderId.Equals(r.OrderId)).ToArray();
                        TotalOrder[] torders = SheetHelper.TotalOrders.Where(t => t.OrderId.Equals(r.OrderId)).ToArray();
                        if (r.Turnover != r.Refund)
                        {
                            flag = false;
                        }
                        if(torders.Length > 0 && torders.First().Cost != torders.First().DeductionAmount)
                        {
                            flag = false;
                        }

                        if (torders.Length > 0 && torders.First().Cost != torders.First().DeductionAmount)
                        {
                            t_refund += torders.First().DeductionAmount;
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
                        if (torders.Length > 0)
                        {
                            
                            DebitRecord[] array = debitRecords.Where(d => d.TradeId.Equals(torders.FirstOrDefault().TradeId)).ToArray();
                            double tcost = 0.0;
                            foreach (var item in array)
                            {
                                double cost = 0.0;
                                double.TryParse(item.Cost, out cost);
                                tcost += cost;

                            }
                            table.AddRow(index++, r.OrderId, r.Turnover, r.Refund, torders.FirstOrDefault()?.Cost, tcost, flag ? "" : "X",shopOrders.FirstOrDefault()?.Country, a, ot, pt,st, rt);
                        }
                        else
                        {
                            table.AddRow(index++, r.OrderId, r.Turnover, r.Refund, "", "", flag ? "" : "X",shopOrders.FirstOrDefault()?.Country, a, ot, pt, st, rt);
                        }

                    }

                    table.AddRow(index++, "TOTAL", "", "", "", t_refund, "", "", "", "", "", "", "");
                    table.Write(Format.MarkDown);
                    

                }
            }
        }

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

            if(o.ClientId != null)
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
                IList<ShopLend> lendings = SheetHelper.ReadShopLending(Path.Combine(d, "放款.xlsx")).OrderBy(o => o.SettlementTime).ToList();
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
#endregion