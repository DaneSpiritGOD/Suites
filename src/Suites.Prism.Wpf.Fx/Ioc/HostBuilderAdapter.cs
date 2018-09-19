using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection.Prism
{
    public class HostBuilderAdapter : IHostBuilderAdapter
    {
        private readonly IHostBuilder _hostBuilder;

        public IHost BuildedHost { get; private set; }
        public HostBuilderAdapter(IHostBuilder hostBuilder)
        {
            _hostBuilder = NamedNullException.Assert(hostBuilder, nameof(hostBuilder));
        }

        public event EventHandler<IHost> HostBuilded;
        private void onBuildHost(IHost host)
        {
            HostBuilded?.Invoke(this, host);
        }

        #region IContainerExtension
        public bool SupportsModules => true;

        public void FinalizeExtension()
        {
            BuildedHost = _hostBuilder.Build();
            onBuildHost(BuildedHost);
        }

        public void Register(Type from, Type to)
        {
            _hostBuilder.ConfigureServices(services =>
            {
                services.AddTransient(from, to);
            });
        }

        [Obsolete("'name' is Not recommend within Microsoft.Extensions.DependencyInjection.")]
        public void Register(Type from, Type to, string name)
        {
            throw new NotImplementedException();
        }

        public void RegisterInstance(Type type, object instance)
        {
            _hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton(type, instance);
            });
        }

        public void RegisterSingleton(Type from, Type to)
        {
            _hostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton(from, to);
            });
        }

        public object Resolve(Type type)
        {
            NamedNullException.Assert(BuildedHost, nameof(BuildedHost));
            return BuildedHost.Services.GetRequiredService(type);
        }

        [Obsolete("'name' is Not recommend within Microsoft.Extensions.DependencyInjection.")]
        public object Resolve(Type type, string name)
        {
            throw new NotImplementedException();
        }

        public object ResolveViewModelForView(object view, Type viewModelType)
        {
            NamedNullException.Assert(BuildedHost, nameof(BuildedHost));
            return BuildedHost.Services.GetService(viewModelType);
        }
        #endregion IContainerExtension

        #region IHostBuilder
        public IDictionary<object, object> Properties => _hostBuilder.Properties;
        public IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            return _hostBuilder.ConfigureHostConfiguration(configureDelegate);
        }

        public IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            return _hostBuilder.ConfigureAppConfiguration(configureDelegate);
        }

        public IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            return _hostBuilder.ConfigureServices(configureDelegate);
        }

        public IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
        {
            return _hostBuilder.UseServiceProviderFactory(factory);
        }

        public IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        {
            return _hostBuilder.ConfigureContainer(configureDelegate);
        }

        [Obsolete("build method is invoked in the 'FinalizeExtension' method.")]
        public IHost Build()
        {
            throw new NotSupportedException("build method is invoked in the 'FinalizeExtension' method.");
        }
        #endregion IHostBuilder
    }
}
