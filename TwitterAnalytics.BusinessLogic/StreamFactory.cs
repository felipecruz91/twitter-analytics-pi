using Tweetinvi;

namespace TwitterAnalytics.BusinessLogic
{
    public class StreamFactory
    {
        private readonly ITweetProcessor _tweetProcessor;

        public StreamFactory(ITweetProcessor tweetProcessor)
        {
            _tweetProcessor = tweetProcessor;
        }

        public void StartStream(string track)
        {
            // Set up your credentials (https://apps.twitter.com)
            const string consumerKey = Environment.GetEnvironmentVariable("TWITTER_CONSUMER_KEY");
            const string consumerSecret = Environment.GetEnvironmentVariable("TWITTER_CONSUMER_SECRET");
            const string userAccessToken = Environment.GetEnvironmentVariable("TWITTER_USER_ACCESS_TOKEN");
            const string userAccessSecret = Environment.GetEnvironmentVariable("TWITTER_USER_ACCESS_SECRET");

            Auth.SetUserCredentials(consumerKey, consumerSecret,
                userAccessToken, userAccessSecret);

            var stream = Stream.CreateFilteredStream();
            stream.AddTrack(track);
            stream.MatchingTweetReceived += (sender, args) => { _tweetProcessor.ProcessTweet(track, args); };
            stream.StartStreamMatchingAllConditions();
        }
    }
}