using System.Collections.Generic;
using Newtonsoft.Json;

namespace TwitterAnalytics.Domain.Models.SentimentAnalysis
{
    public class RequestRootObject
    {
        [JsonProperty("documents")] public List<RequestDocument> Documents { get; set; }
    }
}