using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterStreamService
{
    public class TwitterStats 
    {
        private static int totalTweets = 0;
        private static readonly object Instancelock = new object();
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
    }
}
