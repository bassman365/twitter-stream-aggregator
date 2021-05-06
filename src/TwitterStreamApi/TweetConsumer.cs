using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TwitterStreamApi.Models.TweetModels;
using TwitterStreamApi.Services.TweetProcessing;

namespace TwitterStreamApi
{
    public class TweetConsumer : BackgroundService
    {
        private readonly ILogger<TweetConsumer> logger;
        private readonly ChannelReader<Tweet> channelReader;
        private readonly ITweetProcessor tweetProcessor;
        private const int QueueThreshold = 50;

        public TweetConsumer(
            ILogger<TweetConsumer> logger,
            ChannelReader<Tweet> channelReader,
            ITweetProcessor tweetProcessor)
        {
            this.logger = logger;
            this.channelReader = channelReader;
            this.tweetProcessor = tweetProcessor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation($"{nameof(TweetConsumer)} consuming...");
            await Task.Run(() => Parallel.For(
                0,
                16,
                new ParallelOptions { MaxDegreeOfParallelism = 4 },
                async x =>
                {
                    await ProcessTweets(stoppingToken);
                }));
        }

        private async Task ProcessTweets(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var tweet = await channelReader.ReadAsync(cancellationToken);
                await tweetProcessor.Process(tweet);
                if (channelReader.Count > QueueThreshold)
                {
                    logger
                        .LogWarning($"{nameof(TweetConsumer)} has exceeded its threashold of {QueueThreshold} with {channelReader.Count} waiting to be processed");
                }
            }
        }
    }
}
