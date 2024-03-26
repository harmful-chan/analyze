using analyze.core.Models.Daily;
using analyze.core.Models.Sheet;
using analyze.core.Outputs;
using IPinfo.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XWPF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;                
using System.Text;
using System.Text.RegularExpressions;
using ICell = NPOI.SS.UserModel.ICell;

namespace analyze.core.Clients
{
    public class SheetClient
    {

        public static string Message { get; set; }

        private IOutput _output;

        public IOutput Output
        {
            get { return _output; }
            set { _output = value; }
        }


        public SheetClient()
        {
            
        }

        #region XLSX操作

        public IList<PurchaseOrder> ReadPurchaseOrder(int type, string fileName)
        {

            List<PurchaseOrder> os = new List<PurchaseOrder>();
            string[] files = ReadSortFiles(fileName);  // 读取相同文件
            for (int i = 0; i < files.Length; i++)
            {
                using (FileStream fs = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = WorkbookFactory.Create(fs);
                    _output.Write($"读取 {files[i]}");
                    var sheet = workbook.GetSheetAt(0);
                    if (sheet != null)
                    {
                        for (int j = 0; j < sheet.LastRowNum; j++)
                        {
                            var row = sheet.GetRow(j);
                            if (row == null)
                            {
                                continue;
                            }

                            PurchaseOrder to = new PurchaseOrder();
                            
                            // 去除表格 us A1空 B1=操作人




                            if (row != null && type == 1)  // 类型1巴西单
                            {
                                string A1 = row.GetCell(0)?.ToString();
                                string B1 = row.GetCell(1)?.ToString();
                                string C1 = row.GetCell(2)?.ToString();
                                string D1 = row.GetCell(3)?.ToString();
                                string E1 = row.GetCell(4)?.ToString();
                                // 去除表格 br A1=序号 B1=提单日期
                                if (string.IsNullOrWhiteSpace(C1) || "下单时间".Equals(A1) && "剩余".Equals(B1))
                                {
                                    continue;
                                }




                                
                                to.Country = "BR";
                                if (row.GetCell(0)?.CellType == CellType.Numeric && HSSFDateUtil.IsCellDateFormatted(row.GetCell(0)))
                                {
                                    to.OrderDate = (DateTime)(row.GetCell(0)?.DateCellValue);
                                }

                                if (row.GetCell(1)?.CellType == CellType.Numeric)
                                {
                                    
                                    to.OrderOverdue = (int)row.GetCell(1).NumericCellValue;
                                }
                                else
                                {
                                    to.OrderOverdue = -1;
                                }

                                
                                if (row.GetCell(3)?.CellType == CellType.Numeric && HSSFDateUtil.IsCellDateFormatted(row.GetCell(3)))
                                {
                                    to.SubmissionDate = (DateTime)(row.GetCell(3)?.DateCellValue);
                                }


                                if (row.GetCell(4)?.CellType == CellType.Numeric)
                                {

                                    to.SubmissionOverdue = (int)row.GetCell(4).NumericCellValue;
                                }
                                else
                                {
                                    to.SubmissionOverdue = -1;
                                }

                                to.OrderId = row.GetCell(8)?.ToString();
                                to.Status = row.GetCell(24)?.ToString();
                                string v = row.GetCell(23)?.ToString();
                                to.IsUpdate = string.IsNullOrWhiteSpace(v) ? false : "已更新".Equals(v);
                                to.Buyer = row.GetCell(18)?.ToString();

                                to.Index = C1;
                                os.Add(to);
                            }
                            else if (row != null && type == 2)  // 类型2美国单
                            {
                                string A1 = row.GetCell(0)?.ToString();
                                string B1 = row.GetCell(1)?.ToString();
                                if (string.IsNullOrWhiteSpace(A1) && "操作人".Equals(B1) || "序号".Equals(A1) && "日期".Equals(B1))
                                {
                                    continue;
                                }

                                to.Country = "US";
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

        public IList<ShopRefund> ReadShopRefund(string fileName)
        {
            List<ShopRefund> os = new List<ShopRefund>();
            string[] ss = ReadSortFiles(fileName);
            for (int c = 0; c < ss.Length; c++)
            {
                using (FileStream fs = new FileStream(ss[c], FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = WorkbookFactory.Create(fs);
                    _output.Write($"读取 {ss[c]}");
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

        public void SaveShopLend(string fileName, ShopLend[] shopLends, string demoFileName, bool overwrite)
        {
            File.Copy(demoFileName, fileName, overwrite);

            IWorkbook workbook;
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                workbook = WorkbookFactory.Create(fs);
            }
            #region 设置数据
                ICellStyle style = workbook.CreateCellStyle();
                style.Alignment = HorizontalAlignment.Center;
                style.VerticalAlignment = VerticalAlignment.Center;
                ISheet sheet = workbook.GetSheetAt(0);
                for (int i = 0; i < shopLends.Length; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);



                    ICell cell0 = row.CreateCell(0); cell0.SetCellType(CellType.Numeric); cell0.CellStyle = style; cell0.SetCellValue(i + 1);
                    ICell cell1 = row.CreateCell(1); cell1.SetCellType(CellType.String); cell1.CellStyle = style; cell1.SetCellValue(shopLends[i].ProductId);
                    ICell cell2 = row.CreateCell(2); cell2.SetCellType(CellType.String); cell2.SetCellValue(shopLends[i].ProdectName);
                    ICell cell3 = row.CreateCell(3); cell3.SetCellType(CellType.String); cell3.SetCellValue(shopLends[i].SUK);
                    ICell cell4 = row.CreateCell(4); cell4.SetCellType(CellType.String); cell4.SetCellValue(shopLends[i].ProductCode);
                    ICell cell5 = row.CreateCell(5); cell5.SetCellType(CellType.Numeric); cell5.CellStyle = style; cell5.SetCellValue(shopLends[i].Quantity);
                    ICell cell6 = row.CreateCell(6); cell6.SetCellType(CellType.String); cell6.SetCellValue(shopLends[i].ProductImage);
                    ICell cell7 = row.CreateCell(7); cell7.SetCellType(CellType.Numeric); cell7.CellStyle = style; cell7.SetCellValue(shopLends[i].Turnover);
                    ICell cell8 = row.CreateCell(8); cell8.SetCellType(CellType.Numeric); cell8.CellStyle = style; cell8.SetCellValue(shopLends[i].Lend);
                    ICell cell9 = row.CreateCell(9); cell9.SetCellType(CellType.Numeric); cell9.CellStyle = style; cell9.SetCellValue(shopLends[i].Cost);
                    ICell cell10 = row.CreateCell(10); cell10.SetCellType(CellType.Numeric); cell10.CellStyle = style; cell10.SetCellValue(shopLends[i].Profit);
                    ICell cell11 = row.CreateCell(11); cell11.SetCellType(CellType.Numeric); cell11.CellStyle = style; cell11.SetCellValue(shopLends[i].Rate);

                    ICell cell12 = row.CreateCell(12);
                    ICellStyle style1 = workbook.CreateCellStyle();
                    style1.DataFormat = workbook.CreateDataFormat().GetFormat("yyyy-MM-dd HH:mm:ss");
                    style1.Alignment = HorizontalAlignment.Center;
                    style1.VerticalAlignment = VerticalAlignment.Center;
                    cell12.CellStyle = style1;
                    cell12.SetCellValue(shopLends[i].SettlementTime);

                    ICell cell13 = row.CreateCell(13); cell13.SetCellType(CellType.String); cell13.CellStyle = style; cell13.SetCellValue(shopLends[i].OrderId);

                }
                IRow totalRow = sheet.CreateRow(shopLends.Length + 2);
                ICell c0 = totalRow.CreateCell(7);
                c0.SetCellType(CellType.String);
                c0.CellStyle = style;
                c0.SetCellValue("总额:");
                ICell c1 = totalRow.CreateCell(8);
                c1.SetCellType(CellType.Numeric);
                c1.CellStyle = style;
                c1.SetCellFormula($"SUBTOTAL(9, I2:I{shopLends.Length + 1})");
                ICell c2 = totalRow.CreateCell(9);
                c2.SetCellType(CellType.Numeric);
                c2.CellStyle = style;
                c2.SetCellFormula($"SUBTOTAL(9, J2:J{shopLends.Length + 1})");
                ICell c3 = totalRow.CreateCell(10);
                c3.SetCellType(CellType.Numeric);
                c3.CellStyle = style;
                c3.SetCellFormula($"SUBTOTAL(9, K2:K{shopLends.Length + 1})");

                double lend = 0.0;
                double profix = 0.0;
                double cost = 0.0;
                foreach (var item in shopLends)
                {
                    lend += item.Lend;
                    cost += item.Cost;
                    profix += item.Profit;
                }

                ICell c4 = totalRow.CreateCell(11);
                c4.SetCellType(CellType.Numeric);
                c4.CellStyle = style;
                c4.SetCellValue(Math.Round(profix / cost, 2));
            #endregion
            FileStream fileStream = File.Create(fileName);
            workbook.Write(fileStream);
            _output.WriteLine($"保存 {fileName}");
            workbook.Close();
            fileStream.Close();

        }

        public void SaveShopRefund(string fileName, ShopRefund[] shopRefunds, string demoFileName, bool overwrite)
        {
            File.Copy(demoFileName, fileName, overwrite);

            IWorkbook workbook;

            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                workbook = WorkbookFactory.Create(fs);
            }

            #region 处理数据
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
                style.DataFormat = workbook.CreateDataFormat().GetFormat("yyyy-MM-dd HH:mm:ss");
                cell13.CellStyle = style;
                cell13.SetCellValue(shopRefunds[i].RefundTime);



                ICell cell14 = row.CreateCell(14); cell14.SetCellType(CellType.String); cell14.SetCellValue(shopRefunds[i].RefundReason);
                ICell cell15 = row.CreateCell(15); cell15.SetCellType(CellType.String); cell15.SetCellValue(shopRefunds[i].DebitOperation ?? "");
                ICell cell16 = row.CreateCell(16); cell16.SetCellType(CellType.Numeric); cell16.SetCellValue(Math.Round(shopRefunds[i].Deduction, 2));
                ICell cell17 = row.CreateCell(17); cell17.SetCellType(CellType.String); cell17.SetCellValue(shopRefunds[i].RefundOperation ?? "");
                ICell cell18 = row.CreateCell(18); cell18.SetCellType(CellType.String); cell18.SetCellValue(shopRefunds[i].RefundId ?? "");
                ICell cell19 = row.CreateCell(19); cell19.SetCellType(CellType.String); cell19.SetCellValue(shopRefunds[i].Trade ?? "");
            }
            IRow totalRow = sheet.CreateRow(shopRefunds.Length + 2);
            ICell c0 = totalRow.CreateCell(15); c0.SetCellType(CellType.String); c0.SetCellValue("总额:");
            ICell c1 = totalRow.CreateCell(16);
            c1.SetCellType(CellType.Numeric);
            c1.SetCellFormula($"SUBTOTAL(9, Q2:Q{shopRefunds.Length + 1})");
            #endregion

            FileStream fileStream = File.Create(fileName);
            workbook.Write(fileStream);
            _output.WriteLine($"保存 {fileName}");
            workbook.Close();
            fileStream.Close();

        }

        public IList<ShopLend> ReadShopLend(string fileName)
        {
            List<ShopLend> os = new List<ShopLend>();
            string[] ss = ReadSortFiles(fileName);
            for (int c = 0; c < ss.Length; c++)
            {
                using (FileStream fs = new FileStream(ss[c], FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = WorkbookFactory.Create(fs);
                    _output.Write($"读取 {ss[c]}");
                    var sheet = workbook.GetSheetAt(0);

                    if (sheet != null && sheet.LastRowNum > 0)
                    {
                        for (int i = 1; i < sheet.LastRowNum + 1; i++)
                        {
                            var row = sheet.GetRow(i);

                            ShopLend to = new ShopLend();

                            to.ProductId = row.GetCell(0)?.ToString();
                            to.ProdectName = row.GetCell(1)?.ToString();
                            to.SUK = row.GetCell(2)?.ToString();
                            to.ProductCode = row.GetCell(3)?.ToString();
                            to.Quantity = row.GetCell(4).NumericCellValue;
                            to.ProductImage = row.GetCell(5)?.ToString();
                            to.Turnover = row.GetCell(6).NumericCellValue ;
                            to.Lend = row.GetCell(7).NumericCellValue;
                            to.Fee = row.GetCell(8).NumericCellValue;
                            to.Affiliate = row.GetCell(9).NumericCellValue;
                            to.Cashback = row.GetCell(10).NumericCellValue;
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

        public IList<ShopOrder> ReadShopOrder(string fileName)
        {
            List<ShopOrder> os = new List<ShopOrder>();
            string[] ss = ReadSortFiles(fileName);
            for (int c = 0; c < ss.Length; c++)
            {
                using (FileStream fs = new FileStream(ss[c], FileMode.Open, FileAccess.Read))
                {
                    _output.Write($"读取 {ss[c]}");
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

        public IList<SubmitOrder> ReadSubmitOrder(string fileName)
        {
            List<SubmitOrder> os = new List<SubmitOrder>();
            string[] files = ReadSortFiles(fileName);
            for (int i = 0; i < files.Length; i++)
            {
                using (FileStream fs = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                {
                    _output.Write($"读取 {files[i]}");
                    IWorkbook workbook = WorkbookFactory.Create(fs);
                    var sheet = workbook.GetSheetAt(0);
                    if (sheet != null)
                    {
                        for (int j = 0; j <= sheet.LastRowNum; j++)
                        {
                            var row = sheet.GetRow(j);

                            SubmitOrder to = new SubmitOrder();
                            to.StoreName = row.GetCell(0)?.ToString();
                            // 跳过表头
                            if (to.StoreName != null && to.StoreName.Contains("店铺"))
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

                            if (!string.IsNullOrWhiteSpace(to.TradeId) && amount == -1.1 ||
                                string.IsNullOrWhiteSpace(to.TradeId) && amount != -1.1)
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

        public IList<Shop> ReadShopCatalog(string fileName)
        {
            List<Shop> os = new List<Shop>();
            string[] files = ReadSortFiles(fileName);
            for (int i = 0; i < files.Length; i++)
            {
                using (FileStream fs = new FileStream(files[i], FileMode.Open, FileAccess.Read))
                {
                    _output.Write($"读取 {files[i]}");
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
                        o.CompanyName = row.GetCell(2)?.ToString();
                        o.Nick = row.GetCell(3)?.ToString();
                        o.CN = row.GetCell(4)?.ToString();
                        o.Category = row.GetCell(5)?.ToString();
                        o.Status = row.GetCell(7)?.ToString();
                        os.Add(o);
                    }
                }
            }
            return os;
        }

        public KeyValuePair<string, double> SplitAmount(string raw)
        {
            if (!string.IsNullOrWhiteSpace(raw))
            {
                string[] str = new string[2];
                int index = new Regex(@"\d").Match(raw).Index;
                str[0] = raw.Substring(0, index).Replace("(", ""); // 符号
                str[1] = raw.Substring(index).Replace(")", "");
                int v = str[1].LastIndexOf(',');
                if (str[1].LastIndexOf(',') == str[1].Length - 3 || str[1].LastIndexOf(',') == str[1].Length - 2)
                {
                    str[1] = str[1].Replace(',', '.');
                }
                double d1;
                double.TryParse(str[1], out d1);

                return new KeyValuePair<string, double>(str[0], d1);
            }
            else
            {
                return new KeyValuePair<string, double>("", 0.00);
            }
        }

        private int[] GetHeaderLine(ISheet sheet, string header, string[] headerList)
        {
            int start = -1, end = -1;
            // 搜索开始序号
            for (int i = 0; i < sheet.LastRowNum; i++)
            {
                ICell cell = sheet.GetRow(i).GetCell(0);
                if(cell!=null &&  cell.CellType == CellType.String)
                {
                    if ( header.Equals(cell.StringCellValue) && headerList.Contains(cell.StringCellValue))
                    {
                        start = i;
                        break;
                    }
                }
            }
            // 搜索结束序号，搜不到就用表格最后一行
            if(start != -1)
            {
                ArrayList arrayList = new ArrayList(headerList);
                arrayList.Remove(header);
                for (int i = start + 1; i < sheet.LastRowNum; i++)
                {
                    ICell cell = sheet.GetRow(i).GetCell(0);
                    if (cell != null && cell.CellType == CellType.String)
                    {

                        if (arrayList.Contains(cell.StringCellValue))
                        {
                            end = i;
                            break;
                        }
                    }
                }

                if(end == -1)
                {
                    end = sheet.LastRowNum;
                }
            }


            return start == -1 ? null: new int[] { start, end-1 };
        } 

        private string[] headerFirst = new string[] { "时间", "订单号", "订单信息", "订单详情", "日期" };
        public DailyDetail ReadDaily(string filename)
        {
            DailyDetail daily = new DailyDetail();
            List<NotShip> notShips = new List<NotShip>();
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                _output.Write($"读取 {filename}");
                IWorkbook workbook = WorkbookFactory.Create(fs);
                var sheet = workbook.GetSheetAt(0);

                // 公司数据
                string date = Path.GetFileName(Path.GetDirectoryName(filename));
                daily.CollectionDate = DateTime.Parse(date);
                daily.CompanyNumber = Path.GetFileNameWithoutExtension(filename).Substring(0, 4);
                daily.Company = sheet.GetRow(0).GetCell(0).ToString();
                daily.Nick = sheet.GetRow(0).GetCell(1).ToString();
                daily.Operator = sheet.GetRow(1).GetCell(0).ToString();
                daily.CN = Shop.Convert(filename).CN;
                // 在售数据
                string u = sheet.GetRow(1).GetCell(1).ToString();
                daily.InSrockNumber = 0;
                if (u.Contains("("))
                {
                    int i = u.IndexOf("(");
                    u = u.Substring(u.IndexOf("(") + 1, u.Length - i - 2);
                    int d1 = 0;
                    int.TryParse(u, out d1);
                    daily.InSrockNumber = d1;
                }

                string m = sheet.GetRow(1).GetCell(2).ToString();
                daily.ReviewNumber = 0;
                if (m.Contains("("))
                {
                    int i = m.IndexOf("(");
                    m = m.Substring(m.IndexOf("(") + 1, m.Length - i - 2);
                    int d1 = 0;
                    int.TryParse(m, out d1);
                    daily.ReviewNumber = d1;
                }

                string d = sheet.GetRow(1).GetCell(3).ToString();
                daily.RemovedNumber = 0;
                if (d.Contains("("))
                {
                    int i = d.IndexOf("(");
                    d = d.Substring(d.IndexOf("(") + 1, d.Length - i - 2);
                    int d1 = 0;
                    int.TryParse(d, out d1);
                    daily.RemovedNumber = d1;
                }

                // 店铺数据
                var r0 = sheet.GetRow(2);
                daily.IM24 = r0.GetCell(0).CellType == CellType.Numeric ? r0.GetCell(0).NumericCellValue : 0.0;
                daily.NotSell = r0.GetCell(1).CellType == CellType.Numeric ? r0.GetCell(1).NumericCellValue : 0.0;
                daily.WrongGoods = r0.GetCell(2).CellType == CellType.Numeric ? r0.GetCell(2).NumericCellValue : 0.0;
                daily.Dispute = r0.GetCell(3).CellType == CellType.Numeric ? r0.GetCell(3).NumericCellValue : 0.0;
                daily.GoodReviews = r0.GetCell(4).CellType == CellType.Numeric ? r0.GetCell(4).NumericCellValue : 0.0;
                daily.Collect72 = r0.GetCell(5).CellType == CellType.Numeric ? r0.GetCell(5).NumericCellValue : 0.0;

                // 资金数据
                var r1 = sheet.GetRow(3);
                daily.Lend = SplitAmount(r1.GetCell(0).ToString()).Value;
                daily.Freeze = SplitAmount(r1.GetCell(1).ToString()).Value;
                daily.OnWay = SplitAmount(r1.GetCell(2).ToString()).Value;
                daily.Arrears = SplitAmount(r1.GetCell(3).ToString()).Value;

                // 直通车数据
                daily.Consume = SplitAmount(r1.GetCell(4)?.ToString()).Value;
                daily.Promotion = SplitAmount(r1.GetCell(5)?.ToString()).Value;

                // 提现数据
                var ints = GetHeaderLine(sheet, "时间", headerFirst); 
                if(ints != null)
                {
                    List<Withdraw> withdraws = new List<Withdraw>();
                    for (int i = ints[0]; i < ints[1]; i++) 
                    {
                        var row = sheet.GetRow(i);
                        string v = row.GetCell(0).ToString();
                        if (v.Contains("时间") || v.Contains("暂无数据"))
                        {
                            continue;
                        }
                        Withdraw withdraw = new Withdraw();
                        withdraw.WithdrawTime = row.GetCell(0).DateCellValue;
                        withdraw.Amount = double.Parse(row.GetCell(4).ToString().Split(' ')[0]);
                        withdraws.Add(withdraw);
                    }
                    daily.Withdraws = withdraws.ToArray();
                }
                else
                {
                    daily.Withdraws = new Withdraw[0];
                }
                ////在途订单
                ints = GetHeaderLine(sheet, "订单号", headerFirst);
                if (ints != null)
                {
                    List<OnWayOrder> OnWayOrders = new List<OnWayOrder>();
                    for (int j = ints[0]; j <= ints[1]; j++)
                    {

                        var row = sheet.GetRow(j);
                        OnWayOrder onWayOrder = new OnWayOrder();
                    
                        if (row.GetCell(0).CellType == CellType.String)
                        {
                            var v = row.GetCell(0).StringCellValue;
                            if (v.Contains("订单号"))
                            {
                                continue;
                            }
                        }
                    
                        onWayOrder.OrderId = row.GetCell(0).NumericCellValue.ToString();
                        onWayOrder.PaymentTime = row.GetCell(1).DateCellValue;
                        onWayOrder.ShippingTime = row.GetCell(2) == null ? default : row.GetCell(2).DateCellValue;
                        onWayOrder.ReceiptTime = row.GetCell(3) == null ? default : row.GetCell(3).DateCellValue;
                        onWayOrder.Amount = SplitAmount(row.GetCell(4).ToString()).Value;
                        onWayOrder.Reason = row.GetCell(9).ToString();
                        OnWayOrders.Add(onWayOrder);
                    }
                    daily.OnWayOrders = OnWayOrders.ToArray();
                }
                else
                {
                    daily.OnWayOrders = new OnWayOrder[0];
                }

                //// 纠纷订单

                ints = GetHeaderLine(sheet, "订单信息", headerFirst);
                if (ints != null)
                {
                    List<Dispute> disputes = new List<Dispute>();
                    for (int j = ints[0]; j <= ints[1]; j++)
                    {
                        var row = sheet.GetRow(j);
                        string v = row.GetCell(0).ToString();
                        if (v.Equals("订单信息") || v.Equals("暂无数据"))
                        {
                            continue;
                        }

                    
                        Dispute dispute = new Dispute();
                        string c0 = row.GetCell(0).ToString();
                        // 订单
                        /// 订单号
                        dispute.OrderId = Regex.Match(c0, @"订单信息\s*(\d+)").Groups[1].Value;
                        /// 下单时间
                        string time = Regex.Match(c0, @"下单时间\s*([\d-]+\s[\d:]+)").Groups[1].Value;
                        DateTime dateTime;
                        DateTime.TryParseExact(time, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out dateTime);
                        dispute.OrderTime = dateTime;
                        dispute.DisputeTime = row.GetCell(4).DateCellValue;
                    
                        var c5 = row.GetCell(5)?.ToString();
                        dispute.Status = c5.Split('\n')[0];
                        if (c5.Contains("剩余："))
                        {
                            int index = c5.IndexOf("剩余：");
                            dispute.LastTime = c5.Substring(index + 4);
                        }
                    
                        disputes.Add(dispute);
                    }
                    daily.DisputeOrders = disputes.ToArray();
                }
                else
                {
                    daily.DisputeOrders = new Dispute[0];
                }

                //// 未发货订单
                ////for (int j = rowNumber; j <= sheet.LastRowNum; j++)
                ////{
                ////    rowNumber = j;
                ////    NotShip notShip = new NotShip();
                ////    var row = sheet.GetRow(j);
                ////    string v = row.GetCell(0).ToString();
                ////    if (row.GetCell(0).ToString().Equals("订单详情"))
                ////    {
                ////        break;
                ////    }
                ////    // 订单时间
                ////    DateTime? dateCellValue = row.GetCell(0)?.DateCellValue;
                ////    notShip.OrderTime = (DateTime)dateCellValue;
                ////
                ////    // 金额
                ////    string amount = row.GetCell(1)?.ToString();
                ////    KeyValuePair<string, double> kv = SplitAmount(amount);
                ////    notShip.Symbol = kv.Key;
                ////    notShip.Amount = kv.Value;
                ////
                ////    // 剩余天数
                ////    string day = row.GetCell(2)?.ToString();
                ////    int d2;
                ////    int.TryParse(day, out d2);
                ////    notShip.LastDay = d2;
                ////
                ////    notShips.Add(notShip);
                ////}

                // 订单详情
                ints = GetHeaderLine(sheet, "订单详情", headerFirst);
                if (ints != null)
                {
                    for (int j = ints[0]; j <= ints[1]; j++)
                    {
                        var row = sheet.GetRow(j);
                        OrderDetail od = new OrderDetail();
                        var c0 = row.GetCell(0)?.ToString();
                        if (c0.Equals("订单详情"))
                        {
                            continue;
                        }
                    
                        // 订单
                        /// 订单号
                        od.OrderId = Regex.Match(c0, @"订单号:\s*(\d+)").Groups[1].Value;
                        /// 下单时间
                        string time = Regex.Match(c0, @"下单时间:\s*([\d-]+\s[\d:]+)").Groups[1].Value;
                        DateTime dateTime;
                        DateTime.TryParseExact(time, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out dateTime);
                        od.OrderTime = dateTime;
                    
                        // 标题
                        var c2 = row.GetCell(2)?.ToString();
                        string[] raw = c2.Split('\n');
                        od.Title = raw.ElementAtOrDefault(0);    // 标题
                        od.IsAssess = raw.Contains("上网时效考核订单");
                        for (int i = 0; i < raw.Length; i++)
                        {
                            if (raw[i].Equals("商品属性:") && raw[i+1].IndexOf("United States", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                od.ShipsFrom = OrderShipsFromTypes.UnitedStates;
                            }
                            if (raw[i].Equals("商品属性:") && raw[i + 1].IndexOf("brazil", StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                od.ShipsFrom = OrderShipsFromTypes.Brazil;
                            }
                        }



                        // 数量 
                        var c3 = row.GetCell(3)?.ToString();
                        string n = string.IsNullOrWhiteSpace(c3) ? "1" :  c3.Substring(1);
                        int d1;
                        int.TryParse(n, out d1);
                        od.Quantity = d1;


                        //售后
                        var c4 = row.GetCell(4)?.ToString();
                        od.After = c4;
                    
                        // 金额
                        var c5 = row.GetCell(5)?.ToString();
                        od.RMB = SplitAmount(c5.Split('\n')[0]).Value;
                        if (c5.Split('\n').Length > 1)
                        {
                            KeyValuePair<string, double> kv = SplitAmount(c5.Split('\n')[1]);
                            od.Symbol = kv.Key;
                            od.Amount = kv.Value;
                        }
                        else
                        {
                            od.Amount = 0.0;
                        }
                    
                    
                        // 状态
                        var c6 = row.GetCell(6)?.ToString();
                        od.Status = c6.Split('\n')[0];
                        if (c6.Contains("剩余时间："))
                        {
                            int index = c6.IndexOf("剩余时间：");
                            od.LastTime = c6.Substring(index + 6);
                        }
                        orderDetails.Add(od);
                    }
                    
                    daily.NotShips = notShips.ToArray();
                    daily.OrderDetails = orderDetails.GroupBy(x => x.OrderId).Select(y => y.First()).ToArray();
                }
                else
                {
                    daily.OrderDetails = new OrderDetail[0];
                }

                // 直通车消耗
                ints = GetHeaderLine(sheet, "日期", headerFirst);
                if (ints != null)
                {
                    List<PromotionDetail> list = new List<PromotionDetail>();

                    for (int j = ints[0]; j <= ints[1]; j++)
                    {
                        var row = sheet.GetRow(j);
                        PromotionDetail pd = new PromotionDetail();
                        var c0 = row.GetCell(0)?.ToString();
                        if (c0.Equals("日期"))
                        {
                            continue;
                        }

                        pd.Time = row.GetCell(0).DateCellValue;
                        pd.Expenses = row.GetCell(1).NumericCellValue;
                        pd.InCome = row.GetCell(2).NumericCellValue;

                        list.Add(pd);
                    }

                    daily.PromotionDetails = list.ToArray();
                }
                else
                {
                    daily.PromotionDetails = new PromotionDetail[0];
                }
            }

            return daily;
        }

     
        #endregion



        public static string[] ReadSortFiles(string fileName)
        {
            string fullPath = Path.GetFullPath(fileName);
            string[] fs = Directory.GetFiles(Path.GetDirectoryName(fullPath));

            fs = fs.Where(s => 
            Path.GetFileName(s).StartsWith(Path.GetFileNameWithoutExtension(fileName))
            && !Path.GetFileName(s).StartsWith("~")).ToArray();
            Array.Sort(fs);
            return fs;
        }
    }
}
