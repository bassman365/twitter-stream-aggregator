using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using TwitterStreamApi.Clients;
using TwitterStreamApi.Models;

namespace TwitterStreamApi
{
    public class TweetStreamProducer
    {
        private readonly CancellationToken cancellationToken;
        private readonly ILogger<TweetStreamProducer> logger;
        private readonly ITwitterClient twitterClient;
        private readonly ChannelWriter<Tweet> channelWriter;

        public TweetStreamProducer(
            IHostApplicationLifetime applicationLifetime,
            ILogger<TweetStreamProducer> logger,
            ITwitterClient twitterClient,
            ChannelWriter<Tweet> channelWriter)
        {
            cancellationToken = applicationLifetime.ApplicationStopping;
            this.logger = logger;
            this.twitterClient = twitterClient;
            this.channelWriter = channelWriter;
        }

        public void Run()
        {
            logger.LogInformation($"{nameof(TweetStreamProducer)} starting...");
            Task.Run(async () => await ProcessTweetStream());
        }

        private async Task ProcessTweetStream()
        {
            using (var stream = await twitterClient.GetSampleStreamAsync(cancellationToken))
            {
                if (stream == null)
                {
                    logger.LogError(
                        $"{nameof(TweetStreamProducer)} failed to get tweet stream, stopping...");

                    return;
                }
                using (var streamReader = new StreamReader(stream))
                {
                    var line = await streamReader.ReadLineAsync();
                    while (line != null && !cancellationToken.IsCancellationRequested)
                    {
                        if (!string.IsNullOrEmpty(line))
                        {
                            var tweet = JsonConvert.DeserializeObject<Tweet>(line);
                            if (tweet != null)
                            {
                                await channelWriter.WriteAsync(tweet, cancellationToken);
                            }
                        }
                        line = await streamReader.ReadLineAsync();
                    }
                }
            }
        }
    }
}
