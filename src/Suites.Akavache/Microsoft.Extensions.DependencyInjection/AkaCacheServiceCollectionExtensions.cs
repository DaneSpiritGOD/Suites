using System;
using System.Collections.Generic;
using System.Text;
using SQLitePCL;
using Suites.Akavache;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AkaCacheServiceCollectionExtensions
    {
        static AkaCacheServiceCollectionExtensions()
        {
            Batteries_V2.Init();
        }

        public static IServiceCollection AddAkaCache(this IServiceCollection services)
        {
            NamedNullException.Assert(services, nameof(services));
            services.AddSingleton<IAkavacheManager, DefaultAkavacheManager>();
            return services;
        }

        public static IServiceCollection AddAkaCache(this IServiceCollection services, params AkaCacheOptions[] options)
        {
            NamedNullException.Assert(services, nameof(services));
            services.AddSingleton<IAkavacheManager>(_ =>
            {
                var mgr = new DefaultAkavacheManager();
                for (var index = 0; index < options.Length; ++index)
                {
                    mgr.AddPersistenceMedia(options[index]);
                }
                return mgr;
            });
            return services;
        }
    }
}
