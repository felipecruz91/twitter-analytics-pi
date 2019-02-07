using Microsoft.Extensions.Configuration;
using TwitterAnalytics.BusinessLogic;

namespace TwitterAnalytics.Console
{
    internal class TextAnalyticsConfiguration : ITextAnalyticsConfiguration
    {
        private readonly IConfigurationRoot _configuration;

        public TextAnalyticsConfiguration(IConfigurationRoot configuration)
        {
            _configuration = configuration;
        }

        public string Name => _configuration["TextAnalytics:Name"];
        public string Key1 => _configuration["TextAnalytics:Key1"];
        public string Key2 => _configuration["TextAnalytics:Key2"];
    }
}