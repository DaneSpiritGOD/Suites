using CommonServiceLocator;
using Microsoft.Extensions.Hosting;
using Prism;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Microsoft.Extensions.DependencyInjection.Prism
{
    public class PrismApplication<TMainWindow> : PrismApplicationBase
        where TMainWindow : Window
    {
        protected IHostBuilderAdapter HostBuilder { get; }

        private IHost BuildedHost => NamedNullException.Assert(HostBuilder.BuildedHost, nameof(BuildedHost));

        public PrismApplication(IHostBuilderAdapter hostBuilder)
        {
            HostBuilder = NamedNullException.Assert(hostBuilder, nameof(hostBuilder));
        }

        protected override IContainerExtension CreateContainerExtension()
        {
            return HostBuilder;
        }

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
            HostBuilder.ConfigureServices(services =>
            {
                services.AddSingleton<IRegionNavigationContentLoader, MEDIRegionNavigationContentLoader>();
                services.AddSingleton<IServiceLocator, MEDIServiceLocatorAdapter>();
                services.AddTransient<SelectorRegionAdapter>();
                services.AddTransient<ItemsControlRegionAdapter>();
                services.AddTransient<ContentControlRegionAdapter>();

                services.AddSingleton<Application>(this);
                services.AddSingleton(Dispatcher);
            });
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry) { }

        protected override Window CreateShell()
        {
            return BuildedHost.Services.GetRequiredService<TMainWindow>();
        }

        protected override void OnInitialized()
        {
            BuildedHost.Start();
            base.OnInitialized();
        }
    }
}
