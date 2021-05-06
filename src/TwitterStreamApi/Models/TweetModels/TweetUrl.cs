using Newtonsoft.Json;
using System.Collections.Generic;

namespace TwitterStreamApi.Models.TweetModels
{
    public class TweetUrl
    {
        public int Start { get; set; }

        public int End { get; set; }

        [JsonProperty("expanded_url")]
        public string? Url { get; set; }

        [JsonProperty("unwound_url")]
        public string? DestinationUrl { get; set; }
        public IEnumerable<TweetImage> Images { get; set; } = new List<TweetImage>();
    }
}
