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
        private static readonly ConcurrentDictionary<string, int> emojis = new();
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
            totalTweets++;
        }

        public static int TweetsWithEmojis => tweetsWithEmojis;
        public static void IncrementEmojiTweet()
        {
            tweetsWithEmojis++;
        }

        public static IOrderedEnumerable<KeyValuePair<string, int>> GetTopTweets()
        {
            return emojis.OrderByDescending(x => x.Value);
        }

        public static void AddEmoji(string emojiText)
        {
            var emojiValue = emojis.AddOrUpdate(emojiText, 1, (key, currentValue) =>
            {
                currentValue++;
                return currentValue;
            });
        }
    }
}
