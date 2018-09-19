using Microsoft.Extensions.DependencyInjection.Prism;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Hosting
{
    public static class PrismWpfHostBuilderExtensions
    {
        public static TApplication UseWpf<TApplication>(this IHostBuilder hostBuilder) where TApplication : class
        {
            hostBuilder.UseWpfLifetime();
            var adapter = new HostBuilderAdapter(hostBuilder);
            return Activator.CreateInstance(typeof(TApplication), adapter) as TApplication;
        }
    }
}
