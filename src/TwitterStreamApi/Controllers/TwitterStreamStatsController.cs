using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using TwitterStreamApi.Dtos;
using TwitterStreamApi.Repositories;

namespace TwitterStreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TwitterStreamStatsController : ControllerBase
    {
        private readonly ILogger<TwitterStreamStatsController> logger;
        private readonly ITwitterStatsRepository statsRepository;
        public TwitterStreamStatsController(
            ILogger<TwitterStreamStatsController> logger,
            ITwitterStatsRepository statsRepository)
        {
            this.logger = logger;
            this.statsRepository = statsRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(TwitterStatsDto), 200)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var stats = await Task.Run(() =>
                {
                    return new TwitterStatsDto
                    {
                        Total = statsRepository.TotalTweets,
                        Averages = statsRepository.GetAverageTweets(),
                        PercentContainingEmojis = statsRepository.GetEmojiPercentage(),
                        PercentContainingPhotoUrl = statsRepository.GetPhotoUrlPercentage(),
                        PercentContainingUrl = statsRepository.GetUrlPercentage(),
                        TopDomains = statsRepository.GetTopDomains(10),
                        TopEmojis = statsRepository.GetTopEmojis(10),
                        TopHashtags = statsRepository.GetTopHashtags(10),
                        TopCryptos = statsRepository.GetTopCryptos(10),
                    };
                });

                return Ok(stats);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    $"{nameof(TwitterStreamStatsController)} Get stats route failed: {ex.Message}");

                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

        }
    }
}
