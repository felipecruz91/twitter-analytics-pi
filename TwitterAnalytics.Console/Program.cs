using System;
using InfluxDB.Collector.Diagnostics;
using TwitterAnalytics.BusinessLogic;
using TwitterAnalytics.DataAccess;

namespace TwitterAnalytics.Console
{
    internal class Program
    {
        private const string Db = "TwitterAnalytics";

        private static void Main(string[] args)
        {
            System.Console.WriteLine($"[{DateTime.Now}] - Starting program...");

            CollectorLog.RegisterErrorHandler((message, exception) =>
            {
                System.Console.Error.WriteLine($"[{DateTime.Now}] - {message}: {exception}");
            });

            try
            {
                var influxDbServerAddress = Environment.GetEnvironmentVariable("influxdb-server-addr");
                if (string.IsNullOrEmpty(influxDbServerAddress))
                {
                    throw new Exception(
                        "Influx DB server address not set. Set the env. variable 'influxdb-server-addr' following the format 'http://IP_ADDRESS:PORT'.");
                }

                var repository = new TweetsRepository(influxDbServerAddress, Db, TimeSpan.FromSeconds(2));
                System.Console.WriteLine(
                    $"[{DateTime.Now}] - Established connection with Influx DB server at '{influxDbServerAddress}'.");

                var streamFactory = new StreamFactory(new TweetProcessor(repository));
                var trackingWord = Environment.GetEnvironmentVariable("trackingWord");
                if (string.IsNullOrEmpty(trackingWord))
                {
                    throw new Exception("Tracking word not set. Set the env. variable 'trackingWord'.");
                }

                streamFactory.StartStream(trackingWord);

                System.Console.WriteLine(
                    $"[{DateTime.Now}] - Started listening for tweets that contains the keyword '{trackingWord}'...");
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine($"[{DateTime.Now}] - Exception: {e.Message}");
            }
        }
    }
}