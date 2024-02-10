using analyze.Models;
using NPOI.SS.Formula.Functions;
using NPOI.SS.Formula.PTG;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
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

        public static string Message { get; set; }


        #region XLSX操作
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">1,巴西采购；2，美国采购</param>
        /// <param name="fileName">采购单路径</param>
        /// <returns></returns>
        public static IList<TotalPurchase> TotalPurchase(int type, string fileName)
        {
            List<TotalPurchase> os = new List<TotalPurchase>();
            string[] files = ReadSortFiles(fileName);  // 读取相同文件
            for (int i = 0; i < files.Length; i++)
            {
                using (FileStream fs = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = WorkbookFactory.Create(fs);
                    Log($"读取 {files[i]}");
                    var sheet = workbook.GetSheetAt(0);
                    if (sheet != null)
                    {
                        for (int j = 0; j < sheet.LastRowNum ; j++)
                        {
                            var row = sheet.GetRow(j);
                            if(row == null)
                            {
                                continue;
                            }

                            TotalPurchase to = new TotalPurchase();
                            string A1 = row.GetCell(0)?.ToString();
                            string B1 = row.GetCell(0)?.ToString();
                            // 去除表格 us A1空 B1=操作人
                            if(string.IsNullOrWhiteSpace(A1) && "操作人".Equals(B1) || "序号".Equals(A1) && "日期".Equals(B1))
                            {
                                continue;
                            }


                            if (row != null && type == 1 )
                            {
                                to.OrderId = row.GetCell(6)?.ToString();
                                to.Status = row.GetCell(22)?.ToString();
                                os.Add(to);
                            }
                            else if(row != null && type == 2)
                            {
                                to.OrderId = row.GetCell(5)?.ToString();
                                to.Status = row.GetCell(23)?.ToString();
                                os.Add(to);
                            }
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
                            to.ProductName = row.GetCell(2)?.ToString();
                            to.SUK = row.GetCell(3)?.ToString();
                            to.ProductCode = row.GetCell(4)?.ToString();

                            int d1;
                            int.TryParse(row.GetCell(5)?.ToString(), out d1);
                            to.Quantity = d1;

                            double d2;
                            double.TryParse(row.GetCell(6)?.ToString(), out d2);
                            to.Turnover = d2;

                            double d3;
                            double.TryParse(row.GetCell(7)?.ToString(), out d3);
                            to.Refund = d3;

                            to.Sources = row.GetCell(8)?.ToString();

                            double d4;
                            double.TryParse(row.GetCell(9)?.ToString(), out d4);
                            to.Fee = d4;

                            double d5;
                            double.TryParse(row.GetCell(10)?.ToString(), out d5);
                            to.Fee = d5;

                            double d6;
                            double.TryParse(row.GetCell(11)?.ToString(), out d6);
                            to.Alliance = d6;

                            double d7;
                            double.TryParse(row.GetCell(11)?.ToString(), out d7);
                            to.Cashback = d7;


                            DateTime t1;
                            DateTime.TryParseExact(row.GetCell(12)?.ToString(), "yyyy-MM-dd HH:mm:ss", new CultureInfo("zh-cn"), DateTimeStyles.None, out t1);
                            to.RefundTime = t1;

                            to.FileName = ss[c];

                            os.Add(to);
                        }
                    }
                }
            }

            return os.GroupBy(o => o.OrderId).Select(s => s.First()).ToList();
        }

        public static void SaveShopRefund(string fileName, ShopRefund[] shopRefunds, bool overwrite)
        {
            File.Copy(Path.Combine("resources", "refund.xlsx"), fileName, overwrite);

            IWorkbook workbook;
            using (FileStream fs = File.OpenRead(fileName))
            {
                workbook =WorkbookFactory.Create(fs);
                
            }
            ISheet sheet = workbook.GetSheetAt(0);
            for (int i = 0; i < shopRefunds.Length; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                


                ICell cell0 = row.CreateCell(0); cell0.SetCellType(CellType.Numeric); cell0.SetCellValue(i + 1);
                ICell cell1 = row.CreateCell(1); cell1.SetCellType(CellType.String); cell1.SetCellValue(shopRefunds[i].OrderId);
                ICell cell2 = row.CreateCell(2); cell2.SetCellType(CellType.String); cell2.SetCellValue(shopRefunds[i].ProductId);
                ICell cell3 = row.CreateCell(3); cell3.SetCellType(CellType.String); cell3.SetCellValue(shopRefunds[i].ProductName);
                ICell cell4 = row.CreateCell(4); cell4.SetCellType(CellType.String); cell4.SetCellValue(shopRefunds[i].SUK);
                ICell cell5 = row.CreateCell(5); cell5.SetCellType(CellType.String); cell5.SetCellValue(shopRefunds[i].ProductCode);
                ICell cell6 = row.CreateCell(6); cell6.SetCellType(CellType.Numeric); cell6.SetCellValue(shopRefunds[i].Quantity);
                ICell cell7 = row.CreateCell(7); cell7.SetCellType(CellType.Numeric); cell7.SetCellValue(shopRefunds[i].Turnover);
                ICell cell8 = row.CreateCell(8); cell8.SetCellType(CellType.Numeric); cell8.SetCellValue(shopRefunds[i].Refund);
                ICell cell9 = row.CreateCell(9); cell9.SetCellType(CellType.String); cell9.SetCellValue(shopRefunds[i].Sources);
                ICell cell10 = row.CreateCell(10); cell10.SetCellType(CellType.Numeric); cell10.SetCellValue(shopRefunds[i].Fee);
                ICell cell11 = row.CreateCell(11); cell11.SetCellType(CellType.Numeric); cell11.SetCellValue(shopRefunds[i].Alliance);
                ICell cell12 = row.CreateCell(12); cell12.SetCellType(CellType.Numeric); cell12.SetCellValue(shopRefunds[i].Cashback);


                ICell cell13 = row.CreateCell(13);
                ICellStyle style = workbook.CreateCellStyle();
                style.DataFormat =  workbook.CreateDataFormat().GetFormat("yyyy-MM-dd HH:mm:ss");
                cell13.CellStyle = style;
                cell13.SetCellValue(shopRefunds[i].RefundTime);

                

                ICell cell14 = row.CreateCell(14); cell14.SetCellType(CellType.String); cell14.SetCellValue(shopRefunds[i].RefundReason);
                ICell cell15 = row.CreateCell(15); cell15.SetCellType(CellType.String); cell15.SetCellValue(shopRefunds[i].DebitOperation);
                ICell cell16 = row.CreateCell(16); cell16.SetCellType(CellType.Numeric); cell16.SetCellValue(Math.Round(shopRefunds[i].Deduction, 2));
                ICell cell17 = row.CreateCell(17); cell17.SetCellType(CellType.String); cell17.SetCellValue(shopRefunds[i].RefundOperation);
                ICell cell18 = row.CreateCell(18); cell18.SetCellType(CellType.String); cell18.SetCellValue(shopRefunds[i].RefundId);
                ICell cell19 = row.CreateCell(19); cell19.SetCellType(CellType.String); cell19.SetCellValue(shopRefunds[i].Trade);
            }
            IRow totalRow = sheet.CreateRow(shopRefunds.Length+2);
            ICell c0 = totalRow.CreateCell(15); c0.SetCellType(CellType.String); c0.SetCellValue("总额:");
            ICell c1 = totalRow.CreateCell(16);
            c1.SetCellType(CellType.Numeric);
            c1.SetCellFormula($"SUBTOTAL(9, Q2:Q{shopRefunds.Length+1})");

            using (FileStream fs =new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                workbook.Write(fs);
                Log($"保存 {fileName}");
            }
        }
        public static IList<ShopLend> ReadShopLend(string fileName)
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
                            to.Lend = d2;

                            double d3;
                            double.TryParse(row.GetCell(8)?.ToString(), out d3);
                            to.Fee = d3;

                            double d4;
                            double.TryParse(row.GetCell(9)?.ToString(), out d4);
                            to.Affiliate = d4;

                            double d5;
                            double.TryParse(row.GetCell(10)?.ToString(), out d5);
                            to.Cashback = d5;


                            DateTime t1;
                            DateTime.TryParseExact(row.GetCell(11)?.ToString(), "yyyy-MM-dd HH:mm:ss", new CultureInfo("zh-cn"), DateTimeStyles.None, out t1);
                            to.SettlementTime = t1;

                            to.OrderId = row.GetCell(12)?.ToString();
                            to.FileName = ss[c];
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
                            string v1 = row.GetCell(10)?.ToString();
                            v1 = v1.Replace("CN￥ ", "");
                            double.TryParse(v1, out d1);
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
            string[] files = ReadSortFiles(fileName);
            for (int i = 0; i < files.Length; i++)
            {
                using (FileStream fs = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                {
                    Log($"读取 {files[i]}");
                    IWorkbook workbook = WorkbookFactory.Create(fs);
                    var sheet = workbook.GetSheetAt(0);
                    if (sheet != null)
                    {
                        for (int j = 0; j <= sheet.LastRowNum; j++)
                        {
                            var row = sheet.GetRow(j);

                            TotalOrder to = new TotalOrder();
                            to.StoreName = row.GetCell(0)?.ToString();
                            // 跳过表头
                            if (to.StoreName!=null && to.StoreName.Contains("店铺"))
                            {
                                continue;
                            }


                            to.Operator = row.GetCell(1)?.ToString();
                            to.ClientId = row.GetCell(2)?.ToString();
                            to.OrderId = row.GetCell(4)?.ToString();
                            to.OrderStatus = row.GetCell(5)?.ToString();

                            string time = row.GetCell(7)?.ToString();
                            DateTime dateTime = new DateTime();
                            DateTime.TryParseExact(time, "yyyy/MM/dd hh:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out dateTime);
                            to.OrderTime = dateTime;

                            double cost;
                            double.TryParse(row.GetCell(13)?.ToString(), out cost);
                            to.Cost = cost;

                            to.OrderPrice = row.GetCell(14)?.ToString();
                            to.TradeId = row.GetCell(17)?.ToString().Trim().Replace("\r", "").Replace("\n", "");


                            double amount = -1.1;
                            double.TryParse(row.GetCell(18)?.ToString().Trim().Replace("RMB", "").Replace("R", "").Replace("$", ""), out amount);
                            to.DeductionAmount = amount;

                            if((!string.IsNullOrWhiteSpace(to.TradeId) && amount == -1.1) ||
                                (string.IsNullOrWhiteSpace(to.TradeId) && amount != -1.1))
                            {
                                Message += $"{to.OrderId} Trade error.\r\n";
                            }
                            os.Add(to);
                        }

                    }
                }
            }

            return os;
        }

        public static IList<Shop> ReadShopInfo(string fileName)
        {
            List<Shop> os = new List<Shop>();
            string[] files = ReadSortFiles(fileName);
            for (int i = 0; i < files.Length; i++)
            {
                using (FileStream fs = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                {
                    Log($"读取 {files[i]}");
                    IWorkbook workbook = WorkbookFactory.Create(fs);
                    var sheet = workbook.GetSheetAt(0);
                    if (sheet == null)
                    {
                        continue;
                    }

                    for (int j = 1; j <= sheet.LastRowNum; j++)
                    {
                        var row = sheet.GetRow(j);

                        Shop o = new Shop();
                        o.CompanyNumber = row.GetCell(0)?.ToString();
                        o.ClientId = row.GetCell(1)?.ToString();
                        o.CompanyName= row.GetCell(2)?.ToString();
                        o.Nick = row.GetCell(3)?.ToString();
                        o.CN = row.GetCell(4)?.ToString();
                        o.Category = row.GetCell(5)?.ToString();

                        os.Add(o);
                    }
                }
            }
            return os;
        }
        #endregion

        #region Collect

        public static List<Shop> AllShops = new List<Shop>();
        public static List<ShopRecord> ShopRecords = new List<ShopRecord>();
        public static List<TotalOrder> TotalOrders = new List<TotalOrder>();
        public static List<TotalPurchase> TotalPurchases = new List<TotalPurchase>();


        private static List<Shop> SelectShop(List<Shop> shops, string prefix)
        {
            List<Shop> ss = new List<Shop>();
            if (!string.IsNullOrWhiteSpace(prefix))
            {

                ss = AllShops.Where((s) =>
                {
                    string str = $"{s.CompanyNumber}{s.CN}";
                    string str1 = $"{prefix}";
                    if (str.Length >= str1.Length)
                    {
                        return str.Contains(str1);
                    }
                    else
                    {
                        return str1.Contains(str);
                    }

                }).ToList();
                if (ss.Count() > 0)
                {
                    return ss;
                }
            }
            return shops;
        }
        public static void Collect(string rawDir = "", string dataDir ="", string shopDirPrefix = "")
        {
            string shopRecordFileName = Path.Combine(rawDir, "店铺记录.xlsx");
            string orderRecordFileName = Path.Combine(rawDir, "订单总表.xlsx");
            string usPruchasRecordFileName = Path.Combine(rawDir, "美国采购单.xlsx");
            string brPruchasRecordFileName = Path.Combine(rawDir, "巴西采购单.xlsx");

            if(TotalOrders.Count == 0)
            {
                TotalOrders = ReadTotalOrder(orderRecordFileName).ToList();
            }
            if(TotalPurchases.Count == 0)
            {
                IList<TotalPurchase> totalPurchases1 = TotalPurchase(1, brPruchasRecordFileName);
                IList<TotalPurchase> totalPurchases2 = TotalPurchase(2, usPruchasRecordFileName);
                TotalPurchases.AddRange(totalPurchases1);
                TotalPurchases.AddRange(totalPurchases2);
            }
            
            if(AllShops.Count == 0)
            {
                AllShops = ReadShopInfo(shopRecordFileName).ToList();
            }


            List<Shop> ss = SelectShop(AllShops, shopDirPrefix);
            
            foreach (var shop in ss)
            {
                string[] ds = Directory.GetDirectories(dataDir);
                ds = ds.Where(d => Path.GetFileName(d).StartsWith($"{shop.CompanyNumber}{shop.CN}")).ToArray();
                if (ds.Length == 1)
                {
                    IList<ShopOrder> orders = ReadShopOrder(Path.Combine(ds[0], "订单.xlsx")).OrderBy(o => o.OrderTime).ToList();
                    IList<ShopLend> lendings = ReadShopLend(Path.Combine(ds[0], "放款.xlsx")).OrderBy(o => o.SettlementTime).ToList();
                    IList<ShopRefund> refunds = ReadShopRefund(Path.Combine(ds[0], "退款.xlsx")).OrderBy(o => o.RefundTime).ToList();

                    ShopRecords.Add(new ShopRecord() { Shop = shop, ShopOrderList = orders, ShopLendList = lendings, ShopRefundList = refunds });
                }

            }
            // CheckData();
        }

        #endregion
        public static bool CheckData()
        {
            bool ret = true;
            // 订单总表
            /// 非采购单 促销单 ： 订单号 交易号 扣款金额不完整。
            string[] clientIds = Array.ConvertAll(AllShops.ToArray(), s => s.ClientId);
            foreach (var item in TotalOrders.Where(t => clientIds.Contains(t.ClientId?.Trim()) && !string.IsNullOrWhiteSpace(t.ClientId)))
            {
                if (!string.IsNullOrWhiteSpace(item.OrderId) && string.IsNullOrWhiteSpace(item.TradeId) || item.DeductionAmount < 0)
                {
                    if (!item.OrderStatus.Equals("采购单") && !item.OrderStatus.Equals("促销单"))
                    {
                        Log($"{item.StoreName} {item.OrderId} {item.TradeId} {item.DeductionAmount}");
                        ret = false;
                    }
                }
            }
            return ret;
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
