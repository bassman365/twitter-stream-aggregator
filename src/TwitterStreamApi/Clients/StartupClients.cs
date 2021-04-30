using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TwitterStreamApi.Clients
{
    public static class StartupClients
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<ITwitterClient, TwitterClient>(client =>
            {
                var url = configuration["Twitter:BaseUrl"];
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    configuration["Twitter:Token"]);

            })
            .AddPolicyHandler(GetRetryPolicy());
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(
                    5, 
                    retryAttempt => 
                        TimeSpan.FromSeconds(Math.Pow(2,retryAttempt)));
        }
    }
}
