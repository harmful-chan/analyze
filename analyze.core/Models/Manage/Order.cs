using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace analyze.core.Models.Manage
{
    public class Order
    {
        public int Status { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "order_code")]
        public string OrderId { get; set; }

        [JsonProperty(PropertyName = "distribution_costs")]
        public double Cost { get; set; }
    }
}
