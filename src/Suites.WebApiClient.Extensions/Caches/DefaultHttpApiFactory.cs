using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiClient
{
    public sealed class DefaultHttpApiFactory<TInterface>
        : IHttpApiFactory<TInterface>
        where TInterface : class, IHttpApi
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultHttpApiFactory(IServiceProvider provider)
        {
            _serviceProvider = NamedNullException.Assert(provider, nameof(provider));
        }

        public void ConfigureHttpApiConfig(Action<HttpApiConfig> options) 
            => throw new NotImplementedException();

        public TInterface CreateHttpApi()
            => _serviceProvider.GetRequiredService<TInterface>();
    }
}
