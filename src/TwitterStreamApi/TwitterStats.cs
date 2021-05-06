using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TwitterStreamApi.Models.TweetModels;

namespace TwitterStreamApi
{
    public class TwitterStats 
    {
        private static int totalTweetCount = 0;
        private static int tweetsWithEmojisCount = 0;
        private static int tweetsWithUrlsCount = 0;
        private static int tweetsWithImageUrlsCount = 0;
        private static DateTime firstTweetDateTimeUtc = new();
        private static readonly ConcurrentDictionary<string, int> emojis = new();
        private static readonly ConcurrentDictionary<string, int> hashtags = new();
        private static readonly ConcurrentDictionary<string, int> domains = new();
        private static readonly object Instancelock = new();
        private TwitterStats(){}
        private static TwitterStats? instance = null;
        public static TwitterStats GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (Instancelock)
                    {
                        if (instance == null)
                        {
                            instance = new TwitterStats();
                        }
                    }
                }
                return instance;
            }
        }

        public static int TotalTweets => totalTweetCount;
        //public static int EmojiTweetCount => tweetsWithEmojisCount;

        public static void IncrementTweet()
        {
            if (totalTweetCount == 0)
            {
                firstTweetDateTimeUtc = DateTime.UtcNow;
            }
            totalTweetCount++;
        }
        public static void IncrementEmojiTweets()
        {
            tweetsWithEmojisCount++;
        }
        public static void IncrementUrlTweets()
        {
            tweetsWithUrlsCount++;
        }
        public static void IncrementImageUrlTweets()
        {
            tweetsWithImageUrlsCount++;
        }
        public static void AddEmoji(string emojiText)
        {
            emojis.AddOrUpdate(emojiText, 1, (key, currentValue) =>
            {
                currentValue++;
                return currentValue;
            });
        }
        public static void AddHashtag(string hashtag)
        {
            hashtags.AddOrUpdate(hashtag, 1, (key, currentValue) =>
            {
                currentValue++;
                return currentValue;
            });
        }
        public static void AddDomain(string domain)
        {
            domains.AddOrUpdate(domain, 1, (key, currentValue) =>
            {
                currentValue++;
                return currentValue;
            });
        }

        public static IEnumerable<KeyValuePair<string, int>> GetTopEmojis(int numberOfElements = 10)
        {
            return emojis
                .OrderByDescending(x => x.Value)
                .Take(numberOfElements);
        }
        public static IEnumerable<KeyValuePair<string, int>> GetTopHashtags(int numberOfElements = 10)
        {
            return hashtags
                .OrderByDescending(x => x.Value)
                .Take(numberOfElements);
        }
        public static IEnumerable<KeyValuePair<string, int>> GetTopDomains(int numberOfElements = 10)
        {
            return domains
                .OrderByDescending(x => x.Value)
                .Take(numberOfElements);
        }
        public static decimal GetUrlPercent()
        {
            return (tweetsWithUrlsCount / (decimal)TotalTweets) * 100;
        }
        public static decimal GetImageUrlPercent()
        {
            return (tweetsWithImageUrlsCount / (decimal)TotalTweets) * 100;
        }

        public static decimal GetEmojiPercent()
        {
            return (tweetsWithEmojisCount / (decimal)TotalTweets) * 100;
        }

        public static TweetAverages AverageTweets()
        {
            var span = DateTime.UtcNow - firstTweetDateTimeUtc;
            return new TweetAverages
            {
                PerHour = (int)(TotalTweets / span.TotalHours),
                PerMinute = (int)(TotalTweets / span.TotalMinutes),
                PerSecond = (int)(TotalTweets / span.TotalSeconds)
            };
        }
    }
}
