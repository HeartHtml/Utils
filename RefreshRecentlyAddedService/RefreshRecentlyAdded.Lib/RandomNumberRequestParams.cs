using Newtonsoft.Json;

namespace RefreshRecentlyAddedService
{
    [JsonObject("params")]
    public class RandomNumberRequestParams
    {
        [JsonProperty("apiKey")]
        public string ApiKey
        { get; set; }

        [JsonProperty("n")]
        public int N { get; set; }

        [JsonProperty("min")]
        public int Min { get; set; }

        [JsonProperty("max")]
        public int Max { get; set; }

        [JsonProperty("replacement")]
        public bool Replacement { get; set; }

        [JsonProperty("base")]
        public int Base { get; set; }
    }
}
