using Microsoft.Extensions.Hosting;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection.Prism
{
    public interface IHostBuilderAdapter : IContainerExtension, IHostBuilder
    {
        IHost BuildedHost { get; }
        event EventHandler<IHost> HostBuilded;
    }
}
