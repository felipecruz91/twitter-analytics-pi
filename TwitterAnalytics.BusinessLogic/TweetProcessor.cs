using Tweetinvi.Events;
using TwitterAnalytics.DataAccess;

namespace TwitterAnalytics.BusinessLogic
{
    public class TweetProcessor : ITweetProcessor
    {
        private readonly ITweetsRepository _tweetsRepository;

        public TweetProcessor(ITweetsRepository tweetsRepository)
        {
            _tweetsRepository = tweetsRepository;
        }

        public void ProcessTweet(string track, MatchedTweetReceivedEventArgs args)
        {
            _tweetsRepository.Save(args);
        }
    }
}