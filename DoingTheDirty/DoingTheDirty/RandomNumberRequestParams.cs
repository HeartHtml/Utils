using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DoingTheDirty
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
