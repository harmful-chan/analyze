using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace analyze.core.Options
{
    [Verb("daily", HelpText = "每日数据")]
    public class DailyOptions
    {
        [Option('f', "file-name", HelpText = "读取文件")]
        public IEnumerable<string> DailyFiles { get; set; }

        [Option("shop-info", HelpText = "店铺信息文件")]
        public string ShopInfoFileName { get; set; }


        [Option('d', "file-dir", HelpText = "每日数据文件目录")]
        public string FileDir { get; set; }

        [Option("br-purchase-filenmae", HelpText = "巴西采购数据路径")]
        public string BrPurchaseFilenmae { get; set; }

        [Option("list-opear", HelpText = "列出运营人员")]
        public bool IsListOpera { get; set; }

        [Option("list-company", HelpText = "列出公司人员")]
        public bool IsListCompany { get; set; }

        [Option("list-profit", HelpText = "列出利润")]
        public bool IsListProfit { get; set; }


        [Option("upload-order", HelpText = "上报订单数据")]
        public bool IsUploadOrder { get; set; }

        [Option("upload-info", HelpText = "上报公司数据")]
        public bool IsUploadProfit { get; set; }



    }
}
