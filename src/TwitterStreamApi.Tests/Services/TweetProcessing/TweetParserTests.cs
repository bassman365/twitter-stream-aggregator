using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitterStreamApi.Models.TweetModels;
using TwitterStreamApi.Services.TweetProcessing;

namespace TwitterStreamApi.Tests
{
    [TestClass]
    public class TweetParserTests
    {
        private TweetParser tweetParser;
        private Mock<ILogger<TweetParser>> mockLogger;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<TweetParser>>();
            tweetParser = new TweetParser(mockLogger.Object);
        }

        [TestMethod]
        public void TweetParser_GetUrls_ReturnsExpectedUrls_WhenDestinationUrls_AreAllValid()
        {
            var tweet = new Tweet();

            tweet.Data.Entities.TweetUrls = GetTestUrls()
                .Select(x => new TweetUrl() { DestinationUrl = x });

            var expectedResults = GetTestUrls()
                .Select(x => new Uri(x));

            var results = tweetParser.GetUris(tweet);

            results.Should()
                .BeEquivalentTo(expectedResults);
        }

        [TestMethod]
        public void TweetParser_GetUrls_ReturnsExpectedUrls_WhenDestinationUrls_ContainSomeValidUrls()
        {
            var tweet = new Tweet();
            var badUrl = @"notgonnawork.com";

            tweet.Data.Entities.TweetUrls = GetTestUrls()
                .Select(x => new TweetUrl() { DestinationUrl = x })
                .Concat(new List<TweetUrl>() { new TweetUrl() { DestinationUrl = badUrl } });

            var expectedResults = GetTestUrls()
                .Select(x => new Uri(x));

            var results = tweetParser.GetUris(tweet);

            results.Should()
                .BeEquivalentTo(expectedResults);
        }

        [TestMethod]
        public void TweetParser_GetUrls_LogsError_WhenDestinationUrls_ContainSomeValidUrls()
        {
            var tweet = new Tweet();
            var badUrl = @"notgonnawork.com";

            tweet.Data.Entities.TweetUrls = GetTestUrls()
                .Select(x => new TweetUrl() { DestinationUrl = x })
                .Concat(new List<TweetUrl>() { new TweetUrl() { DestinationUrl = badUrl } });

            var results = tweetParser.GetUris(tweet);

            var expectedLogMessage = $"{nameof(TweetParser.GetUris)} failed to construct Uri from tweet with url: {badUrl}";

            mockLogger.Verify(x => x.Log(
                It.Is<LogLevel>(x => x == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Equals(expectedLogMessage)),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));
        }

        [TestMethod]
        public void TweetParser_GetHashtags_ReturnsExpectedHashtags_WhenHashtagsArePresent()
        {
            var hashtags = new List<string>()
            {
                "testTag1",
                "testTag2",
                string.Empty,
                "testTag3"
            };
            var tweet = new Tweet();
            tweet.Data.Entities.Hashtags = hashtags.Select(x => new TweetHashtag() { Tag = x });

            var expectedHashTags = hashtags.Where(x => !string.IsNullOrEmpty(x));
            var results = tweetParser.GetHashtags(tweet);

            results.Should().BeEquivalentTo(expectedHashTags);
        }

        [TestMethod]
        public void TweetParser_GetEmojis_ReturnsExpectedEmojis_WhenEmojisArePresent()
        {
            var tweet = new Tweet();
            tweet.Data.Text = "👊 I like emojis in tests 😀";
            var expectedEmojis = new List<string>() { "👊", "😀" };
            var result = tweetParser.GetEmojis(tweet);
            result.Should().BeEquivalentTo(expectedEmojis);
        }

        [TestMethod]
        public void TweetParser_GetCryptos_ReturnsExpectedCryptos_WhenCryptosArePresent()
        {
            var tweet = new Tweet();
            tweet.Data.Text = "bitcoin, ethereum, and dogecoin oh my!";
            var expectedCoins = new List<string>() { "Bitcoin", "Ethereum", "Dogecoin" };
            var result = tweetParser.GetCryptos(tweet);
            result.Should().BeEquivalentTo(expectedCoins);
        }

        private static IEnumerable<string> GetTestUrls()
        {
            return new List<string>()
            {
                @"https://pic.twitter.com/test/a",
                @"https://www.instagram.com/p/test/",
                @"http://www.test.org"
            };
        }
    }
}
