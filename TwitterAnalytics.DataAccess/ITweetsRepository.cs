using Tweetinvi.Events;
using TwitterAnalytics.BusinessLogic;

namespace TwitterAnalytics.DataAccess
{
    public interface ITweetsRepository
    {
        void SaveTweet(MatchedTweetReceivedEventArgs args);
        void SaveSentiment(TweetSentiment tweetSentiment);
    }
}