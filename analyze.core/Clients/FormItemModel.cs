using System.IO;

namespace analyze.core.Clients
{
    internal class FormItemModel
    {
        public FormItemModel()
        {
        }

        public string Key { get; set; }
        public FileStream FileContent { get; set; }
        public string FileName { get; set; }
        public string Value { get; set; }
        public bool IsFile { get; internal set; }
    }
}