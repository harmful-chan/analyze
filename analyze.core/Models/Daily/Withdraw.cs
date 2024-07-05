using System;
using System.Collections.Generic;
using System.Text;

namespace analyze.core.Models.Daily
{
    public class Withdraw
    {
        public int ID { get; set; }
        public DateTime WithdrawTime { get; set; }
        public double Amount { get; set; }
    }
}
