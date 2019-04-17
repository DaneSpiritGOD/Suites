using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace WebApiClient.Extensions.Caches
{
    public class CacheAdapter : IResponseCacheProvider
    {
        private readonly IMemoryCache _cache;
        public CacheAdapter(IMemoryCache cache)
        {
            _cache = NamedNullException.Assert(cache, nameof(cache));
        }

        public string Name => typeof(IMemoryCache).FullName;

        public Task<ResponseCacheResult> GetAsync(string key)
        {
            if (_cache.TryGetValue<ResponseCacheEntry>(key, out var result))
                return Task.FromResult(new ResponseCacheResult(result, hasValue: true));
            return Task.FromResult(ResponseCacheResult.NoValue);
        }

        public Task SetAsync(string key, ResponseCacheEntry entry, TimeSpan expiration)
        {
            _cache.Set(key, entry,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(expiration));
            return Task.CompletedTask;
        }
    }
}
