using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TwitterStreamService.Clients;

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
                    services.AddSingleton<TweetStreamProcessor>();
                    StartupClients.ConfigureServices(services, hostContext.Configuration);
                    services.AddHostedService<Worker>();
                })
                .Build();

            await host.StartAsync();

            var tweetStreamProcessor = host.Services.GetRequiredService<TweetStreamProcessor>();
            tweetStreamProcessor.Run();

            await host.WaitForShutdownAsync();
            //.ConfigureAppConfiguration((hostingContext, config) =>
            //{
            //    config.AddJsonFile(
            //        "appsettings.Local.json",
            //         optional: true,
            //         reloadOnChange: true);

            //    config.AddJsonFile(
            //        "appsettings.json",
            //        optional: true,
            //        reloadOnChange: true);

            //    config.AddJsonFile(
            //        $"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
            //        optional: true,
            //        reloadOnChange: true);
            //})
            //.ConfigureServices((hostContext, services) =>
            //{
            //    StartupClients.ConfigureServices(services, hostContext.Configuration);
            //    services.AddHostedService<Worker>();
            //});
        }

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureAppConfiguration((hostingContext, config) =>
        //        {
        //            config.AddJsonFile(
        //                "appsettings.Local.json",
        //                 optional: true,
        //                 reloadOnChange: true);
                    
        //            config.AddJsonFile(
        //                "appsettings.json", 
        //                optional: true,
        //                reloadOnChange: true);

        //            config.AddJsonFile(
        //                $"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", 
        //                optional: true,
        //                reloadOnChange: true);
        //        })
        //        .ConfigureServices((hostContext, services) =>
        //        {
        //            StartupClients.ConfigureServices(services, hostContext.Configuration);
        //            services.AddHostedService<Worker>();                 
        //        });
    }
}
