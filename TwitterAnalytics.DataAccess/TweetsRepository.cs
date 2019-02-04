using System;
using System.Collections.Generic;
using InfluxDB.Collector;
using Tweetinvi.Events;

namespace TwitterAnalytics.DataAccess
{
    public class TweetsRepository : ITweetsRepository
    {
        public MetricsCollector Collector;

        public TweetsRepository(string serverBaseAddress, string database, TimeSpan interval)
        {
            Collector = new CollectorConfiguration()
                .Batch.AtInterval(interval)
                .WriteTo.InfluxDB(serverBaseAddress, database)
                .CreateCollector();
        }

        public void Save(MatchedTweetReceivedEventArgs args)
        {
            Collector.Write("tweet",
                new Dictionary<string, object>
                {
                    {"text", args.Tweet.Text},
                    {"screen_name", args.Tweet.CreatedBy.UserIdentifier.ScreenName},
                    {"isRetweet", args.Tweet.IsRetweet},
                    {"retweetCount", args.Tweet.RetweetCount},
                    {"favorited", args.Tweet.Favorited},
                    {"favoriteCount", args.Tweet.FavoriteCount},
                    {"created_at", args.Tweet.CreatedAt}
                });
        }
    }
}