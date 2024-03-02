namespace analyze.core.Outputs
{
    public interface IOutput
    {
        public void Write(string str, bool online = true);
        public void WriteLine(string str = "");
        public void Append(string str);
        public void AddRow(params object[] objs);
        public void Show(params string[] headers);
    }
}