using System;

namespace analyze.core.Models.Daily
{
    public class NotShip
    {
        public DateTime OrderTime { get; set; }
        public string Symbol { get; set; }
        public double Amount { get; set; }
        public int LastDay { get; set; }
    }
}