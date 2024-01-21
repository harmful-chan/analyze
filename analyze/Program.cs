using analyze.Models;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze
{
    public class Program
    {

        public static void Main(string[] args)
        {
            if (args[0].Equals("ListRefunds"))
            {
                ListRefunds(args[1], args[2]);
            }
        }


        /// <summary>
        /// args[0] 订单 xlsx路径
        /// args[1] 文件夹前缀
        /// </summary>
        /// <param name="args"></param>
        public static void ListRefunds(params string[] args)
        {
            // 读订单总表
            IList<TotalOrder> totalOrders = ReadTotalOrder(args[0]);

            // 获取当前目录下的文件夹
            string[] ds = Directory.GetDirectories(Path.GetDirectoryName(args[0]));
            if (!string.IsNullOrEmpty(args[1]))
            {
                ds = ds.Where(d => Path.GetFileName(d).StartsWith(args[1])).ToArray();
            }
            // 获取文件夹下的子文件夹列表
            foreach (var d in ds)
            {
                IList<Order> orders = ReadOrder(Path.Combine(d, "订单.xlsx")).OrderBy(o => o.OrderTime).ToList();
                IList<Lending> lendings = ReadLending(Path.Combine(d, "放款.xlsx")).OrderBy(o => o.SettlementTime).ToList();
                IList<Refund> refunds = ReadRefund(Path.Combine(d, "退款.xlsx")).OrderBy(o => o.RefundTime).ToList();

                int index = 1;  // 序号

                Dictionary<int, double> refunddic = new Dictionary<int, double>();
                foreach (var re in refunds)
                {
                    double cost = 0.0;
                    double deduction = 0.0;
                    TotalOrder[] torders = totalOrders.Where(t => t.OrderId.Equals(re.OrderId)).ToArray();
                    Order[] oorders = orders.Where(o => o.OrderId.Equals(re.OrderId)).ToArray();

                    Console.Write("{0:D2} ", index++);
                    Console.Write("{0} 成交:{1,-8} ", re.OrderId, re.Turnover);

                    Console.Write("退款:");
                    ConsoleC(re.Turnover != re.RefundAmount ? ConsoleColor.Green : Console.ForegroundColor, string.Format("{0,-8} ", re.RefundAmount));

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

                    if (re.Turnover != re.RefundAmount || cost != deduction)
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
        public static IList<Refund> ReadRefund(string fileName)
        {
            List<Refund> os = new List<Refund>();
            string[] ss = ReadSortFiles(fileName);
            for (int c = 0; c < ss.Length; c++)
            {
                using (FileStream fs = new FileStream(ss[c], FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = WorkbookFactory.Create(fs);
                    Console.WriteLine($"读取 {ss[c]}");
                    var sheet = workbook.GetSheetAt(0);
                    if (sheet != null && sheet.LastRowNum + 1 >= 2)
                    {
                        for (int i = 1; i < sheet.LastRowNum + 1; i++)
                        {
                            var row = sheet.GetRow(i);

                            Refund to = new Refund();

                            to.OrderId = row.GetCell(0)?.ToString();
                            to.ProductId = row.GetCell(1)?.ToString();

                            int d1;
                            int.TryParse(row.GetCell(5)?.ToString(), out d1);
                            to.Quantity = d1;

                            double d2;
                            double.TryParse(row.GetCell(6)?.ToString(), out d2);
                            to.Turnover = d2;

                            double d3;
                            double.TryParse(row.GetCell(7)?.ToString(), out d3);
                            to.RefundAmount = d3;


                            DateTime t1;
                            DateTime.TryParseExact(row.GetCell(12)?.ToString(), "yyyy-MM-dd HH:mm:ss", new CultureInfo("zh-cn"), DateTimeStyles.None, out t1);
                            to.RefundTime = t1;

                            os.Add(to);
                        }
                    }
                }
            }
                
            return os.GroupBy(o => o.OrderId).Select(s => s.First()).ToList();
        }
        public static IList<Lending> ReadLending(string fileName)
        {
            List<Lending> os = new List<Lending>();
            string[] ss = ReadSortFiles(fileName);
            for (int c = 0; c < ss.Length; c++)
            {
                using (FileStream fs = new FileStream(ss[c], FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = WorkbookFactory.Create(fs);
                    Console.WriteLine($"读取 {ss[c]}");
                    var sheet = workbook.GetSheetAt(0);
                    if (sheet != null && sheet.LastRowNum > 2)
                    {
                        for (int i = 1; i < sheet.LastRowNum + 1; i++)
                        {
                            var row = sheet.GetRow(i);

                            Lending to = new Lending();



                            to.ProductId = row.GetCell(0)?.ToString();

                            double d1;
                            double.TryParse(row.GetCell(6)?.ToString(), out d1);
                            to.Turnover = d1;

                            double d2;
                            double.TryParse(row.GetCell(7)?.ToString(), out d2);
                            to.RebateAmount = d2;

                            double d3;
                            double.TryParse(row.GetCell(8)?.ToString(), out d3);
                            to.TradeAmount = d1;

                            double d4;
                            double.TryParse(row.GetCell(9)?.ToString(), out d4);
                            to.AffiliateAmount = d4;

                            double d5;
                            double.TryParse(row.GetCell(10)?.ToString(), out d5);
                            to.Cashback = d5;


                            DateTime t1;
                            DateTime.TryParseExact(row.GetCell(11)?.ToString(), "yyyy-MM-dd HH:mm:ss", new CultureInfo("zh-cn"), DateTimeStyles.None, out t1);
                            to.SettlementTime = t1;

                            to.OrderId = row.GetCell(12)?.ToString();

                            os.Add(to);
                        }
                    }
                }
            }
            return os;
        }
        public static IList<Order> ReadOrder(string fileName)
        {
            List<Order> os = new List<Order>();
            string[] ss = ReadSortFiles(fileName);
            for (int c = 0; c < ss.Length; c++)
            {
                using (FileStream fs = new FileStream(ss[c], FileMode.Open, FileAccess.Read))
                {
                    Console.WriteLine($"读取 {ss[c]}");
                    IWorkbook workbook = WorkbookFactory.Create(fs);
                    var sheet = workbook.GetSheetAt(0);
                    if (sheet != null && sheet.LastRowNum > 2)
                    {
                        for (int i = 1; i < sheet.LastRowNum + 1; i++)
                        {
                            var row = sheet.GetRow(i);

                            Order to = new Order();
                            to.FileName = ss[c];

                            to.OrderId = row.GetCell(0)?.ToString();
                            to.OrderStatus = row.GetCell(1)?.ToString();

                            DateTime t1;
                            DateTime.TryParseExact(row.GetCell(4)?.ToString(), "yyyy-MM-dd HH:mm", new CultureInfo("zh-cn"), DateTimeStyles.None, out t1);
                            to.OrderTime = t1;


                            DateTime t2;
                            DateTime.TryParseExact(row.GetCell(5)?.ToString(), "yyyy-MM-dd HH:mm", new CultureInfo("zh-cn"), DateTimeStyles.None, out t2);
                            to.PaymentTime = t2;


                            double d1;
                            double.TryParse(row.GetCell(10)?.ToString(), out d1);
                            to.OrderAmount = d1;


                            to.Country = row.GetCell(17)?.ToString();

                            to.TradeNumber = row.GetCell(26)?.ToString();


                            DateTime t3;
                            string v = row.GetCell(27)?.ToString();
                            DateTime.TryParseExact(v.Replace("\n", ""), "yyyy-MM-dd HH:mm", new CultureInfo("zh-cn"), DateTimeStyles.None, out t3);
                            to.ShippingTime = t3;

                            DateTime t4;
                            DateTime.TryParseExact(row.GetCell(28)?.ToString(), "yyyy-MM-dd HH:mm", new CultureInfo("zh-cn"), DateTimeStyles.None, out t4);
                            to.ReceiptTime = t4;
                            os.Add(to);
                        }
                    }
                }
            }

            return os;
        }
        public static IList<TotalOrder> ReadTotalOrder(string fileName)
        {
            List<TotalOrder> os = new List<TotalOrder>();
            string[] ss = ReadSortFiles(fileName);
            for (int c = 0; c < ss.Length; c++)
            {
                using (FileStream fs = new FileStream(ss[c], FileMode.Open, FileAccess.Read))
                {
                    Console.WriteLine($"读取 {ss[c]}");
                    IWorkbook workbook = WorkbookFactory.Create(fs);
                    var sheet = workbook.GetSheetAt(0);
                    if (sheet != null && sheet.LastRowNum >= 1)
                    {
                        for (int i = 0; i < sheet.LastRowNum+1; i++)
                        {
                            var row = sheet.GetRow(i);



                            TotalOrder to = new TotalOrder();
                            to.StoreName = row.GetCell(0)?.ToString();
                            to.Operator = row.GetCell(1)?.ToString();
                            to.OrderId = row.GetCell(4)?.ToString();


                            string time = row.GetCell(7)?.ToString();
                            DateTime dateTime = new DateTime();
                            DateTime.TryParseExact(time, "yyyy/MM/dd hh:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out dateTime);
                            to.OrderTime = dateTime;

                            double cost;
                            double.TryParse(row.GetCell(13)?.ToString(), out cost);
                            to.Cost = cost;

                            to.OrderPrice = row.GetCell(14)?.ToString();
                            to.TradeId = row.GetCell(17)?.ToString();
                            double amount;
                            double.TryParse(row.GetCell(18)?.ToString().Trim().Replace("RMB", "").Replace("R", "").Replace("$", ""), out amount);
                            to.DeductionAmount = amount;


                            os.Add(to);
                        }

                    }
                }
            }

            return os;
        }

        public static string[] ReadSortFiles(string fileName)
        {
            string[] fs = Directory.GetFiles(Path.GetDirectoryName(fileName));
            fs = fs.Where(s => s.Contains( fileName.Replace(Path.GetExtension(fileName), ""))).ToArray();
            Array.Sort(fs);
            return fs;
        }
    }
}
