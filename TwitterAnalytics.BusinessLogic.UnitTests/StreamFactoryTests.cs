using System;
using Moq;
using NUnit.Framework;
using Tweetinvi;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Streaming;

namespace TwitterAnalytics.BusinessLogic.UnitTests
{
    public class StreamFactoryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void StartStreamTest()
        {
            // Arrange
            const string keyword = "hi";
            var tweetProcessor = new Mock<ITweetProcessor>();
            var credentials = new Mock<ITwitterCredentials>();
            var stream = new Mock<IFilteredStream>();

            var streamFactory = new StreamFactory(tweetProcessor.Object, credentials.Object, stream.Object);

            // Act
            streamFactory.StartStream(keyword);

            // Assert
            stream.Verify(x => x.AddTrack(keyword, null), Times.Once);
            stream.Verify(x => x.StartStreamMatchingAllConditions(), Times.Once);
        }
    }
}