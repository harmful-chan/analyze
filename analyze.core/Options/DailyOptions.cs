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


        [Option('d', "file-dir", HelpText = "文件目录")]
        public string FileDir { get; set; }


        [Option('l', "list", HelpText = "列出数据")]
        public bool IsList { get; set; }
    }
}
