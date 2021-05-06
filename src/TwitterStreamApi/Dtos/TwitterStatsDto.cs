using System.Collections.Generic;
using TwitterStreamApi.Models.TweetModels;

namespace TwitterStreamApi.Dtos
{
    public class TwitterStatsDto
    {
        public int Total { get; set; }

        public TweetAverages Averages { get; set; } = new TweetAverages();

        public IEnumerable<KeyValuePair<string, int>> TopEmojis { get; set; } = new Dictionary<string, int>();

        public decimal PercentContainingEmojis { get; set; }

        public IEnumerable<KeyValuePair<string, int>> TopHashtags { get; set; } = new Dictionary<string, int>();

        public decimal UrlPercent { get; set; }

        public decimal ImageUrlPercent { get; set; }

        public IEnumerable<KeyValuePair<string, int>> TopDomains { get; set; } = new Dictionary<string, int>();
    }
}
