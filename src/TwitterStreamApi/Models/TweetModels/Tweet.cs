using Newtonsoft.Json;

namespace TwitterStreamApi.Models.TweetModels
{
    public class Tweet
    {
        public TweetData Data { get; set; } = new TweetData();
    }
}
