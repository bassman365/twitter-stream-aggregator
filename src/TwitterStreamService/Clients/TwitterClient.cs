using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TwitterStreamService.Clients
{
    public class TwitterClient : ITwitterClient
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<TwitterClient> logger;

        public TwitterClient(HttpClient httpClient, ILogger<TwitterClient> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<Stream?> GetSampleStreamAsync(CancellationToken cancellationToken = default)
        {
            var path = @"/2/tweets/sample/stream";
            
            if (httpClient.BaseAddress == null)
            {
                logger
                    .LogError($"{nameof(httpClient.BaseAddress)} is not set for {nameof(TwitterClient)}");
                
                return null;
            }

            var uri = new Uri(httpClient.BaseAddress, path);

            try
            {
                var response = await httpClient.GetAsync(
                    uri,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken);

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStreamAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}
