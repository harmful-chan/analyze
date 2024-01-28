using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.Models
{
    public class ShopOrder
    {
        public string FileName { get; set; }
        public string OrderId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderTime { get; set; }
        public DateTime PaymentTime { get; set; }
        public double OrderAmount { get; set; }

        public string TradeNumber { get; set; }
        public DateTime ShippingTime { get; set; }
        public DateTime ReceiptTime { get; set; }
        public string Country { get; set; }

    }
}
