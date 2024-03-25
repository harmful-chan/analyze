using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Models.Manage
{
    public enum ShipTypes
    {
        Deduct,
        DeductAndShip,
        Ship,
        ShipAndDeclare,
        Declare,
        DeductAndShipAndDeclare,


    }
    public class ShipObject
    {
        public int Id { get; set; }
        public string ClientID { get; set; }
        public string OrderID { get; set; }

        public string TrackingNumber { get; set; }

        public string TrackingNumberOld { get; set; }

        public string Carrier { get; set; }

        public string CarrierOld { get; set; }

        public ShipTypes Step { get; set; }

        public string TradeID { get; set; }
        public string DeductionAmount { get; set; }

        public bool IsDeduction { get; set; } = false;

        public bool IsShipped { get; set; }



    }
}
