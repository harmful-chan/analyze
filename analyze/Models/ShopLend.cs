using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.Models
{
    public class ShopLend
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public double Turnover { get; set; }
        public double RebateAmount { get; set; }
        public double TradeAmount { get; set; }
        public double AffiliateAmount { get; set; }
        public double Cashback { get; set; }
        public DateTime SettlementTime { get; set; }

    }
}
