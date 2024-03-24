using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.core.Models.Manage
{

    

    public class MarkOrder
    {
        [JsonProperty(PropertyName = "order_code")]
        public string OrderId { get; set; }

        [JsonProperty(PropertyName = "tracking_number")]
        public string  TrackingNumber { get; set; }

        [JsonProperty(PropertyName = "tracking_number_old")]
        public string TrackingNumberOld { get; set; }

        [JsonProperty(PropertyName = "platform_carrier_code")]
        public string Carrier { get; set; }

        [JsonProperty(PropertyName = "platform_carrier_code_old")]
        public string CarrierOld { get; set; }


        [JsonProperty(PropertyName = "sync_status_text")]
        public string StatusText { get; set; }

        [JsonProperty(PropertyName = "tracking_tips")]
        public string Tips { get; set; }

        public int Step
        {
            get 
            {
                int ret = 0;
                if (!string.IsNullOrWhiteSpace(OrderId))
                {
                    ret = 1;
                    if( !string.IsNullOrEmpty(TrackingNumber) && TrackingNumber.Equals(TrackingNumberOld))
                    {
                        ret = 2;
                    }

                    if (!string.IsNullOrEmpty(TrackingNumber) && !TrackingNumber.Equals(TrackingNumberOld) && !string.IsNullOrWhiteSpace(Tips))
                    {
                        ret = 3;
                    }

                }
                return ret;
            }
        }
    }
}
