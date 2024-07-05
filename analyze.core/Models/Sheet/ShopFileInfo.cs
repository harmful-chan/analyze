using NPOI.HPSF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Models.Sheet
{
    public class DateRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

    }
    public class ShopFileInfo
    {
        public string CompanyNumber { get; set; }
        public string CompanyName { get; set; }
        public string Nick { get; set; }
        public string CN { get; set; }
        public string FileName { get; set; }

        public DateRange CollectRange { get; set; }

        public static ShopFileInfo Convert(string filename)
        {
            ShopFileInfo shop = new ShopFileInfo();
            shop.FileName = filename;
            string name = Path.GetFileNameWithoutExtension(filename);
            string fixName = name;
            // _开头的是可选配置
            if (name.Contains('_'))
            {
                string[] splits = name.Split('_');
                fixName = splits[0];
                shop.CN = splits.FirstOrDefault(x => x.StartsWith("cn"));

                List<string> list = splits.ToList();
                list.Remove(fixName);
                list.Remove(shop.CN);
                splits = list.ToArray();

                if(splits.Length > 0)
                {
                    string[] formats = [ "yyyy", "yyyyMM", "yyyyMMdd", "yyyyMMddHH", "yyyyMMddHHmm", "yyyyMMddHHmmss"];
                    DateRange dateRange = new DateRange();
                    if (splits.Length == 1)
                    {

                        
                        DateTime start = default;
                        bool flag = DateTime.TryParseExact(splits[0], formats, new CultureInfo("en-US"), DateTimeStyles.None, out start);
                        if (flag)
                        {
                            dateRange.Start = start.AddDays(1 - start.Day).Date;
                            dateRange.End = start.AddDays(1 - start.Day).Date.AddMonths(1).AddSeconds(-1);
                        }
                    }

                    if(splits.Length == 2)
                    {
                        DateTime start = default;
                        DateTime end = default;
                        bool flag1 = DateTime.TryParseExact(splits[0], formats, new CultureInfo("en-US"), DateTimeStyles.None, out start);
                        bool flag2 = DateTime.TryParseExact(splits[1], formats, new CultureInfo("en-US"), DateTimeStyles.None, out end); ;
                        if (flag1) dateRange.Start = start;
                        if (flag2) dateRange.End = end;

                    }
                    shop.CollectRange = dateRange;

                }
                else
                {
                    shop.CollectRange = null;
                }

            }

            shop.CompanyNumber = fixName.Substring(0, 4);
            shop.CompanyName = fixName.Substring(4, fixName.Length-6);
            shop.Nick = fixName.Substring(fixName.Length - 2, 2);



            return shop;
        }
    }
}
