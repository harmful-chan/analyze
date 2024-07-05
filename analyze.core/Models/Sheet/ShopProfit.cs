using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Models.Sheet
{

    public class ShopProfit
    {
        public string CompanyName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Platfrom { get; set; }
        public string CN { get; set; }
        public string Category { get; set; }
        public string Nick { get; set; }
        public string Payee { get; set; }
        public double Cost { get; set; }
        public double Profix { get; set; }
        public double Rate { get; set; }
        public double Withdraw { get; set; }
        public double Mark { get; set; }
    }
}
