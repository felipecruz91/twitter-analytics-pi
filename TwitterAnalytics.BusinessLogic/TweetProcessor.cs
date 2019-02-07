﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Tweetinvi.Events;
using TwitterAnalytics.DataAccess;
using TwitterAnalytics.Domain.Models.SentimentAnalysis;

namespace TwitterAnalytics.BusinessLogic
{
    public class TweetProcessor : ITweetProcessor
    {
        private readonly HttpClient _httpClient;
        private readonly ITweetsRepository _tweetsRepository;

        public TweetProcessor(ITweetsRepository tweetsRepository, ITextAnalyticsConfiguration configuration,
            HttpClient httpClient)
        {
            _tweetsRepository = tweetsRepository;
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", $"{configuration.Key1}");
        }

        public async Task ProcessTweetAsync(string track, MatchedTweetReceivedEventArgs args)
        {
            var responseDocument = await MakeRequest(args.Tweet.FullText);

            var tweetSentiment = new TweetSentiment
            {
                FullText = args.Tweet.FullText,
                Score = responseDocument.Score
            };

            _tweetsRepository.SaveTweet(args);
            _tweetsRepository.SaveSentiment(tweetSentiment);
        }

        private async Task<ResponseDocument> MakeRequest(string tweetFullText)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            var uri = "https://westeurope.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment?" + queryString;

            HttpResponseMessage response;

            // Request body
            var requestBody = new RequestRootObject
            {
                Documents = new List<RequestDocument>
                {
                    new RequestDocument
                    {
                        Language = "en",
                        Id = "1",
                        Text = tweetFullText
                    }
                }
            };

            var serializedRequestBody = JsonConvert.SerializeObject(requestBody);
            var byteData = Encoding.UTF8.GetBytes(serializedRequestBody);

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = await _httpClient.PostAsync(uri, content);
            }

            return await GetResponseDocument(response);
        }

        private static async Task<ResponseDocument> GetResponseDocument(HttpResponseMessage response)
        {
            // Asynchronously get the JSON response.
            var contentString = await response.Content.ReadAsStringAsync();

            // Deserialize the content string.
            var deserializedContent = JsonConvert.DeserializeObject<ResponseRootObject>(contentString);

            var errors = deserializedContent.Errors;
            if (errors.Any())
            {
                errors.ForEach(error => Console.Error.WriteLine($"Error - Id: {error.Id}. Message: {error.Message}"));
            }

            return deserializedContent.Documents.FirstOrDefault();
        }
    }
}