using System;
using Tweetinvi;
using Tweetinvi.Models;

namespace TwitterAnalytics.BusinessLogic
{
    public class StreamFactory
    {
        private readonly ITweetProcessor _tweetProcessor;

        public StreamFactory(ITweetProcessor tweetProcessor, ITwitterCredentials credentials)
        {
            _tweetProcessor = tweetProcessor;
            Auth.SetUserCredentials(credentials.ConsumerKey, credentials.ConsumerSecret,
                credentials.AccessToken, credentials.AccessTokenSecret);
        }

        public void StartStream(string keyword)
        {
            Console.WriteLine(
                $"[{DateTime.Now}] - Starting listening for tweets that contains the keyword '{keyword}'...");

            var stream = Stream.CreateFilteredStream();
            stream.AddTrack(keyword);
            stream.MatchingTweetReceived += (sender, args) => { _tweetProcessor.ProcessTweetAsync(keyword, args); };
            stream.StartStreamMatchingAllConditions();
        }
    }
}