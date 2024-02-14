using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Options
{
    [Verb("collect", HelpText = "退款数据")]
    public class CollectOptions
    {
        [Option('l', "list", HelpText = "列出用户列表")]
        public DateTime StartDate { get; set; }
    }
}
