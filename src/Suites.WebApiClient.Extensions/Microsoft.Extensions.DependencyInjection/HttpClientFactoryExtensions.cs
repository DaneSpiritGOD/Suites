//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Text;
//using WebApiClient;

//namespace Microsoft.Extensions.DependencyInjection
//{
//    public static class HttpClientFactoryExtensions
//    {
//        public static IHttpClientBuilder AddHttpApiTypedClient<TInterface>(
//            this IServiceCollection services,
//            Action<IServiceProvider, HttpClient> configureClient,
//            Func<HttpApiConfig, IServiceProvider, IApiInterceptor> interceptorFactory)
//            where TInterface : class, IHttpApi
//        {
//            NamedNullException.Assert(interceptorFactory, nameof(interceptorFactory));
//            return services.AddHttpClient<TInterface>(configureClient)
//                .AddTypedClient(
//                (hc, s) =>
//               {
//                   var val = new HttpApiConfig(hc);
//                   return HttpApiClient.Create(typeof(TInterface), interceptorFactory(val, s)) as TInterface;
//               });
//        }
//    }
//}
