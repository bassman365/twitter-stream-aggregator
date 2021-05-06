using Newtonsoft.Json;
using System.Collections.Generic;

namespace TwitterStreamApi.Models.TweetModels
{
    public class TweetEntity
    {
        public IEnumerable<TweetHashtag> Hashtags { get; set; } = new List<TweetHashtag>();

        [JsonProperty("urls")]
        public IEnumerable<TweetUrl> TweetUrls { get; set; } = new List<TweetUrl>();
    }
}
