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

        public string Write(string str, bool online)
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
            return str;
        }

        public string WriteLine(string str = "")
        {
            Console.WriteLine(str);
            return str;
        }

        void IOutput.WriteLine(string str)
        {
            throw new NotImplementedException();
        }
    }
}
