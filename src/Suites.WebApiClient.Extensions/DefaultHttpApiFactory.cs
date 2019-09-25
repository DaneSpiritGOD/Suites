using System;
using System.Net.Http;
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

        public Type InterfaceType => typeof(TInterface);

        [Obsolete("不需要使用该方法！")]
        public void ConfigureHttpApiConfig(Action<HttpApiConfig> options)
            => throw new NotImplementedException("不需要使用该方法！");

        [Obsolete("不需要使用该方法！")]
        public void ConfigureHttpMessageHandler(Func<HttpMessageHandler> factory)
            => throw new NotImplementedException("不需要使用该方法！");

        public TInterface CreateHttpApi()
            => _serviceProvider.GetRequiredService<TInterface>();

        [Obsolete("不需要使用该方法！")]
        HttpApi IHttpApiFactory.CreateHttpApi()
            => throw new NotImplementedException("不需要使用该方法！");
    }
}
