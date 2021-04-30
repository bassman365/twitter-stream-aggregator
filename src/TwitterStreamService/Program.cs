using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using TwitterStreamService.Clients;
using TwitterStreamService.Models;

namespace TwitterStreamService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //CreateHostBuilder(args).Build().Run();
            using var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    //config.SetBasePath(Directory.GetCurrentDirectory());
                    //config.AddJsonFile(
                    //    "appsettings.json",
                    //    optional: true,
                    //    reloadOnChange: true);

                    //config.AddJsonFile(
                    //    $"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
                    //    optional: true,
                    //    reloadOnChange: true);

                    config.AddJsonFile(
                        "appsettings.Local.json",
                         optional: true,
                         reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<TweetStreamProducer>();
                    StartupClients.ConfigureServices(services, hostContext.Configuration);
                    services.AddHostedService<TweetConsumerManager>();
                    var channel = Channel.CreateUnbounded<Tweet>();
                    services.AddSingleton(channel.Reader);
                    services.AddSingleton(channel.Writer);
                    services.AddDbContext<TweetStatsContext>(
                        config => config.UseInMemoryDatabase("TweetStats"));
                })
                .Build();

            await host.StartAsync();

            var tweetStreamProcessor = host.Services.GetRequiredService<TweetStreamProducer>();
            tweetStreamProcessor.Run();

            await host.WaitForShutdownAsync();  
        }
    }
}
