using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TwitterStreamApi.Dtos;

namespace TwitterStreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TwitterStreamStatsController : ControllerBase
    {
        private readonly ILogger<TwitterStreamStatsController> logger;
        public TwitterStreamStatsController(
            ILogger<TwitterStreamStatsController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(TwitterStatsDto), 200)]
        public async Task<IActionResult> Get()
        {
            var stats = await Task.Run(() =>
            {
                return new TwitterStatsDto
                {
                    Averages = TwitterStats.AverageTweets(),
                    PercentContainingEmojis = TwitterStats.GetEmojiPercent(),
                    ImageUrlPercent = TwitterStats.GetImageUrlPercent(),
                    TopDomains = TwitterStats.GetTopDomains(10),
                    TopEmojis = TwitterStats.GetTopEmojis(10),
                    TopHashtags = TwitterStats.GetTopHashtags(10),
                    Total = TwitterStats.TotalTweets,
                    UrlPercent = TwitterStats.GetUrlPercent()
                };
            });

            return Ok(stats);
        }
    }
}
