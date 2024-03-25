using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Models.Purchase
{
    public class PurchaseProgressUnit
    {
        public int Total { get; set; }
        public int Processing { get; set;}
        public int Solved { get; set; }
        public int Cancel { get; set; }
        public int Cut { get; set; }
        public int Pending { get; set; }
    }


    public class PurchaseProgress
    {
        public DateTime Date { get; set; }
        public Dictionary<string, PurchaseProgressUnit> Purchase { get; set; }
        
    }
}
