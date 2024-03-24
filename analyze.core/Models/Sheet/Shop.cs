using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Models.Sheet
{
    public class Shop
    {
        public string CompanyNumber { get; set; }
        public string CompanyName { get; set; }
        public string Nick { get; set; }
        public string CN { get; set; }
        public string ClientId { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }

        public static Shop Convert(string filename) 
        { 
            filename = Path.GetFileNameWithoutExtension(filename);
            int index = filename.IndexOf("cn");
            Shop shop = new Shop();
            shop.CompanyNumber = filename.Substring(0, index);
            shop.CN = filename.Substring(index, 17);
            shop.CompanyName = filename.Substring(index + 17, filename.Length - 2 - 17 -index);
            shop.Nick = filename.Substring(filename.Length - 2, 2);

            return shop;
        }
    }
}
