using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using TwitterStreamService.Models;

namespace TwitterStreamService
{
    public class TweetConsumer
    {
        //private readonly ChannelReader<Tweet> channelReader;
        //private readonly TwitterStats twitterStats;
        public TweetConsumer()
        {
            //this.channelReader = channelReader;
            //this.twitterStats = twitterStats;
        }

        public async Task ProcessTweet(Tweet tweet)
        {
            TwitterStats.IncrementTweet();
            //stream.Your app should consume this sample stream and keep track of the following: 
            //•	Total number of tweets received
            //a counter that is incremented

            //•	Average tweets per hour/ minute / second

            //•	Top emojis in tweets *
            // emoji dictionary with emoji as key and count as value

            //•	Percent of tweets that contains emojis 
            // total count of dictionary values / total

            //•	Top hashtags 
            // Dictionary
            //•	Percent of tweets that contain a url
            //•	Percent of tweets that contain a photo url(pic.twitter.com or Instagram)
            //•	Top domains of urls in tweets
            //* The emoji - data project provides a convenient emoji.json file that you can use to determine which emoji unicode characters to look for in the tweet text.

        }

    }
}
