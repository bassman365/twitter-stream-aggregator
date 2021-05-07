using System.Collections.Generic;
using TwitterStreamApi.Models.TweetModels;

namespace TwitterStreamApi.Dtos
{
    public class TwitterStatsDto
    {
        public int Total { get; set; }
        public TweetAverages Averages { get; set; } = new TweetAverages();
        public decimal PercentContainingEmojis { get; set; }
        public decimal PercentContainingUrl { get; set; }
        public decimal PercentContainingPhotoUrl { get; set; }
        public IEnumerable<KeyValuePair<string, int>> TopDomains { get; set; } = new Dictionary<string, int>();
        public IEnumerable<KeyValuePair<string, int>> TopEmojis { get; set; } = new Dictionary<string, int>();
        public IEnumerable<KeyValuePair<string, int>> TopHashtags { get; set; } = new Dictionary<string, int>();
        public IEnumerable<KeyValuePair<string, int>> TopCryptos { get; set; } = new Dictionary<string, int>();
    }
}
