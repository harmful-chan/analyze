using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Models.Purchase
{



    public class PurchaseProgress
    {
        public DateTime Date { get; set; }
        public Dictionary<string, PurchaseProgressUnit> Purchase { get; set; }
        
    }
}
