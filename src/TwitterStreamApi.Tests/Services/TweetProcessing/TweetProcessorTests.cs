using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitterStreamApi.Models.TweetModels;
using TwitterStreamApi.Repositories;
using TwitterStreamApi.Services.TweetProcessing;

namespace TwitterStreamApi.Tests
{
    [TestClass]
    public class TweetProcessorTests
    {
        private Mock<ILogger<TweetProcessor>> mockLogger;
        private Mock<ITweetParser> mockTweetParser;
        private Mock<ITwitterStatsRepository> mockStatsRepository;
        private TweetProcessor tweetProcessor;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<TweetProcessor>>();
            mockTweetParser = new Mock<ITweetParser>();
            mockStatsRepository = new Mock<ITwitterStatsRepository>();
            tweetProcessor = new TweetProcessor(
                mockLogger.Object,
                mockTweetParser.Object,
                mockStatsRepository.Object);

            mockStatsRepository
                .Setup(x => x.IncrementTweetCount())
                .Verifiable();
            mockStatsRepository
                .Setup(x => x.IncrementEmojiTweetCount())
                .Verifiable();
            mockStatsRepository
                .Setup(x => x.IncrementPhotoUrlTweetCount())
                .Verifiable();
            mockStatsRepository
                .Setup(x => x.IncrementUrlTweetCount())
                .Verifiable();
            mockStatsRepository
                .Setup(x => x.AddEmoji(It.IsAny<string>()))
                .Verifiable();
            mockStatsRepository
                .Setup(x => x.AddHashtag(It.IsAny<string>()))
                .Verifiable();
            mockStatsRepository
                .Setup(x => x.AddCrypto(It.IsAny<string>()))
                .Verifiable();
        }

        [TestMethod]
        public async Task TweetProcessor_ProcessAsync_IncrementsTweetCount()
        {
            await tweetProcessor.ProcessAsync(new Tweet());

            mockStatsRepository.Verify(x => x.IncrementTweetCount(), Times.Once);
        }

        [TestMethod]
        public async Task TweetProcessor_ProcessAsync_IncrementsEmojisCount_WhenEmojisArePresent()
        {
            mockTweetParser
                .Setup(x => x.GetEmojis(It.IsAny<Tweet>()))
                .Returns(new List<string>() { "😁" });

            await tweetProcessor.ProcessAsync(new Tweet());

            mockStatsRepository.Verify(x => x.IncrementEmojiTweetCount(), Times.Once);
        }

        [TestMethod]
        public async Task TweetProcessor_ProcessAsync_AddsEmojis_WhenEmojisArePresent()
        {
            var emojis = new string[] { "🥺", "🎃", "🤨" };
            mockTweetParser
                .Setup(x => x.GetEmojis(It.IsAny<Tweet>()))
                .Returns(emojis);

            await tweetProcessor.ProcessAsync(new Tweet());

            mockStatsRepository.Verify(x => x.AddEmoji(emojis[0]), Times.Once);
            mockStatsRepository.Verify(x => x.AddEmoji(emojis[1]), Times.Once);
            mockStatsRepository.Verify(x => x.AddEmoji(emojis[2]), Times.Once);
        }

        [TestMethod]
        public async Task TweetProcessor_ProcessAsync_AddsHashtags_WhenHashtagsArePresent()
        {
            var hashtags = new string[] { "hashtagTest1", "hashtagTest2", "hashtagTest3" };
            mockTweetParser
                .Setup(x => x.GetHashtags(It.IsAny<Tweet>()))
                .Returns(hashtags);

            await tweetProcessor.ProcessAsync(new Tweet());

            mockStatsRepository.Verify(x => x.AddHashtag(hashtags[0]), Times.Once);
            mockStatsRepository.Verify(x => x.AddHashtag(hashtags[1]), Times.Once);
            mockStatsRepository.Verify(x => x.AddHashtag(hashtags[2]), Times.Once);
        }

        [TestMethod]
        public async Task TweetProcessor_ProcessAsync_IncrementsPhotoUrlTweetCount_WhenPhotoUrlsArePresent()
        {
            var uris = new Uri[]
            {
                new Uri("http://www.test1.org"),
                new Uri("http://www.test2.org"),
                new Uri("http://www.test3.org")
            };

            mockTweetParser
                .Setup(x => x.GetUris(It.IsAny<Tweet>()))
                .Returns(uris);

            mockTweetParser
                .Setup(x => x.ContainsPhotoUrls(uris))
                .Returns(true);

            await tweetProcessor.ProcessAsync(new Tweet());

            mockStatsRepository.Verify(x => x.IncrementPhotoUrlTweetCount(), Times.Once);
        }

        [TestMethod]
        public async Task TweetProcessor_ProcessAsync_IncrementUrlTweetCount_WhenUrlsArePresent()
        {
            var uris = new Uri[]
            {
                new Uri("http://www.test1.org"),
                new Uri("http://www.test2.org"),
                new Uri("http://www.test3.org")
            };

            mockTweetParser
                .Setup(x => x.GetUris(It.IsAny<Tweet>()))
                .Returns(uris);

            mockTweetParser
                .Setup(x => x.ContainsPhotoUrls(uris))
                .Returns(false);

            await tweetProcessor.ProcessAsync(new Tweet());

            mockStatsRepository.Verify(x => x.IncrementUrlTweetCount(), Times.Once);
        }

        [TestMethod]
        public async Task TweetProcessor_ProcessAsync_AddsDomain_WhenUrlsArePresent()
        {
            var uris = new Uri[]
            {
                new Uri("http://www.test1.org"),
                new Uri("http://www.test2.org"),
                new Uri("http://www.test3.org")
            };

            mockTweetParser
                .Setup(x => x.GetUris(It.IsAny<Tweet>()))
                .Returns(uris);

            mockTweetParser
                .Setup(x => x.ContainsPhotoUrls(uris))
                .Returns(false);

            await tweetProcessor.ProcessAsync(new Tweet());

            mockStatsRepository.Verify(x => x.AddDomain(uris[0].Host), Times.Once);
            mockStatsRepository.Verify(x => x.AddDomain(uris[1].Host), Times.Once);
            mockStatsRepository.Verify(x => x.AddDomain(uris[2].Host), Times.Once);
        }

        [TestMethod]
        public async Task TweetProcessor_ProcessAsync_AddsCrypto_WhenCryptossArePresent()
        {
            var coins = new string[]
            {
                "Bitcoin",
                "Ethereum",
                "Dogecoin"
            };

            mockTweetParser
                .Setup(x => x.GetCryptos(It.IsAny<Tweet>()))
                .Returns(coins);

            await tweetProcessor.ProcessAsync(new Tweet());

            mockStatsRepository.Verify(x => x.AddCrypto(coins[0]), Times.Once);
            mockStatsRepository.Verify(x => x.AddCrypto(coins[1]), Times.Once);
            mockStatsRepository.Verify(x => x.AddCrypto(coins[2]), Times.Once);
        }

        [TestMethod]
        public async Task TweetProcessor_ProcessAsync_LogsError_WhenExceptionIsThrown()
        {
            var tweet = new Tweet();
            tweet.Data.Text = "This isn't going to go well";

            mockStatsRepository
                .Setup(x => x.IncrementTweetCount())
                .Throws(new Exception());

            var expectedLogMessage = $"{nameof(TweetProcessor)} failed to process tweet: {tweet.Data.Text}";

            await tweetProcessor.ProcessAsync(tweet);

            mockLogger.Verify(x => x.Log(
                It.Is<LogLevel>(x => x == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Equals(expectedLogMessage)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }
    }
}
