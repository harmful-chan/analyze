using analyze.core.Clients;
using analyze.core.Models.Rola;
using analyze.core.Models.Sheet;
using analyze.core.Options;
using analyze.core.Outputs;
using CommandLine;
using NPOI.SS.Formula.Functions;
using PlaywrightSharp;
using System.Diagnostics;
using static NPOI.HSSF.Util.HSSFColor;

namespace analyze.core.test
{
    [TestFixture]
    public class AnalyzerTest
    {

        [SetUp]
        public void Setup()
        {

        }

        //[Test]
        //public void TestOrderLend([Values("order -a lend -r Z:\\数据采集 -l")] string args)
        //{
        //    OrderOptions o = new OrderOptions(); 
        //    Parser.Default.ParseArguments<OrderOptions>(args.Split(' ')).WithParsed((OrderOptions o1) =>{ o = o1;  });
        //
        //    DebugOutput output = new DebugOutput();
        //    Analyzer analyzer = new Analyzer();
        //    analyzer.Output = output;
        //    if (analyzer.SetRootDirectorie(o.RootDir))
        //    {
        //        DateTime s = DateTime.Parse("2023-07");
        //        for (int i = 0; (s = s.AddMonths(1)) < DateTime.Now.AddMonths(-1); i++)
        //        {
        //            analyzer.StartCollect("公司23cn1077984038qwgae百舸群创aa");
        //            ShopRecord[] shopRecords = analyzer.GetShopRecords("公司23cn1077984038qwgae百舸群创aa");
        //            foreach (var shopRecord in shopRecords)
        //            {
        //                analyzer.FillProfitForOneMonth(s.Year, s.Month, shopRecord);
        //                Analyzer.Profit profit = analyzer.StatisticalProfit(s.Year, s.Month, shopRecord);
        //                Debug.WriteLine($"公司23cn1077984038qwgae百舸群创aa {s.Year} {s.Month.ToString("D2")} Lend:{Math.Round(profit.Lend, 2)} Cost:{Math.Round(profit.Cost, 2)} Profit:{Math.Round(profit.Value, 2)} Rate:{Math.Round(profit.Rate, 2)}");
        //            }
        //        }
        //    }
        //}

        [Test]
        public void TestRola()
        {
            RolaClient rola = new RolaClient();
            Position p = rola.RolaCheck("GB", "England", "London");
            string result = rola.RolaRefresh("BushyNeville_1", p.Country, p.Region, p.City).Result;
            Position result1 = rola.GetPosition("gate8.rola.vip", 2024, "BushyNeville_1", "123").Result;
            Position result2 = rola.CheckIpPosition(result1?.IP).Result;
        }


        [Test]
        public void TestDaily([Values("daily -d D:\\我的坚果云\\数据采集\\每日数据\\2024年03月12日 --shop-info D:\\我的坚果云\\数据采集\\店铺信息.xlsx --listcompany --upload-order")] string args)
        {
            DailyOptions o = new DailyOptions();
            Parser.Default.ParseArguments<DailyOptions>(args.Split(' '))
                .WithParsed(
                (DailyOptions o1) =>
                {
                    o = o1;
                });

            o.FileDir = "D:\\我的坚果云\\数据采集\\每日数据\\2024年03月12日";
            o.ShopInfoFileName = "D:\\我的坚果云\\数据采集\\店铺信息.xlsx";
            o.IsListCompany = true;
            o.IsUploadProfit = true;
            DebugOutput output = new DebugOutput();
            Analyzer analyzer = new Analyzer();
            analyzer.Output = output;
            if (analyzer.SetRootDirectorie("D:\\我的坚果云\\数据采集"))
            {
                //analyzer.DailyRun(o);
            }
        }

        [Test]
        public async Task TestUrlAsync()
        {
            DebugOutput output = new DebugOutput();
            Analyzer analyzer = new Analyzer();
            //await analyzer.GetUrl();
        }


        [Test]
        public async Task TestChange()
        {

            //string dir1 = "D:\\我的坚果云\\数据采集\\每日数据\\2024年03月27日\\公司05广州市誉川防水建材有限公司aa_cn1076763119lzfae.xlsx";
            //
            //ShopFileInfo shopFileInfo1 = ShopFileInfo.Convert(dir1);
            //
            //
            //string dir2 = "D:\\我的坚果云\\数据采集\\每日数据\\2024年03月27日\\广州金猿宝传媒科技有限公司ab_20240201000000_20240229235959.xlsx";
            //
            //ShopFileInfo shopFileInfo2 = ShopFileInfo.Convert(dir2);
            //
            //
            //string dir3 = "D:\\我的坚果云\\数据采集\\每日数据\\2024年03月27日\\广州金猿宝传媒科技有限公司ab_20240201_20240229235959.xlsx";
            //
            //ShopFileInfo shopFileInfo3 = ShopFileInfo.Convert(dir3);
            //
            //string dir4 = "D:\\我的坚果云\\数据采集\\每日数据\\2024年03月27日\\广州金猿宝传媒科技有限公司ab_20240201.xlsx";
            //
            //ShopFileInfo shopFileInfo4 = ShopFileInfo.Convert(dir4);
            //
            //string dir5 = "D:\\我的坚果云\\数据采集\\每日数据\\2024年03月27日\\广州金猿宝传媒科技有限公司ab_202402.xlsx";
            //
            //ShopFileInfo shopFileInfo5 = ShopFileInfo.Convert(dir5);


            //string dirs = "D:\\我的坚果云\\数据采集\\利润统计\\2024年02月";
            //
            //foreach (string dir in Directory.GetDirectories(dirs))
            //{
            //    string[] strings = Directory.GetFiles(dir);
            //    foreach (string str in strings)
            //    {
            //        string name = Path.GetFileName(str);
            //        string path = Path.GetDirectoryName(str);
            //        string name1 = name.Substring(0, name.Length - 11);
            //        string name2 = name.Substring(name.Length - 11);
            //        // string[] splits = name.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            //        //if (splits.Length == 2)
            //        //{
            //        //    string name1 = splits[0];
            //        //    int i;
            //        //    if (int.TryParse(splits[1].Replace(".xlsx", ""), out i) && i < 4)
            //        //    {
            //        //        name1 += "_2024" + $"{i:00}.xlsx";
            //        //    }
            //        //    else
            //        //    {
            //        //        name1 += "_2023" + $"{i:00}.xlsx";
            //        //    }
            //        //
            //        //
            //        //    File.Move(str, Path.Combine(path, name1));
            //        //}
            //
            //        File.Move(str, Path.Combine(path, name1 + "_" + name2));
            //    }
            //}

        }

