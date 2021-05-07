using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TwitterStreamApi.Models.TweetModels;
using TwitterStreamApi.Repositories;
using TwitterStreamApi.Services.TweetProcessing;

namespace TwitterStreamApi.Tests.Repositories
{
    [TestClass]
    public class TwitterStatsRepositoryTests
    {
        [TestMethod]
        public void AddCrypto_Adds_ExpectedCryptos()
        {
            var coins = new Dictionary<string, int>
            {
                { "testcoin1", 1 },
                { "testcoin2", 1 },
                { "testcoin3", 1 }
            };

            var repo = new TwitterStatsRepository();
            foreach (var coin in coins.Keys)
            {
                repo.AddCrypto(coin);
            }

            var results = repo.GetTopCryptos(3);

            results
                .Should()
                .BeEquivalentTo(coins);
        }

        [TestMethod]
        public void AddCrypto_Updates_ExpectedCryptos()
        {
            var coins = new Dictionary<string, int>
            {
                { "testcoin1", 1 },
                { "testcoin2", 1 },
                { "testcoin3", 1 }
            };

            var repo = new TwitterStatsRepository();
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine($"i value: {i}");
                foreach (var coin in coins.Keys)
                {
                    repo.AddCrypto(coin);
                }
            }

            var expectedCoins = new Dictionary<string, int>
            {
                { "testcoin1", 2 },
                { "testcoin2", 2 },
                { "testcoin3", 2 }
            };

            var results = repo.GetTopCryptos(3);

            results
                .Should()
                .BeEquivalentTo(expectedCoins);
        }

        [TestMethod]
        public void AddDomain_Adds_ExpectedDomains()
        {
            var domains = new Dictionary<string, int>
            {
                { "www.test1.org", 1 },
                { "www.test2.org", 1 },
                { "www.test3.org", 1 }
            };

            var repo = new TwitterStatsRepository();
            foreach (var domain in domains.Keys)
            {
                repo.AddDomain(domain);
            }

            var results = repo.GetTopDomains(3);

            results
                .Should()
                .BeEquivalentTo(domains);
        }

        [TestMethod]
        public void AddDomain_Updates_ExpectedDomains()
        {
            var domains = new Dictionary<string, int>
            {
                { "www.test1.org", 1 },
                { "www.test2.org", 1 },
                { "www.test3.org", 1 }
            };

            var repo = new TwitterStatsRepository();

            for (int i = 0; i < 2; i++)
            {
                foreach (var domain in domains.Keys)
                {
                    repo.AddDomain(domain);
                }
            }

            var expectedDomains = new Dictionary<string, int>
            {
                { "www.test1.org", 2 },
                { "www.test2.org", 2 },
                { "www.test3.org", 2 }
            };
            var results = repo.GetTopDomains(3);

            results
                .Should()
                .BeEquivalentTo(expectedDomains);
        }

        [TestMethod]
        public void AddEmoji_Adds_ExpectedDomains()
        {
            var emojis = new Dictionary<string, int>
            {
                { "😿", 1 },
                { "🙀", 1 },
                { "😾", 1 }
            };

            var repo = new TwitterStatsRepository();
            foreach (var emoji in emojis.Keys)
            {
                repo.AddEmoji(emoji);
            }

            var results = repo.GetTopEmojis(3);

            results
                .Should()
                .BeEquivalentTo(emojis);
        }

        [TestMethod]
        public void AddEmoji_Updates_ExpectedDomains()
        {
            var emojis = new Dictionary<string, int>
            {
                { "😿", 1 },
                { "🙀", 1 },
                { "😾", 1 }
            };

            var repo = new TwitterStatsRepository();
            for (int i = 0; i < 2; i++)
            {
                foreach (var emoji in emojis.Keys)
                {
                    repo.AddEmoji(emoji);
                }
            }

            var expectedEmojis = new Dictionary<string, int>
            {
                { "😿", 2 },
                { "🙀", 2 },
                { "😾", 2 }
            };

            var results = repo.GetTopEmojis(3);

            results
                .Should()
                .BeEquivalentTo(expectedEmojis);
        }

        [TestMethod]
        public void AddHashtag_Adds_ExpectedHashtags()
        {
            var hashtags = new Dictionary<string, int>
            {
                { "tag1", 1 },
                { "tag2", 1 },
                { "tag3", 1 }
            };

            var repo = new TwitterStatsRepository();
            foreach (var hashtag in hashtags.Keys)
            {
                repo.AddHashtag(hashtag);
            }

            var results = repo.GetTopHashtags(3);

            results
                .Should()
                .BeEquivalentTo(hashtags);
        }

        [TestMethod]
        public void AddHashtag_Updates_ExpectedHashtags()
        {
            var hashtags = new Dictionary<string, int>
            {
                { "tag1", 1 },
                { "tag2", 1 },
                { "tag3", 1 }
            };

            var repo = new TwitterStatsRepository();

            for (int i = 0; i < 2; i++)
            {
                foreach (var hashtag in hashtags.Keys)
                {
                    repo.AddHashtag(hashtag);
                }
            }

            var expectedHashtags = new Dictionary<string, int>
            {
                { "tag1", 2 },
                { "tag2", 2 },
                { "tag3", 2 }
            };

            var results = repo.GetTopHashtags(3);

            results
                .Should()
                .BeEquivalentTo(expectedHashtags);
        }

        [TestMethod]
        public void GetEmojiPercentage_Returns_Expected_Percentage()
        {
            var repo = new TwitterStatsRepository();

            for (int i = 0; i < 100; i++)
            {
                repo.IncrementTweetCount();
            }

            for (int i = 0; i < 25; i++)
            {
                repo.IncrementEmojiTweetCount();
            }

            repo.GetEmojiPercentage()
                .Should()
                .Be(25.0m);
        }

        [TestMethod]
        public void GetPhotoUrlPercentage_Returns_Expected_Percentage()
        {
            var repo = new TwitterStatsRepository();

            for (int i = 0; i < 100; i++)
            {
                repo.IncrementTweetCount();
            }

            for (int i = 0; i < 25; i++)
            {
                repo.IncrementPhotoUrlTweetCount();
            }

            repo.GetPhotoUrlPercentage()
                .Should()
                .Be(25.0m);
        }

        [TestMethod]
        public void GetUrlPercentage_Returns_Expected_Percentage()
        {
            var repo = new TwitterStatsRepository();

            for (int i = 0; i < 100; i++)
            {
                repo.IncrementTweetCount();
            }

            for (int i = 0; i < 25; i++)
            {
                repo.IncrementUrlTweetCount();
            }

            repo.GetUrlPercentage()
                .Should()
                .Be(25.0m);
        }
    }
}
