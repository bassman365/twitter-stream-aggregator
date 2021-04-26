using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TwitterStreamAggregator.Clients;
using TwitterStreamAggregator.Models;

namespace TwitterStreamAggregator.HostedServices
{
    public class TweetCreationService : BackgroundService
    {
        private readonly ILogger<TweetCreationService> logger;
        public TweetCreationService(
            IServiceProvider services,
            ILogger<TweetCreationService> logger)
        {
            Services = services;
            this.logger = logger;
        }

        public IServiceProvider Services { get; }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation(
                $"{nameof(TweetCreationService)} starting in background");

            using (var scope = Services.CreateScope())
            {
                var twitterClient =
                    scope.ServiceProvider
                        .GetRequiredService<ITwitterClient>();

                using (var stream = await twitterClient.GetSampleStreamAsync(stoppingToken))
                {
                    if (stream == null)
                    {
                        logger.LogError(
                            $"{nameof(TweetCreationService)} failed to get tweet stream, stopping...");
                        await StopAsync(stoppingToken);
                        return;
                    }
                    using (var streamReader = new StreamReader(stream))
                    {
                        var line = await streamReader.ReadLineAsync();
                        while (line != null)
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

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation(
                $"{nameof(TweetCreationService)} is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
