using System;
using System.Collections.Generic;
using InfluxDB.Collector;
using InfluxDB.Collector.Diagnostics;
using Tweetinvi.Events;

namespace TwitterAnalytics.DataAccess
{
    public class TweetsRepository : ITweetsRepository
    {
        public MetricsCollector Collector;

        public TweetsRepository(string serverBaseAddress, string database, TimeSpan interval)
        {
            Console.WriteLine($"[{DateTime.Now}] - Influx DB server address: '{serverBaseAddress}'.");

            Collector = new CollectorConfiguration()
                .Batch.AtInterval(interval)
                .WriteTo.InfluxDB(serverBaseAddress, database)
                .CreateCollector();

            CollectorLog.RegisterErrorHandler((message, exception) =>
            {
                Console.Error.WriteLine($"[{DateTime.Now}] - {message}: {exception}");
            });
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