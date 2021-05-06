using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using TwitterStreamApi.Models.TweetModels;

namespace TwitterStreamApi.Services.TweetProcessing
{
    public class TweetProcessor : ITweetProcessor
    {
        private readonly ILogger<TweetProcessor> logger;
        private readonly ITweetParser tweetParser;
        public TweetProcessor(
            ILogger<TweetProcessor> logger, 
            ITweetParser tweetParser)
        {
            this.logger = logger;
            this.tweetParser = tweetParser;
        }

        public ValueTask Process(Tweet tweet)
        {
            TwitterStats.IncrementTweet();

            var emojis = tweetParser.GetEmojis(tweet);
            if (emojis.Any())
            {
                TwitterStats.IncrementEmojiTweets();
                foreach (var emoji in emojis)
                {
                    TwitterStats.AddEmoji(emoji);
                }
            }

            var hashtags = tweetParser.GetHashtags(tweet);

            foreach (var hashtag in hashtags)
            {
                TwitterStats.AddHashtag(hashtag);
            }

            var urls = tweetParser.GetUris(tweet);
            if (tweetParser.ContainsImageUrls(urls))
            {
                TwitterStats.IncrementImageUrlTweets();
            }

            if (urls.Any())
            {
                TwitterStats.IncrementUrlTweets();
                foreach (var url in urls)
                {
                    TwitterStats.AddDomain(url.Host);
                }
            }

            return new ValueTask();
        }
    }
}
