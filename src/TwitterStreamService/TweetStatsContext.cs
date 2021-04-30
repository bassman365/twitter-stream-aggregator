using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TwitterStreamService.Models;

namespace TwitterStreamService
{
    public class TweetStatsContext : DbContext
    {
        public TweetStatsContext(DbContextOptions<TweetStatsContext> options)
            : base(options)
        {
        }

        public DbSet<TweetStats> TweetStatSet { get; set; }

        public async Task<TweetStats> GetTweetStatsAsync()
        {
            return await TweetStatSet.FirstOrDefaultAsync();
        }
        
    }
}
