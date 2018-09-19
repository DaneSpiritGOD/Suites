using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    public static class WpfHostBuilderExtensions
    {
        public static IHostBuilder UseWpfLifetime(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureServices(delegate (HostBuilderContext context, IServiceCollection collection)
            {
                collection.AddSingleton<IHostLifetime, WpfLifetime>();
            });
        }        
    }
}
