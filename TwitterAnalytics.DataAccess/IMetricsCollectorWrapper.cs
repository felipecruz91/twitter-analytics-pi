using System.Collections.Generic;

namespace TwitterAnalytics.DataAccess
{
    public interface IMetricsCollectorWrapper
    {
        void Write(string measurement, IReadOnlyDictionary<string, object> fields);
    }
}