using System.Threading.Tasks;
using Tweetinvi.Events;

namespace TwitterAnalytics.BusinessLogic
{
    public interface ITweetProcessor
    {
        Task ProcessTweetAsync(string track, MatchedTweetReceivedEventArgs args);
    }
}