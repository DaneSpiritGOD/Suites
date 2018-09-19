using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Prism;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    public static class HostExtensions
    {
        public static IConfiguration GetConfiguration(this IHost host) => host.Services.GetRequiredService<IConfiguration>();

        public static ILogger<T> GetLogger<T>(this IHost host) =>
            host.Services
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger<T>();
    }
}