        [Test]
        public async Task TestChange1()
        {

            string[] dirs = Directory.GetDirectories("D:\\我的坚果云\\数据采集\\订单数据\\2024年03月");
            foreach (string dir in dirs)
            {
                string[] filename = Directory.GetFiles(dir);
                foreach (string file in filename)
                {
                    string name = Path.GetFileNameWithoutExtension(file);
                    string path = Path.GetDirectoryName(file);
                    string xlsx = Path.GetExtension(file);
                    string newname = name;
                    if (name.Contains('_'))
                    {
                        string n1 = name.Split('_')[0];
                        string n2 = name.Split('_')[1];
                        int i = 0;
                        ;
                        if (int.TryParse(n2.Substring(4, 2), out i) && i <= 4)
                        {
                            newname = $"{n1}_2024{i:00}";
                        }
                        else if (int.TryParse(n2.Substring(4, 2), out i) && i > 4)
                        {
                            newname = $"{n1}_2023{i:00}";
                        }
                    }

                    string newfilename = Path.Combine(path, newname + xlsx);
                    if (!File.Exists(newfilename))
                    {
                        File.Move(file, newfilename);
                    }
                }
            }

        }

        [Test]
        public async Task TestPost()
        {


            RolaClient rolaClient = new RolaClient();
            //KeyValuePair<string, string> result = rolaClient.CheckPost("fr", "44730").Result;


        }

        [Test]
        public void GetProfix()
        {
            DateTime start = DateTime.Parse("2024-01");
            DateTime end = DateTime.Parse("2024-04");

            // 希音
            //string[] arra = ["cn1083409115cclae", "cn1083409116oxeae", "cn1081658119xxsae", "cn1083481135nthae", "cn1083481136ignae", "cn1081971110vuyae"];
            // 炬马
            // string[] arra = ["cn1079617241kcsae", "cn1079617246mlwae", "cn1077602209gxcae", "cn1078224227wboae", "cn1077506308barae", "cn1079641273alaae"];

            // 炬皓
            string[] arra = ["cn1078344252curae", "cn1079785058nmlae", "cn1078368050eljae", "cn1077770069zgpae", "cn1079785064lsqae", "cn1077794059itdae"];

            Analyzer analyzer = new Analyzer();
            analyzer.Output = new DebugOutput();
            analyzer.SetRootDirectorie("D:\\我的坚果云\\数据采集");
            analyzer.LoadResources();
            analyzer.StartCollect(CollectTypes.Shop);



            Shop[] shops = analyzer.ShopCatalogs.Where(s => arra.Contains(s.CN)).ToArray();


            foreach (Shop shop in shops)  // 循环每个运营中的店铺
            {

                var cnShop = analyzer.ShopRecords.FirstOrDefault(sr => sr.Shop.CN.Equals(shop.CN));

                List<ShopLend> sureLend = new List<ShopLend>();
                for (DateTime i = start; i <= end; i = i.AddMonths(1))   // 循环选择的月份
                {
                    DateTime startTime = i.AddDays(1 - i.Day).Date;
                    DateTime endTime = i.AddDays(1 - i.Day).Date.AddMonths(1).AddSeconds(-1);

                    if (cnShop == null || cnShop.ShopRefundList == null || cnShop.ShopRefundList.Count() <= 0)
                    {
                        continue;
                    }

                    analyzer.FillProfitForOneMonth(i.Year, i.Month, cnShop);
                    ShopLend[] shopLends = analyzer.GetOneMonthLend(i.Year, i.Month, cnShop);
                    sureLend.AddRange(shopLends);

                }

                string dir = Path.Combine(analyzer.NewestProfitDirectory, $"{cnShop.Shop.CompanyNumber}{cnShop.Shop.CompanyName}");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                if(sureLend.Count() > 0)
                {
                    DateTime sTime = sureLend.First().SettlementTime;
                    DateTime eTime = sureLend.Last().SettlementTime;
                    string filename = Path.Combine(dir, $"{shop.CompanyName}{shop.Nick}_{shop.CN}_{sTime.ToString("yyyyMMddHHmmss")}_{eTime.ToString("yyyyMMddHHmmss")}.xlsx");
                    analyzer.SaveProfit(filename, sureLend.ToArray());
                }

            }

        }

        [Test]
        public void SaveProfix()
        {

        }
    }
}