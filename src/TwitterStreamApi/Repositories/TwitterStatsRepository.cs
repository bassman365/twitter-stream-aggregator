using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TwitterStreamApi.Models.TweetModels;

namespace TwitterStreamApi.Repositories
{
    public class TwitterStatsRepository : ITwitterStatsRepository
    {
        private readonly ConcurrentDictionary<string, int> cryptos = new();
        private readonly ConcurrentDictionary<string, int> domains = new();
        private readonly ConcurrentDictionary<string, int> emojis = new();
        private readonly ConcurrentDictionary<string, int> hashtags = new();
        private DateTime firstTweetDateTimeUtc = new();
        private readonly object syncObject = new();
        private int totalTweetCount = 0;
        private int tweetsWithEmojisCount = 0;
        private int tweetsWithPhotoUrlsCount = 0;
        private int tweetsWithUrlsCount = 0;
        public int TotalTweets => totalTweetCount;

        public void AddCrypto(string crypto)
        {
            cryptos.AddOrUpdate(crypto, 1, (key, currentValue) =>
            {
                currentValue++;
                return currentValue;
            });
        }

        public void AddDomain(string domain)
        {
            domains.AddOrUpdate(domain, 1, (key, currentValue) =>
            {
                currentValue++;
                return currentValue;
            });
        }

        public void AddEmoji(string emojiText)
        {
            emojis.AddOrUpdate(emojiText, 1, (key, currentValue) =>
            {
                currentValue++;
                return currentValue;
            });
        }

        public void AddHashtag(string hashtag)
        {
            hashtags.AddOrUpdate(hashtag, 1, (key, currentValue) =>
            {
                currentValue++;
                return currentValue;
            });
        }

        public TweetAverages GetAverageTweets()
        {
            var span = DateTime.UtcNow - firstTweetDateTimeUtc;
            return new TweetAverages
            {
                PerHour = (int)(TotalTweets / span.TotalHours),
                PerMinute = (int)(TotalTweets / span.TotalMinutes),
                PerSecond = (int)(TotalTweets / span.TotalSeconds)
            };
        }

        public decimal GetEmojiPercentage()
        {
            return (tweetsWithEmojisCount / (decimal)TotalTweets) * 100;
        }

        public decimal GetPhotoUrlPercentage()
        {
            return (tweetsWithPhotoUrlsCount / (decimal)TotalTweets) * 100;
        }

        public IEnumerable<KeyValuePair<string, int>> GetTopCryptos(int numberOfElements = 10)
        {
            {
                return cryptos
                    .OrderByDescending(x => x.Value)
                    .Take(numberOfElements);
            }
        }
        public IEnumerable<KeyValuePair<string, int>> GetTopDomains(int numberOfElements = 10)
        {
            return domains
                .OrderByDescending(x => x.Value)
                .Take(numberOfElements);
        }

        public IEnumerable<KeyValuePair<string, int>> GetTopEmojis(int numberOfElements = 10)
        {
            return emojis
                .OrderByDescending(x => x.Value)
                .Take(numberOfElements);
        }

        public IEnumerable<KeyValuePair<string, int>> GetTopHashtags(int numberOfElements = 10)
        {
            return hashtags
                .OrderByDescending(x => x.Value)
                .Take(numberOfElements);
        }

        public decimal GetUrlPercentage()
        {
            return (tweetsWithUrlsCount / (decimal)TotalTweets) * 100;
        }

        public void IncrementEmojiTweetCount()
        {
            lock (syncObject)
            {
                tweetsWithEmojisCount++;
            }
        }

        public void IncrementPhotoUrlTweetCount()
        {
            lock (syncObject)
            {
                tweetsWithPhotoUrlsCount++;
            }
        }

        public void IncrementTweetCount()
        {
            lock (syncObject)
            {
                if (totalTweetCount == 0)
                {
                    firstTweetDateTimeUtc = DateTime.UtcNow;
                }
                totalTweetCount++;
            }
        }

        public void IncrementUrlTweetCount()
        {
            lock (syncObject)
            {
                tweetsWithUrlsCount++;
            }
        }
    }
}