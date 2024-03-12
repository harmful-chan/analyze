using analyze.core.Outputs;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.test
{
    public class DebugOutput : IOutput
    {
        List<object[]> strList = new List<object[]>();
        public void Append(string str)
        {
            strList.Add([str]);
        }

        public void AddRow(params object[] objs)
        {
            strList.Add(objs);
        }

        public void Show(params string[] headers)
        {
            ConsoleTable consoleTables = new ConsoleTable(headers);
            foreach (var item in strList)
            {
                consoleTables.AddRow(item);
            }
            consoleTables.Write(Format.Minimal);
        }

        public string Write(string str, bool online)
        {
            Debug.WriteLine(str);
            return str;
        }

        public void WriteLine(string str = "")
        {
            Debug.WriteLine(str);
        }

    }
}
