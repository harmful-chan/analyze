using System;

namespace analyze.core.Models.Daily
{
    public class OrderDetail
    {
        public string OrderId { get; set; }
        public DateTime OrderTime { get; set; }
        public string Remark { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public double RMB { get; set; }
        public string Symbol { get; set; }
        public double Amount { get; set; }
        public string After { get; set; }
        public string  Status { get; set; }
        public string LastTime { get; set; }



    }
}