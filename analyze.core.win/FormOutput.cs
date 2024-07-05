using analyze.core.Outputs;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.win
{
    internal class FormOutput : TextWriter, IOutput
    {
        private TextBox _log;
        private Control _control;
        public FormOutput(Control control, TextBox log)
        {
            _log = log;
            _control = control;
        }

        public override Encoding Encoding => Encoding.Unicode;


        List<object[]> objects = new List<object[]>();
        public void AddRow(params object[] objs)
        {
            objects.Add(objs);
        }

        public void Append(string str)
        {
            throw new NotImplementedException();
        }

        public void Show(params string[] headers)
        {
            foreach (var item in headers)
            {
                Write($"{item} ", false);
            }
            Write("\r\n",false);
            foreach (var i in objects)
            {
                foreach(var j in i)
                {
                    Write($"{j} ", false);
                }
                Write("\r\n", false);
            }
            objects = new List<object[]>();
        }

        public string Write(string str, bool online = true)
        {
            _control.BeginInvoke(new Action(() =>
            {
                if (online && !string.IsNullOrEmpty(_log.Text) )
                {
                    int lineIndex = _log.GetLineFromCharIndex(_log.SelectionStart);
                    int lineFirstIndex = _log.GetFirstCharIndexFromLine(lineIndex);
                    _log.SelectionStart = lineFirstIndex;
                    _log.SelectionLength = _log.Lines[lineIndex].Length;
                    _log.SelectedText = str;
                }
                else
                {
                    _log.Text += str;
                }
                _log.ScrollToCaret();
            }));

            return str;
        }

        public  void WriteLine(string str = "")
        {
            _control.BeginInvoke(new Action(() =>
            {
                _log.AppendText(str+"\r\n");
                _log.ScrollToCaret();
            }));
        }
    }
}
