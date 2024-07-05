using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Options
{
    [Verb("purchase", HelpText = "采购进度")]
    public class PurchaseOptions
    {
        [Option('l', "list", HelpText = "列出采购进度")]
        public bool IsListPurchaseProgress { get; set; }

        [Option('b', "buyer", HelpText = "采购手")]
        public string Buyer { get; set; }

        [Option("br-purchase-filenmae", HelpText = "巴西采购数据路径")]
        public string BrPurchaseFilename { get; set; }
    }
}
