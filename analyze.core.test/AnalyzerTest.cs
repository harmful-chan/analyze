using analyze.core.Models.Sheet;
using analyze.core.Options;
using analyze.core.Outputs;
using CommandLine;
using NPOI.SS.Formula.Functions;
using System.Diagnostics;

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
        public void TestOrderLend([Values("order -a lend -r Z:\\数据采集 -l")] string args)
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
                    analyzer.CollectShopRecords("公司23cn1077984038qwgae百舸群创aa");
                    ShopRecord[] shopRecords = analyzer.GetShopRecords("公司23cn1077984038qwgae百舸群创aa");
                    foreach (var shopRecord in shopRecords)
                    {
                        analyzer.FillProfitForOneMonth(s.Year, s.Month, shopRecord);
                        Analyzer.Profit profit = analyzer.StatisticalProfit(s.Year, s.Month, shopRecord);
                        Debug.WriteLine($"公司23cn1077984038qwgae百舸群创aa {s.Year} {s.Month.ToString("D2")} Lend:{Math.Round(profit.Lend, 2)} Cost:{Math.Round(profit.Cost, 2)} Profit:{Math.Round(profit.Value, 2)} Rate:{Math.Round(profit.Rate, 2)}");
                    }
                }
            }
        }
    }
}