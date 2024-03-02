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
            Analyzer analyzer = new Analyzer(output);

            if(analyzer.SetRootDirectorie(o.RootDir))
            {
                DateTime s = DateTime.Parse("2023-07");
                for (int i = 0; (s = s.AddMonths(1)) < DateTime.Now.AddMonths(-1); i++)
                {
                    Models.Sheet.ShopLend[] shopLends = analyzer.GetShopLendProfitForOneMonth(s.Year, s.Month, "公司23cn1077984038qwgae百舸群创aa");
                    Analyzer.Profit profit = analyzer.StatisticalProfit(shopLends);
                    Debug.WriteLine($"公司23cn1077984038qwgae百舸群创aa {s.Year} {s.Month.ToString("D2")} Lend:{Math.Round(profit.Lend, 2)} Cost:{Math.Round(profit.Cost, 2)} Profit:{Math.Round(profit.Value, 2)} Rate:{Math.Round(profit.Rate, 2)}");
                }
            }
        }
    }
}