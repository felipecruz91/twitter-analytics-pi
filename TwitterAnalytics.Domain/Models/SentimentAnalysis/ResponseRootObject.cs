using System.Collections.Generic;
using Newtonsoft.Json;

namespace TwitterAnalytics.Domain.Models.SentimentAnalysis
{
    public class ResponseRootObject
    {
        [JsonProperty("documents")] public List<ResponseDocument> Documents { get; set; }
        [JsonProperty("errors")] public List<Error> Errors { get; set; }
    }
}