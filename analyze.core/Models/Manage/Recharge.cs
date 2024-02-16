using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace analyze.core.Models.Manage
{
    public class Recharge
    {
        [JsonProperty(PropertyName = "ba_code")]
        public string TradeId { get; set; }
        [JsonProperty(PropertyName = "pn_real_amount")]
        public double Amount { get; set; }
        [JsonProperty(PropertyName = "pn_update_time")]
        public DateTime PatmentTime { get; set; }
        [JsonProperty(PropertyName = "pn_note")]
        public string Mark { get; set; }
        [JsonProperty(PropertyName = "company_name")]
        public string CompanyName { get; set; }
        [JsonProperty(PropertyName = "customer_code")]
        public string ClientId { get; set; }


    }
}
