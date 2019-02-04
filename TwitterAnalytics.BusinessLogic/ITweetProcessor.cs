using Tweetinvi.Events;

namespace TwitterAnalytics.BusinessLogic
{
    public interface ITweetProcessor
    {
        void ProcessTweet(string track, MatchedTweetReceivedEventArgs args);
    }
}