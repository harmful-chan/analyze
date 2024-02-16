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


        [Option('d', "file-dir", HelpText = "每日数据文件目录")]
        public string FileDir { get; set; }

        [Option("br-purchase-filenmae", HelpText = "巴西采购数据路径")]
        public string BrPurchaseFilenmae { get; set; }


        [Option('l', "list", HelpText = "列出数据")]
        public bool IsList { get; set; }

        [Option("list-opear", HelpText = "列出运营人员")]
        public bool IsListOpera { get; set; }

        [Option("list-company", HelpText = "列出公司人员")]
        public bool IsListCompany { get; set; }


        [Option('u', "upload", HelpText = "上报数据")]
        public bool IsUpload { get; set; }
    }
}
