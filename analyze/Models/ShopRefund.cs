using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.Models
{
    public class ShopRefund
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public double Turnover { get; set; }
        public double RefundAmount { get; set; }
        public DateTime RefundTime { get; set; }
    }
}
