using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlexHelper.Yts.Entities
{
    public class YtsMovie
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("imdb_code")]
        public string ImdbCode { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("title_long")]
        public string TitleLong { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        public int FuzzyDistance { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, ImdbCode: {1}, Title: {2}", Id, ImdbCode, Title);
        }
    }
}
