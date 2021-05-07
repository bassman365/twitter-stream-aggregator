using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TwitterStreamApi.Models.TweetModels;
using TwitterStreamApi.Repositories;

namespace TwitterStreamApi.Services.TweetProcessing
{
    public class TweetProcessor : ITweetProcessor
    {
        private readonly ILogger<TweetProcessor> logger;
        private readonly ITweetParser tweetParser;
        private readonly ITwitterStatsRepository statsRepository;

        public TweetProcessor(
            ILogger<TweetProcessor> logger,
            ITweetParser tweetParser,
            ITwitterStatsRepository statsRepository)
        {
            this.logger = logger;
            this.tweetParser = tweetParser;
            this.statsRepository = statsRepository;
        }

        public async Task ProcessAsync(Tweet tweet)
        {
            await Task.Run(() =>
            {
                try
                {
                    statsRepository.IncrementTweetCount();
                    ProcessEmojis(tweet);
                    ProcessHashtags(tweet);
                    ProcessUrls(tweet);
                    ProcessCryptos(tweet);
                }
                catch (Exception ex)
                {
                    logger.LogError(
                        ex,
                        $"{nameof(TweetProcessor)} failed to process tweet: {tweet.Data.Text}");
                }
            });
        }

        private void ProcessEmojis(Tweet tweet)
        {
            var emojis = tweetParser.GetEmojis(tweet);
            if (emojis.Any())
            {
                statsRepository.IncrementEmojiTweetCount();
                foreach (var emoji in emojis)
                {
                    statsRepository.AddEmoji(emoji);
                }
            }
        }

        private void ProcessHashtags(Tweet tweet)
        {
            var hashtags = tweetParser.GetHashtags(tweet);
            foreach (var hashtag in hashtags)
            {
                statsRepository.AddHashtag(hashtag);
            }
        }

        private void ProcessUrls(Tweet tweet)
        {
            var uris = tweetParser.GetUris(tweet);
            if (tweetParser.ContainsPhotoUrls(uris))
            {
                statsRepository.IncrementPhotoUrlTweetCount();
            }

            if (uris.Any())
            {
                statsRepository.IncrementUrlTweetCount();
                foreach (var uri in uris)
                {
                    statsRepository.AddDomain(uri.Host);
                }
            }
        }

        private void ProcessCryptos(Tweet tweet)
        {
            var cryptos = tweetParser.GetCryptos(tweet);
            if (cryptos.Any())
            {
                foreach (var crypto in cryptos)
                {
                    statsRepository.AddCrypto(crypto);
                }
            }
        }
    }
}
