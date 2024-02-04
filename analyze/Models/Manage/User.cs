using Newtonsoft.Json;

namespace analyze.Models.Manage
{
    public class User
    {
        [JsonProperty(PropertyName = "cc_code")]
        public string ClientId { get; set; }

        [JsonProperty(PropertyName = "cb_value")]
        public double Balance { get; set; }

        [JsonProperty(PropertyName = "company_name")]
        public string CompanyName { get; set; }

    }
}