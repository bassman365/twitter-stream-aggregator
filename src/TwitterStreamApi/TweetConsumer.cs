using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TwitterStreamApi.Models;

namespace TwitterStreamApi
{
    public class TweetConsumer : BackgroundService
    {
        private readonly ILogger<TweetConsumer> logger;
        private readonly ChannelReader<Tweet> channelReader;

        public TweetConsumer(
            ILogger<TweetConsumer> logger,
            ChannelReader<Tweet> channelReader)
        {
            this.logger = logger;
            this.channelReader = channelReader;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation($"{nameof(TweetConsumer)} consuming...");

            Parallel.For(
                0,
                20,
                new ParallelOptions { MaxDegreeOfParallelism = 4 },
                async x =>
                {
                    await ProcessTweets(stoppingToken);
                });
        }

        private async Task ProcessTweets(CancellationToken cancellationToken)
        {
            var processor = new TweetProcessor();
            while (!cancellationToken.IsCancellationRequested)
            {
                var tweet = await channelReader.ReadAsync(cancellationToken);
                await processor.Process(tweet);
                Console.WriteLine($"{tweet?.Data?.Id} processed {channelReader.Count} left in queue");
            }
        }
    }
}
