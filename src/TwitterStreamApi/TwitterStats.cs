using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterStreamApi
{
    public class TwitterStats 
    {
        private static int totalTweets = 0;
        private static int tweetsWithEmojis = 0;
        private static int tweetsWithUrls = 0;
        private static DateTime firstTweetDateTimeUtc = new DateTime();
        private static readonly ConcurrentDictionary<string, int> emojis = new();
        private static readonly ConcurrentDictionary<string, int> hashtags = new();
        private static readonly object Instancelock = new();
        private TwitterStats()
        {

        }

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

        public static int TotalTweets => totalTweets;

        public static void IncrementTweet()
        {
            if (totalTweets == 0)
            {
                firstTweetDateTimeUtc = DateTime.UtcNow;
            }
            totalTweets++;
        }

        public static int TweetsWithEmojis => tweetsWithEmojis;
        public static void IncrementEmojiTweets()
        {
            tweetsWithEmojis++;
        }

        public static int TweetsWithUrls => tweetsWithUrls;
        public static void IncrementUrlTweets()
        {
            tweetsWithUrls++;
        }

        public static IOrderedEnumerable<KeyValuePair<string, int>> GetTopTweets()
        {
            return emojis.OrderByDescending(x => x.Value);
        }

        public static void AddEmoji(string emojiText)
        {
            emojis.AddOrUpdate(emojiText, 1, (key, currentValue) =>
            {
                currentValue++;
                return currentValue;
            });
        }

        public static IOrderedEnumerable<KeyValuePair<string, int>> GetTopHashtags()
        {
            return hashtags.OrderByDescending(x => x.Value);
        }

        public static void AddHashtag(string hashtag)
        {
            hashtags.AddOrUpdate(hashtag, 1, (key, currentValue) =>
            {
                currentValue++;
                return currentValue;
            });
        }

        public static int AverageTweetsPerHour()
        {
            var span = DateTime.UtcNow - firstTweetDateTimeUtc;
            var hourAvg = (int)(TotalTweets / span.TotalHours);
            return hourAvg;
        }
    }
}
