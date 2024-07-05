using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Options
{
    [Verb("manage", HelpText = "后台管理")]
    public class ManageOptions
    {
        [Option('u', "user", Required = false,  HelpText = "列出用户当天订单")]
        public IEnumerable<string> ClientId { get; set; }

        [Option('l', "list", Required = false, HelpText = "列出用户")]
        public bool IsList { get; set; }

        [Option('s', "session", Required = false, HelpText = "管理员session")]
        public string Session { get; set; }

        [Option('f', "file-name", Required = false, HelpText = "订单文件路径")]
        public string FileName { get; set; }

        [Option("deduction", Required = false, HelpText = "申请扣款")]
        public IEnumerable<string> DeductionId { get; set; }


    }
}
