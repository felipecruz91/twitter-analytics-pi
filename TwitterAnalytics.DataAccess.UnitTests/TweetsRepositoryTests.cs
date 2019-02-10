using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Tweetinvi.Events;
using Tweetinvi.Models;

namespace TwitterAnalytics.DataAccess.UnitTests
{
    public class TweetsRepositoryTests
    {
        [Test]
        public void SaveTweet()
        {
            // Arrange
            var metricsCollectorWrapper = new Mock<IMetricsCollectorWrapper>();
            var tweetsRepository = new TweetsRepository(metricsCollectorWrapper.Object);
            var tweet = new Mock<ITweet>();
            tweet.SetupGet(x => x.Text).Returns("Hi there!");
            tweet.SetupGet(x => x.CreatedBy.UserIdentifier.ScreenName).Returns("@bob");
            tweet.SetupGet(x => x.IsRetweet).Returns(false);
            tweet.SetupGet(x => x.RetweetCount).Returns(5);
            tweet.SetupGet(x => x.Favorited).Returns(true);
            tweet.SetupGet(x => x.FavoriteCount).Returns(1);
            tweet.SetupGet(x => x.CreatedAt).Returns(new DateTime(2019, 02, 10, 01, 02, 03));

            // Act
            tweetsRepository.SaveTweet(new MatchedTweetReceivedEventArgs(tweet.Object, ""));

            // Assert
            var fields = new Dictionary<string, object>
            {
                {"text", "Hi there!"},
                {"screen_name", "@bob"},
                {"isRetweet", false},
                {"retweetCount", 5},
                {"favorited", true},
                {"favoriteCount", 1},
                {"created_at", new DateTime(2019, 02, 10, 01, 02, 03)}
            };

            metricsCollectorWrapper.Verify(x => x.Write("tweet", fields), Times.Once);
        }
    }
}