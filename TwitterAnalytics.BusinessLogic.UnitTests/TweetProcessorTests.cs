using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using Tweetinvi.Events;
using Tweetinvi.Models;
using TwitterAnalytics.DataAccess;

namespace TwitterAnalytics.BusinessLogic.UnitTests
{
    public class TweetProcessorTests
    {
        [Test]
        public void ProcessTweetAsyncTest()
        {
            // Arrange
            const string keyword = "I am happy!";
            var tweetsRepository = new Mock<ITweetsRepository>();
            var configuration = new Mock<ITextAnalyticsConfiguration>();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(
                        "{\r\n\"documents\":[\r\n{\r\n\"score\":0.9999237060546875,\r\n\"id\":\"1\"\r\n}\r\n],\r\n\"errors\":[]\r\n}")
                })
                .Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object);

            var tweetProcessor = new TweetProcessor(tweetsRepository.Object, configuration.Object, httpClient);
            var tweet = new Mock<ITweet>();
            tweet.SetupGet(x => x.FullText).Returns(keyword);

            var matchedTweetReceivedEventArgs = new MatchedTweetReceivedEventArgs(tweet.Object, "");

            // Act
            tweetProcessor.ProcessTweetAsync(keyword, matchedTweetReceivedEventArgs).Wait();

            // Assert
            tweetsRepository.Verify(x => x.SaveTweet(matchedTweetReceivedEventArgs));
            tweetsRepository.Verify(x => x.SaveSentiment(It.Is<TweetSentiment>(t =>
                t.FullText == "I am happy!" && Math.Abs(t.Score - 0.9999237060546875) < 0.01)));
        }
    }
}