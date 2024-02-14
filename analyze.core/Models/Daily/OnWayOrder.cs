using System;
using System.Collections.Generic;
using System.Text;

namespace analyze.core.Models.Daily
{
    public class OnWayOrder
    {
        public string OrderId { get; set; }
        public DateTime PaymentTime { get; set; }
        public DateTime ShippingTime { get; set; }
        public DateTime ReceiptTime { get; set; }
        public double Amount { get; set; }
        public string Reason { get; set; }
    }
}
