using Newtonsoft.Json;

namespace RefreshRecentlyAddedService
{
    public class RandomNumberRequest
    {
        [JsonProperty("jsonrpc")]
        public string JsonRpc
        {
            get; set;
        }

        [JsonProperty("method")]
        public string Method

        {
            get; set;
        }

        [JsonProperty("params")]
        public RandomNumberRequestParams Params { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
