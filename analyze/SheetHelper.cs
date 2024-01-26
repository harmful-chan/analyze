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
    public class SheetHelper
    {
        public delegate void LogDelegate(string arg);
        private static LogDelegate _log;
        public static LogDelegate Log
        {
            get 
            {
                if(_log == null)
                {
                    throw new Exception("LogDelegate NOT SET");
                }
                return _log; 
            }
            set { _log = value; }
        }


        public static IList<TotalPurchase> TotalPurchase(string fileName)
        {
            List<TotalPurchase> os = new List<TotalPurchase>();
            string[] ss = ReadSortFiles(fileName);
            for (int c = 0; c < ss.Length; c++)
            {
                using (FileStream fs = new FileStream(ss[c], FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = WorkbookFactory.Create(fs);
                    Log($"读取 {ss[c]}");
                    var sheet = workbook.GetSheetAt(0);
                    if (sheet != null && sheet.LastRowNum + 1 >= 2)
                    {
                        for (int i = 1; i < sheet.LastRowNum + 1; i++)
                        {
                            var row = sheet.GetRow(i);

                            TotalPurchase to = new TotalPurchase();
                            to.OrderId = row.GetCell(6)?.ToString();
                            to.Status = row.GetCell(20)?.ToString();
                            os.Add(to);
                        }
                    }
                }
            }
            return os.ToList();
        }
        public static IList<ShopRefund> ReadShopRefund(string fileName)
        {
            List<ShopRefund> os = new List<ShopRefund>();
            string[] ss = ReadSortFiles(fileName);
            for (int c = 0; c < ss.Length; c++)
            {
                using (FileStream fs = new FileStream(ss[c], FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = WorkbookFactory.Create(fs);
                    Log($"读取 {ss[c]}");
                    var sheet = workbook.GetSheetAt(0);
                    if (sheet != null && sheet.LastRowNum + 1 >= 2)
                    {
                        for (int i = 1; i < sheet.LastRowNum + 1; i++)
                        {
                            var row = sheet.GetRow(i);

                            ShopRefund to = new ShopRefund();

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
        public static IList<ShopLend> ReadShopLending(string fileName)
        {
            List<ShopLend> os = new List<ShopLend>();
            string[] ss = ReadSortFiles(fileName);
            for (int c = 0; c < ss.Length; c++)
            {
                using (FileStream fs = new FileStream(ss[c], FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = WorkbookFactory.Create(fs);
                    Log($"读取 {ss[c]}");
                    var sheet = workbook.GetSheetAt(0);
       
                    if (sheet != null && sheet.LastRowNum > 2)
                    {
                        for (int i = 1; i < sheet.LastRowNum + 1; i++)
                        {
                            var row = sheet.GetRow(i);

                            ShopLend to = new ShopLend();



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
        public static IList<ShopOrder> ReadShopOrder(string fileName)
        {
            List<ShopOrder> os = new List<ShopOrder>();
            string[] ss = ReadSortFiles(fileName);
            for (int c = 0; c < ss.Length; c++)
            {
                using (FileStream fs = new FileStream(ss[c], FileMode.Open, FileAccess.Read))
                {
                    Log($"读取 {ss[c]}");
                    IWorkbook workbook = WorkbookFactory.Create(fs);
                    var sheet = workbook.GetSheetAt(0);
                    if (sheet != null && sheet.LastRowNum > 2)
                    {
                        for (int i = 1; i < sheet.LastRowNum + 1; i++)
                        {
                            var row = sheet.GetRow(i);

                            ShopOrder to = new ShopOrder();
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
            string errs = "";
            string[] ss = ReadSortFiles(fileName);
            for (int c = 0; c < ss.Length; c++)
            {
                using (FileStream fs = new FileStream(ss[c], FileMode.Open, FileAccess.Read))
                {
                    Log($"读取 {ss[c]}");
                    IWorkbook workbook = WorkbookFactory.Create(fs);
                    var sheet = workbook.GetSheetAt(0);
                    if (sheet != null && sheet.LastRowNum >= 1)
                    {
                        for (int i = 0; i < sheet.LastRowNum + 1; i++)
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
                            double amount = -1.1;
                            double.TryParse(row.GetCell(18)?.ToString().Trim().Replace("RMB", "").Replace("R", "").Replace("$", ""), out amount);
                            to.DeductionAmount = amount;

                            if((!string.IsNullOrWhiteSpace(to.TradeId) && amount == -1.1) ||
                                (string.IsNullOrWhiteSpace(to.TradeId) && amount != -1.1))
                            {
                                errs += $"{to.OrderId} Trade error.\r\n";
                            }
                            os.Add(to);
                        }

                    }
                }
            }
            if(!string.IsNullOrWhiteSpace(errs))
            {
                throw new Exception(errs);
            }
            return os;
        }
        public static string[] ReadSortFiles(string fileName)
        {
            string[] fs = Directory.GetFiles(Path.GetDirectoryName(fileName));
            fs = fs.Where(s => s.Contains(fileName.Replace(Path.GetExtension(fileName), ""))).ToArray();
            Array.Sort(fs);
            return fs;
        }
    }
}
