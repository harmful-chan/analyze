using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Models.Sheet
{
    public class PurchaseOrder
    {
        public string Country { get; set; }
        public string OrderId { get; set; }
        public string Status { get; set; }
        public bool  IsUpdate { get; set; }
        public string Buyer { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderOverdue { get; set; }
        public DateTime SubmissionDate { get; set; }
        public int SubmissionOverdue { get; set; }
        public string Index { get; set; }

    }
}
