using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lemonstand.Domain.Entities
{
    public class Meta
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
