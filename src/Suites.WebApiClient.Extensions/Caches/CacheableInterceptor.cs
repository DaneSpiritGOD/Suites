using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using WebApiClient.Defaults;

namespace WebApiClient.Extensions.Caches
{
    public class CacheableInterceptor : ApiInterceptor
    {
        public const string ApiConfigTagCacheKey = nameof(ApiConfigTagCacheKey);
        public CacheableInterceptor(HttpApiConfig httpApiConfig)
            : base(httpApiConfig)
        {
        }

        public override object Intercept(object target, MethodInfo method, object[] parameters)
        {
            var cache = HttpApiConfig.Tags.Get(ApiConfigTagCacheKey).As<IMemoryCache>();
            if (cache == default)
                return getUpdatedResult();

            var attr = (CacheAttribute)GetApiActionDescriptor(method, parameters)
                .Attributes.FirstOrDefault(x => x.GetType() == typeof(CacheAttribute));
            if (attr == default)
                return getUpdatedResult();

            var key = calcResultKey();
            if (cache.TryGetValue<Task>(key, out var result) &&
                !result.IsCanceled && !result.IsFaulted)
                return result;

            return refreshCache(cache, key, attr.SecAge);

            string calcResultKey()
            {
                return $"{HttpApiConfig.HttpHost}.{method.Name}.{JsonConvert.SerializeObject(parameters)}";
                //{string.Join(".", parameters.Select(x => x?.ToString()).ToArray())}
            }

            object getUpdatedResult() => base.Intercept(target, method, parameters);

            object refreshCache(IMemoryCache innerCache, string innerKey, TimeSpan age)
            {
                var updated = getUpdatedResult();
                if (updated.GetType().IsSubclassOf(typeof(Task)))
                {
                    innerCache.Set(innerKey, updated,
                        new MemoryCacheEntryOptions().SetAbsoluteExpiration(age));
                }
                return updated;
            }
        }
    }
}
