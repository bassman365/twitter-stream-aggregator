using System.Threading.Tasks;
using TwitterStreamApi.Models.TweetModels;

namespace TwitterStreamApi.Services.TweetProcessing
{
    public interface ITweetProcessor
    {
        ValueTask Process(Tweet tweet);
    }
}
