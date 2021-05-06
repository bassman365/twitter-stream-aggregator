using Newtonsoft.Json;

namespace TwitterStreamApi.Models.TweetModels
{
    public class Tweet
    {
        [JsonProperty("data")]
        public TweetData Data { get; set; } = new TweetData();
    }
}
