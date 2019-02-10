using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Tweetinvi.Models;
using TwitterAnalytics.BusinessLogic;
using TwitterAnalytics.DataAccess;

namespace TwitterAnalytics.Console
{
    internal class Program
    {
        private const string Db = "TwitterAnalytics";
        private static IConfigurationRoot _configuration;
        private static readonly HttpClient HttpClient = new HttpClient();

        private static void Main(string[] args)
        {
            System.Console.WriteLine($"[{DateTime.Now}] - Starting program...");

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            System.Console.WriteLine($"[{DateTime.Now}] - Environment: {environmentName}");

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environmentName}.json", true, true);

            _configuration = builder.Build();

            try
            {
                // Set the InfluxDB metrics collector
                var influxDbServerAddress = GetInfluxDbServerAddress();
                IMetricsCollectorWrapper metricsCollectorWrapper = new MetricsCollectorWrapper(influxDbServerAddress, Db, TimeSpan.FromSeconds(2));
                var repository = new TweetsRepository(metricsCollectorWrapper);

                // Start the Twitter Stream
                var credentials = GetTwitterCredentials();
                var keys = new TextAnalyticsConfiguration(_configuration);
                var streamFactory = new StreamFactory(new TweetProcessor(repository, keys, HttpClient), credentials, null);
                var keyword = GetKeyword();
                streamFactory.StartStream(keyword);
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine($"[{DateTime.Now}] - Exception: {e.Message}");
            }
        }

        private static string GetKeyword()
        {
            var keyword = Environment.GetEnvironmentVariable("keyword");
            if (string.IsNullOrEmpty(keyword))
            {
                keyword = _configuration["Twitter:Keyword"];
                if (string.IsNullOrEmpty(keyword))
                {
                    throw new Exception("Tracking word not set. Set the env. variable 'keyword'.");
                }
            }

            return keyword;
        }

        /// <summary>
        ///     Get the Influx DB Server address from an environment variable.
        /// </summary>
        /// <returns>
        ///     The Influx DB server address in the format:
        ///     http://<YOUR_INFLUX_DB_IP_ADDR>:<INFLUX_DB_PORT>
        /// </returns>
        private static string GetInfluxDbServerAddress()
        {
            var influxDbServerAddress = _configuration["InfluxDB:ServerAddress"];
            if (string.IsNullOrEmpty(influxDbServerAddress))
            {
                throw new Exception(
                    "Influx DB server address not set. Set the env. variable 'influxdb-server-addr' following the format 'http://IP_ADDRESS:PORT'.");
            }

            return influxDbServerAddress;
        }

        /// <summary>
        ///     Get the Twitter credentials from environment variables.
        ///     Set up your own credentials at https://apps.twitter.com.
        /// </summary>
        /// <returns></returns>
        private static ITwitterCredentials GetTwitterCredentials()
        {
            var consumerKey = _configuration["Twitter:ConsumerKey"];
            var consumerSecret = _configuration["Twitter:ConsumerSecret"];
            var accessToken = _configuration["Twitter:AccessToken"];
            var accessTokenSecret = _configuration["Twitter:AccessTokenSecret"];

            return new TwitterCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);
        }
    }
}