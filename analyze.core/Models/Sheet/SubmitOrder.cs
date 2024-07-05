using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Models.Sheet
{
    public class SubmitOrder
    {
        public string StoreName { get; set; } // A
        public string Operator { get; set; } // B
        public string ClientId { get; set; }
        public string OrderStatus { get; set; }
        public string OrderId { get; set; }  // E
        public DateTime OrderTime { get; set; }

        public double Cost { get; set; }
        public string OrderPrice { get; set; }
        public string TradeId { get; set; }
        public double DeductionAmount { get; set; }

    }
}
