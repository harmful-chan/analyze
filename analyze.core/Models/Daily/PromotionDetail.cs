using System;

namespace analyze.core.Models.Daily
{
    public class PromotionDetail
    {
        public int ID { get; set; }
        public DateTime Time { get; set; }

        public double Expenses { get; set; }

        public double InCome { get; set; }
    }
}