using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TwitterStreamService.Models;

namespace TwitterStreamService
{
    public class TweetConsumerManager : BackgroundService
    {
        private readonly ILogger<TweetConsumerManager> _logger;
        private readonly ChannelReader<Tweet> channelReader;
        public TweetConsumerManager(
            ILogger<TweetConsumerManager> logger,
            ChannelReader<Tweet> channelReader)
        {
            _logger = logger;
            this.channelReader = channelReader;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Parallel.For(
                0, 
                20, 
                new ParallelOptions { MaxDegreeOfParallelism = 4 }, 
                async x =>
                {
                    //var consumer = new TweetConsumer(channelReader);
                    //await consumer.ProcessTweet()
                    await ProcessTweet(stoppingToken);
                });
        }

        private async Task ProcessTweet(CancellationToken cancellationToken)
        {
            var processor = new TweetConsumer();
            while (!cancellationToken.IsCancellationRequested)
            {
                //await Task.Delay(500, cancellationToken);
                var tweet = await channelReader.ReadAsync(cancellationToken);
                await processor.ProcessTweet(tweet);
                Console.WriteLine($"{tweet?.Data?.Id} processed {channelReader.Count} left in queue");
            }
        }
    }
}
