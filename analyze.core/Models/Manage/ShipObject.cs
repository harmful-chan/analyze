using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Models.Manage
{
    public class ShipObject
    {
        public string ClientID { get; set; }
        public string OrderID { get; set; }
        public string TradeID { get; set; }

        public string Carrier { get; set; }
        public string ShipNumber { get; set; }

    }
}
