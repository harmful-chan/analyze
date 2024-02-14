using System;

namespace analyze.core.Models.Daily
{
    public class Dispute
    {
        public string OrderId { get; set; }
        public DateTime OrderTime { get; set; }
        public string Buyer { get; set; }
        public DateTime DisputeTime { get; set; }
        public string Status { get; set; }
        public string LastTime { get; set; }
    }
}