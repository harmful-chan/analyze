using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Models.Sheet
{
    public class ShopLend
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public double Turnover { get; set; }
        public double Lend { get; set; }
        public double Fee { get; set; }
        public double Affiliate { get; set; }
        public double Cashback { get; set; }
        public DateTime SettlementTime { get; set; }
        public string FileName { get; set; }
    }
}
