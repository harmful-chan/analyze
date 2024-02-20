using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Options
{
    [Verb("order", HelpText = "订单数据数据")]
    public class OrderOptions
    {
        [Option('l', "list", Required = false, HelpText = "列出用户")]
        public bool IsList { get; set; }

        [Option('a', "action", Required = false, HelpText = "执行行为")]
        public string action { get; set; }


        //[Option('s', "start-date", Required = false, HelpText = "开始日期")]
        public DateTime StartDate { get; set; }
        
        //[Option('e', "end-date", Required = false, HelpText = "结束日期")]
        public DateTime EndDate { get; set; }

        [Option('y', "yesr", Required = false, HelpText = "年份")]
        public int Year { get; set; } = -1;

        [Option('m', "moon", Required = false, HelpText = "月份")]
        public int Moon { get; set; } = -1;

        [Option('o', "output", Required = false, HelpText = "输出文件名")]
        public string OutputFile { get; set; }

        [Option('r', "raw", Required = true, HelpText = "数据总表文件夹路径")]
        public string RowDir { get; set; }

        [Option('d', "raw", Required = true, HelpText = "店铺文件夹路径")]
        public string DataDir { get; set; }

        [Option('p', "prefix-dir", Required = false, HelpText = "目录前缀")]
        public IEnumerable<string> DirPrefix { get; set; }


    }
}
