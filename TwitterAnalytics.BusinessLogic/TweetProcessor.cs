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
            //Console.WriteLine($"A tweet containing '{track}' has been found; the tweet is '" + args.Tweet +
            //                  "'");

            _tweetsRepository.Save(args);
        }
    }
}