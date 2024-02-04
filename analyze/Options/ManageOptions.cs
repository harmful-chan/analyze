using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.Options
{
    [Verb("manage", HelpText = "后台管理")]
    public class ManageOptions
    {
        [Option('u', "user", Required = false,  HelpText = "列出用户当天订单")]
        public IEnumerable<string> ClientId { get; set; }


        [Option('l', "list", Required = false, HelpText = "列出用户")]
        public bool IsList { get; set; }
    }
}
