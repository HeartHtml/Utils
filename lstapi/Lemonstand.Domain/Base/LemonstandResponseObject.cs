using Lemonstand.Domain.Entities;
using Lemonstand.Domain.Interfaces;
using Newtonsoft.Json;

namespace Lemonstand.Domain.Base
{
    public class LemonstandResponseObject
    {
        [JsonProperty("meta")]
        public Meta MetaObject { get; set; }

        [JsonProperty("data")]
        public ILemonstandDataObject DataObject { get; set; }

        public LemonstandResponseObject(Discount data)
        {
            DataObject = data;
        }
    }
}
