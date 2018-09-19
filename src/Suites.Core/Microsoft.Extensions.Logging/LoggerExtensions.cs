using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Logging
{
    public static class LoggerExtensions
    {
        public static void LogAggregateException<T>(this ILogger<T> logger, AggregateException ae)
        {
            if (ae.InnerExceptions.Count == 1 && !(ae.InnerException is AggregateException))
            {
                logger.LogError(ae.InnerException, default);
                return;
            }

            logger.LogError(ae, default);
        }
    }
}
