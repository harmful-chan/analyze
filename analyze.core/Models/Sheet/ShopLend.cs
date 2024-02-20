using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Models.Sheet
{
    public class ShopLend
    {

        public string ProductId { get; set; }
        public string ProdectName { get; set; }
        public string SUK { get; set; }
        public string ProductCode { get; set; }
        public double Quantity { get; set; }
        public string ProductImage { get; set; }
        public double Turnover { get; set; }
        public double Lend { get; set; }
        public double Fee { get; set; }
        public double Affiliate { get; set; }
        public double Cashback { get; set; }
        public DateTime SettlementTime { get; set; }
        public string OrderId { get; set; }
        public string FileName { get; set; }

        public double Cost { get; set; }
        public double Profit { get; set; }
        public double Rate { get; set; }
    }
}
