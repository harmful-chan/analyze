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

        [Test]
        public void TestOrderLend([Values("order -a lend -r Z:\\���ݲɼ� -l")] string args)
        {
            OrderOptions o = new OrderOptions(); 
            Parser.Default.ParseArguments<OrderOptions>(args.Split(' ')).WithParsed((OrderOptions o1) =>{ o = o1;  });

            DebugOutput output = new DebugOutput();
            Analyzer analyzer = new Analyzer();
            analyzer.Output = output;
            if (analyzer.SetRootDirectorie(o.RootDir))
            {
                DateTime s = DateTime.Parse("2023-07");
                for (int i = 0; (s = s.AddMonths(1)) < DateTime.Now.AddMonths(-1); i++)
                {
                    analyzer.CollectShopRecords("��˾23cn1077984038qwgae����Ⱥ��aa");
                    ShopRecord[] shopRecords = analyzer.GetShopRecords("��˾23cn1077984038qwgae����Ⱥ��aa");
                    foreach (var shopRecord in shopRecords)
                    {
                        analyzer.FillProfitForOneMonth(s.Year, s.Month, shopRecord);
                        Analyzer.Profit profit = analyzer.StatisticalProfit(s.Year, s.Month, shopRecord);
                        Debug.WriteLine($"��˾23cn1077984038qwgae����Ⱥ��aa {s.Year} {s.Month.ToString("D2")} Lend:{Math.Round(profit.Lend, 2)} Cost:{Math.Round(profit.Cost, 2)} Profit:{Math.Round(profit.Value, 2)} Rate:{Math.Round(profit.Rate, 2)}");
                    }
                }
            }
        }

        [Test]
        public void TestRola()
        {
            RolaClient rola = new  RolaClient();
            Position p = rola.RolaCheck("GB", "England", "London");
            string result = rola.RolaRefresh("BushyNeville_1", p.Country, p.Region, p.City).Result;
            Position result1 = rola.GetPosition("gate8.rola.vip", 2024, "BushyNeville_1", "123").Result;
            Position result2 = rola.CheckIpPosition(result1?.IP).Result;
        }


        [Test]
        public void TestDaily([Values("daily -d D:\\�ҵļ����\\���ݲɼ�\\ÿ������\\2024��03��12�� --shop-info D:\\�ҵļ����\\���ݲɼ�\\������Ϣ.xlsx --listcompany --upload-order")] string args)
        {
            DailyOptions o = new DailyOptions();
            Parser.Default.ParseArguments<DailyOptions>(args.Split(' '))
                .WithParsed(
                (DailyOptions o1) => 
                { 
                    o = o1; 
                });

            o.FileDir = "D:\\�ҵļ����\\���ݲɼ�\\ÿ������\\2024��03��12��";
            o.ShopInfoFileName = "D:\\�ҵļ����\\���ݲɼ�\\������Ϣ.xlsx";
            o.IsListCompany = true;
            o.IsUploadProfit = true;
            DebugOutput output = new DebugOutput();
            Analyzer analyzer = new Analyzer();
            analyzer.Output = output;
            if (analyzer.SetRootDirectorie("D:\\�ҵļ����\\���ݲɼ�"))
            {
                analyzer.DailyRun(o);
            }
        }

        [Test]
        public async Task TestUrlAsync()
        {
            DebugOutput output = new DebugOutput();
            Analyzer analyzer = new Analyzer();
            await analyzer.GetUrl();
        }
    }
}