using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TwitterStreamService.Clients;
using TwitterStreamService.Models;

namespace TwitterStreamService
{
    public class TweetStreamProcessor
    {
        private readonly CancellationToken cancellationToken;
        private readonly ILogger<TweetStreamProcessor> logger;
        private readonly ITwitterClient twitterClient;

        public TweetStreamProcessor(
            IHostApplicationLifetime applicationLifetime,
            ILogger<TweetStreamProcessor> logger,
            ITwitterClient twitterClient)
        {
            cancellationToken = applicationLifetime.ApplicationStopping;
            this.logger = logger;
            this.twitterClient = twitterClient;
        }

        public void Run()
        {
            logger.LogInformation($"{nameof(TweetStreamProcessor)} starting...");
            Task.Run(async () => await ProcessTweetStream());
        }

        private async Task ProcessTweetStream()
        {
            using (var stream = await twitterClient.GetSampleStreamAsync(cancellationToken))
            {
                if (stream == null)
                {
                    logger.LogError(
                        $"{nameof(TweetStreamProcessor)} failed to get tweet stream, stopping...");

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
                            Console.WriteLine(tweet?.Data?.Text);
                        }
                        line = await streamReader.ReadLineAsync();
                    }
                }
            }
        }
    }
}
