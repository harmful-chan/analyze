using analyze.Models;
using analyze.Models.Manage;
using analyze.Options;
using CommandLine;
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

        public static int Main(string[] args)
        {
            var exitCode = -1;
            try 
            {
                exitCode = Parser.Default.ParseArguments<ManageOptions, CollectOptions>(args).MapResult(
                    (ManageOptions o) => ManageRun(o),(CollectOptions o) => CollectRun(o),error => 1);

                Console.ReadKey();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return exitCode;
        }

        private static int ManageRun(ManageOptions o)
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
            return 0;
        }

        #region 采集数据
        static readonly string datadir = @"F:\BaiduSyncdisk\Desktop\12月";
        static readonly string rawdir = Path.Combine(datadir, "原始数据", "2024-01-25");
        static readonly string 店铺记录 = Path.Combine(rawdir, "店铺记录.xlsx");
        static readonly string 订单记录 = Path.Combine(rawdir, "订单总表.xlsx");
        static readonly string 历史采购单 = Path.Combine(rawdir, "历史采购单.xlsx");
        static readonly string 巴西采购单 = Path.Combine(rawdir, "巴西采购单.xlsx");
        static List<ShopRecord> ShopRecords = new List<ShopRecord>();
        static List<TotalOrder> TotalOrders = new List<TotalOrder>();
        static List<TotalPurchase> TotalPurchases = new List<TotalPurchase>();
        class ShopRecord
        {
            public Shop Shop { get; set; }
            public IList<ShopOrder> ShopOrderList { get; set; }
            public IList<ShopLend> ShopLendList { get; set; }
            public IList<ShopRefund> ShopRefundList { get; set; }
        }
        private static int CollectRun(CollectOptions opt)
        {
            SheetHelper.Log = Console.WriteLine;
            TotalOrders = SheetHelper.ReadTotalOrder(订单记录).ToList();
            IList<TotalPurchase> totalPurchases1 = SheetHelper.TotalPurchase(1, 巴西采购单);
            IList<TotalPurchase> totalPurchases2 = SheetHelper.TotalPurchase(2, 历史采购单);
            TotalPurchases.AddRange(totalPurchases1);
            TotalPurchases.AddRange(totalPurchases2);
            IList<Shop> shops = SheetHelper.ReadShopInfo(店铺记录);
            foreach (var shop in shops)
            {
                string[] ds = Directory.GetDirectories(datadir);
                ds = ds.Where(d => Path.GetFileName(d).StartsWith($"{shop.CompanyNumber} {shop.CN}")).ToArray();
                if (ds.Length == 1)
                {
                    IList<ShopOrder> orders = SheetHelper.ReadShopOrder(Path.Combine(ds[0], "订单.xlsx")).OrderBy(o => o.OrderTime).ToList();
                    IList<ShopLend> lendings = SheetHelper.ReadShopLending(Path.Combine(ds[0], "放款.xlsx")).OrderBy(o => o.SettlementTime).ToList();
                    IList<ShopRefund> refunds = SheetHelper.ReadShopRefund(Path.Combine(ds[0], "退款.xlsx")).OrderBy(o => o.RefundTime).ToList();

                    ShopRecords.Add(new ShopRecord() { Shop = shop, ShopOrderList = orders, ShopLendList = lendings, ShopRefundList = refunds });
                }

            }
            CheckData();


            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (line.EndsWith("quit"))
                {
                    break;
                }
                else
                {
                    string[] vs = line.Split(' ');
                    DateTime t1;
                    DateTime.TryParseExact($"{vs[0]} {vs[1]}", "yyyy-MM-dd HH:mm:ss", new CultureInfo("zh-cn"), DateTimeStyles.None, out t1);
                    DateTime t2;
                    DateTime.TryParseExact($"{vs[2]} {vs[3]}", "yyyy-MM-dd HH:mm:ss", new CultureInfo("zh-cn"), DateTimeStyles.None, out t2);
                    if (vs.Length == 4)
                    {
                        Do(t1, t2);
                    }
                    else if (vs.Length == 5)
                    {
                        Do(t1, t2, vs[4]);
                    }
                    else if (vs.Length == 6)
                    {
                        Do(t1, t2, vs[4], vs[5]);
                    }
                }
            }

            return 0;
        }

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

        public static bool CheckData()
        {
            bool ret = true;
            // 订单总表
            /// 非采购单 促销单 ： 订单号 交易号 扣款金额不完整。
            foreach (var item in TotalOrders)
            {
                if( !string.IsNullOrWhiteSpace(item.OrderId) && string.IsNullOrWhiteSpace(item.TradeId) || item.DeductionAmount < 0)
                {
                    if(!item.OrderStatus.Equals("采购单") && !item.OrderStatus.Equals("促销单")){
                        Console.WriteLine($"{item.StoreName} {item.OrderId} {item.TradeId} {item.DeductionAmount}");
                        ret = false;
                    }
                }
            }
            return ret;
        }

        public static void Do(DateTime start, DateTime end, string clientId = "", string cn = "")
        {
            List<ShopRecord> sr = null;

            if (!string.IsNullOrWhiteSpace(clientId))
            {
                sr = ShopRecords.Where(s => s.Shop.ClientId.Equals(clientId)).ToList();
            }
            if(!string.IsNullOrWhiteSpace(clientId) && !string.IsNullOrWhiteSpace(cn))
            {
                sr = ShopRecords.Where(s => s.Shop.ClientId.Equals(clientId))
                    .Where(s => s.Shop.CN.Equals(cn)).ToList();
            }
            if(string.IsNullOrWhiteSpace(clientId) && string.IsNullOrWhiteSpace(cn))
            {
                sr = ShopRecords;
            }


            if(sr!=null && sr.Count > 0)
            {
                List<ShopOrder> shopOrders = new List<ShopOrder>(); ;
                List<ShopLend> shopLend = new List<ShopLend>(); ;
                List<ShopRefund> shopRefund = new List<ShopRefund>(); ;

                foreach (var item in sr)
                {
                    shopOrders.AddRange(item.ShopOrderList.Where(o => start <= o.PaymentTime && o.PaymentTime <= end).ToList());
                }
                shopOrders = shopOrders.GroupBy(s => s.OrderId).Select(x => x.First()).ToList();
                foreach (var item in sr)
                {
                    shopLend.AddRange(item.ShopLendList);
                }
                foreach (var item in sr)
                {
                    shopRefund.AddRange(item.ShopRefundList);
                }
                int    pay = 0,     pay_p = 0, pay_m = 0, pay_p_m = 0, unknow = 0, unknow_p = 0,   allin=0,   allin_p=0,   @in=0,  in_p=0,   out1 = 0,   out1_p = 0,   out2 = 0,   out2_p = 0;
                double pay_c = 0.0, pay_p_c = 0.0, pay_m_c = 0.0, pay_p_m_c = 0.0, unknow_c = 0.0, unknow_p_c = 0.0, allin_c=0.0, allin_p_c = 0.0, in_c = 0.0, in_p_c = 0.0, out1_c = 0.0, out1_p_c = 0.0, out2_c = 0.0, out2_p_c = 0.0;
                int mark = 0, mark_p = 0;
                double mark_c = 0.0, mark_p_c = 0.0;

                
                foreach (var o in shopOrders)
                {
                    // 判断是否促销单
                    bool ispro = false;
                    bool isunknow = true;
                    double amount = 0.0;


                    TotalOrder[] tos = TotalOrders.Where(t => t.OrderId.Contains(o.OrderId)).ToArray();
                    if (tos.Length > 0)
                    {
                        //pay_m++;
                        // 判断是否促销单
                        if ( tos.First().OrderStatus.Equals("促销单"))
                        {
                            ispro = true;
                        }

                        // 判断是否扣款
                        for (int i = 0; i < tos.Length; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(tos[i].TradeId))
                            {
                                mark++;
                                if (ispro)
                                {
                                    mark_p++;
                                }
                                break;
                            }
                        }

                        // 设置扣款总额
                        foreach (var item in tos.GroupBy(to => to.TradeId).Select(t => t.First()))
                        {
                            mark_c += item.DeductionAmount;
                            if (ispro)
                            {
                                mark_p_c += item.DeductionAmount;
                            }
                            amount += item.DeductionAmount;
                        }
                    }
                    else
                    {
                        if (o.ShippingTime != DateTime.MinValue)
                        {
                            Console.WriteLine($"{o.OrderId} {o.PaymentTime} {o.FileName}");
                        }
                            
                    }

                    // 判断是否已发货
                    pay++;
                    pay_c += o.OrderAmount;
                    if (ispro)
                    {
                        pay_p++;
                        pay_p_c += o.OrderAmount;
                    }
                    if (o.ShippingTime == DateTime.MinValue)
                    {
                        pay_m++;
                        pay_m_c += o.OrderAmount;
                        if (ispro)
                        {
                            pay_p_m++;
                            pay_p_m_c += o.OrderAmount;
                        }
                    }


                    // 判断是否放款
                    ShopLend lend = shopLend.Where(l=>l.OrderId.Equals(o.OrderId)).FirstOrDefault();
                    if(lend != null)
                    {
                        isunknow = false;
                        allin++;
                        allin_c += lend.Lend;
                        @in++;
                        @in_c += lend.Lend - lend.Fee - lend.Affiliate - lend.Cashback;
                        if (ispro)
                        {
                            allin_p++;
                            allin_p_c += lend.Lend;
                            in_p++;
                            @in_p_c += lend.Lend - lend.Fee - lend.Affiliate - lend.Cashback;
                        }
                    }

                    // 判断是否退款单
                    ShopRefund refund = shopRefund.Where(l => l.OrderId.Equals(o.OrderId)).FirstOrDefault();
                    if (refund != null)
                    {
                        isunknow = false;
                        if (refund.Turnover != refund.Refund)  // 退款金额与成交金额不相同
                        {
                            out2++;
                            out2_c += refund.Refund;
                            if (ispro)
                            {
                                out2_p++;
                                out2_p_c += refund.Refund;
                            }
                        }
                        else
                        {
                            out1++;
                            out1_c += refund.Refund;
                            if (ispro)
                            {
                                out1_p++;
                                out1_p_c += refund.Refund;
                            }
                        }
                    }

                    // 在途订单
                    if (isunknow)
                    {
                        unknow++;
                        unknow_c += amount;
                        if (ispro)
                        {
                            unknow_p++;
                            unknow_p_c += amount;
                        }
                    }

                } // 结束订单循环
                string str = $"订单总数：{pay} 支付总数：{pay_m} 促销总数：{pay_p} 促销支付总数：{pay_p_m}\r\n"
                + $"订单总额：{pay_c:0.00} 促销总额：{pay_p_c:0.00}\r\n"
                + $"标发总数：{mark} 促销标发总数：{mark_p} 标发扣款总额：{mark_c:0.00} 促销扣款总额：{mark_p_c:0.00}\r\n"
                + $"在途总数：{unknow} 促销在途总数：{unknow_p} 在途总额：{unknow_c:0.00} 促销在途总额：{unknow_p_c:0.00}\r\n"
                + $"放款总数：{allin} 促销放款总数：{allin_p} 放款总额：{allin_c:0.00} 促销放款总额：{allin_p_c:0.00}\r\n"
                + $"入账总数：{@in} 促销入账总数：{in_p} 入账总额：{in_c:0.00} 促销入账总额：{in_p_c:0.00}\r\n"
                + $"退款总数：{out1} 促销退款总数：{out1_p} 退款总额：{out1_c:0.00} 促销退款总额：{out1_p_c:0.00}\r\n"
                + $"部分退款总数：{out2} 促销部分退款总数：{out2_p} 部分退款总额：{out2_c:0.00} 部分退款总额：{out2_p_c:0.00}\r\n";
                Console.WriteLine(str);
            }
        }
    }
}
