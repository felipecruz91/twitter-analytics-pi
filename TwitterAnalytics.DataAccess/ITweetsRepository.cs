using Tweetinvi.Events;

namespace TwitterAnalytics.DataAccess
{
    public interface ITweetsRepository
    {
        void Save(MatchedTweetReceivedEventArgs args);
    }
}