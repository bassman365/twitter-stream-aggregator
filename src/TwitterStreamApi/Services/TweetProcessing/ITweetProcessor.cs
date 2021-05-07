using System.Threading.Tasks;
using TwitterStreamApi.Models.TweetModels;

namespace TwitterStreamApi.Services.TweetProcessing
{
    public interface ITweetProcessor
    {
        Task ProcessAsync(Tweet tweet);
    }
}
