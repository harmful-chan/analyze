using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyze.Models.Manage
{
    public class DebitRecord
    {
        [JsonProperty(PropertyName = "arn_id")]
        public int RecordId { get; set; }

        [JsonProperty(PropertyName = "transaction_no")]
        public string TradeId { get; set; }

        [JsonProperty(PropertyName = "arn_amount")]
        public double Cost { get; set; }

        [JsonProperty(PropertyName = "arn_finish_time")]
        public string CreateTime { get; set; }

        [JsonProperty(PropertyName = "cc_code")]
        public string ClientId { get; set; }

        [JsonProperty(PropertyName = "cu_name_en")]
        public string ClientName { get; set; }

    }
}
