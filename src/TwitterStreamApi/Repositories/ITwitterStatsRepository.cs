using System.Collections.Generic;
using TwitterStreamApi.Models.TweetModels;

namespace TwitterStreamApi.Repositories
{
    public interface ITwitterStatsRepository
    {
        public int TotalTweets { get; }
        public void AddCrypto(string crypto);
        public void AddDomain(string domain);
        public void AddEmoji(string emojiText);
        public void AddHashtag(string hashtag);
        public TweetAverages GetAverageTweets();
        public decimal GetEmojiPercentage();
        public decimal GetPhotoUrlPercentage();
        public IEnumerable<KeyValuePair<string, int>> GetTopCryptos(int numberOfElements = 10);
        public IEnumerable<KeyValuePair<string, int>> GetTopDomains(int numberOfElements = 10);
        public IEnumerable<KeyValuePair<string, int>> GetTopEmojis(int numberOfElements = 10);
        public IEnumerable<KeyValuePair<string, int>> GetTopHashtags(int numberOfElements = 10);
        public decimal GetUrlPercentage();
        public void IncrementEmojiTweetCount();
        public void IncrementPhotoUrlTweetCount();
        public void IncrementTweetCount();
        public void IncrementUrlTweetCount();
    }
}
