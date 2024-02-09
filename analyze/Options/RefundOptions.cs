using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.Options
{
    [Verb("refund", HelpText = "退款数据")]
    public class RefundOptions
    {
        [Option('l', "list", Required = true, HelpText = "列出用户")]
        public bool IsList { get; set; }

        [Option('d', "dir-prefix", Required = false, HelpText = "目录前缀")]
        public IEnumerable<string> DirPrefix { get; set; }

        [Option('s', "start-date", Required = false, HelpText = "开始日期")]
        public DateTime StartDate { get; set; }
        
        [Option('e', "end-date", Required = false, HelpText = "结束日期")]
        public DateTime EndDate { get; set; }

        [Option('m', "moon", Required = false, HelpText = "月份")]
        public string Moon { get; set; }

    }
}
