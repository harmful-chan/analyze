using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.Options
{
    [Verb("collect", HelpText = "收集数据")]
    public class CollectOptions
    {
        [Option('s', "start-date", Required =true, HelpText = "开始日期")]
        public DateTime StartDate { get; set; }
    }
}
