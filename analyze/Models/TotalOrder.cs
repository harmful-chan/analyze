using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.Models
{
    public class TotalOrder
    {
        public string StoreName { get; set; } // A
        public string Operator { get; set; } // B
        public string OrderStatus { get; set; }
        public string OrderId { get; set; }  // E
        public DateTime OrderTime { get; set; }

        public double Cost { get; set; }
        public string  OrderPrice { get; set; }
        public string TradeId { get; set; }
        public double DeductionAmount { get; set; }

    }
}
