using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace analyze.core.Models.Manage
{
    public class PurchaseStatus
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

    }

    public class Tag
    {
        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

        [JsonProperty(PropertyName = "tags")]
        public string[] Tags { get; set; }

        [JsonProperty(PropertyName = "purchaseStatu")]
        public PurchaseStatus[] PurchaseStatus { get; set; }
    }


    public class OrderHeader
    {
        [JsonProperty(PropertyName = "1")]
        public Tag T1 { get; set; }

        [JsonProperty(PropertyName = "2")]
        public Tag T2 { get; set; }

        [JsonProperty(PropertyName = "3")]
        public Tag T3 { get; set; }

        [JsonProperty(PropertyName = "4")]
        public Tag T4 { get; set; }

        [JsonProperty(PropertyName = "5")]
        public Tag T5 { get; set; }

        [JsonProperty(PropertyName = "6")]
        public Tag T6 { get; set; }

        [JsonProperty(PropertyName = "7")]
        public Tag T7 { get; set; }

        [JsonProperty(PropertyName = "8")]
        public Tag T8 { get; set; }

        [JsonProperty(PropertyName = "9")]
        public Tag T9 { get; set; }

    }
}
