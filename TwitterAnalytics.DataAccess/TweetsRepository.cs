using System.Collections.Generic;
using Tweetinvi.Events;
using TwitterAnalytics.BusinessLogic;

namespace TwitterAnalytics.DataAccess
{
    public class TweetsRepository : ITweetsRepository
    {
        private readonly IMetricsCollectorWrapper _metricsCollectorWrapper;

        public TweetsRepository(IMetricsCollectorWrapper metricsCollectorWrapper)
        {
            _metricsCollectorWrapper = metricsCollectorWrapper;
        }

        public void SaveTweet(MatchedTweetReceivedEventArgs args)
        {
            var fields = new Dictionary<string, object>
            {
                {"text", args.Tweet.Text},
                {"screen_name", args.Tweet.CreatedBy.UserIdentifier.ScreenName},
                {"isRetweet", args.Tweet.IsRetweet},
                {"retweetCount", args.Tweet.RetweetCount},
                {"favorited", args.Tweet.Favorited},
                {"favoriteCount", args.Tweet.FavoriteCount},
                {"created_at", args.Tweet.CreatedAt}
            };

            _metricsCollectorWrapper.Write("tweet", fields);
        }

        public void SaveSentiment(TweetSentiment tweetSentiment)
        {
            if (tweetSentiment == null)
            {
                return;
            }

            var fields = new Dictionary<string, object>
            {
                {"fullText", tweetSentiment.FullText},
                {"score", tweetSentiment.Score}
            };

            _metricsCollectorWrapper.Write("sentiment", fields);
        }
    }
}