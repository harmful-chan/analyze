using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Outputs
{
    public class ConsoleOutput : IOutput
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

        public void Write(string str, bool online)
        {
            if (online)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(str);
            }
            else
            {
                Console.Write(str);
            }

        }

        public void WriteLine(string str = "")
        {
            Console.WriteLine(str);
        }
    }
}
