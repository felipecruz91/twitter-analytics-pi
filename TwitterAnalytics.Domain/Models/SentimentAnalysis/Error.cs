using Newtonsoft.Json;

namespace TwitterAnalytics.Domain.Models.SentimentAnalysis
{
    public class Error
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("message")] public string Message { get; set; }

    }
}