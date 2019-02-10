using System;
using System.Collections.Generic;
using InfluxDB.Collector;
using InfluxDB.Collector.Diagnostics;

namespace TwitterAnalytics.DataAccess
{
    public class MetricsCollectorWrapper : IMetricsCollectorWrapper
    {
        public MetricsCollector Collector { get; set; }

        public MetricsCollectorWrapper(string serverBaseAddress, string database, TimeSpan interval)
        {
            Collector = new CollectorConfiguration()
                .Batch.AtInterval(interval)
                .WriteTo.InfluxDB(serverBaseAddress, database)
                .CreateCollector();

            CollectorLog.RegisterErrorHandler((message, exception) =>
            {
                Console.Error.WriteLine($"[{DateTime.Now}] - {message}: {exception}");
            });
        }


        public void Write(string measurement, IReadOnlyDictionary<string, object> fields)
        {
            Collector.Write(measurement, fields);
        }
    }
}