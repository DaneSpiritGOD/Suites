using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace WebApiClient.Extensions.Caches
{
    public interface ICacheProvider : IResponseCacheProvider
    {
        void RemoveKeyHeadWith(string keyHead);
        void Remove(string key);
    }

    public class CacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _cache;
        private readonly ConcurrentDictionary<string, string> _keys;

        public CacheProvider(IMemoryCache cache)
        {
            _cache = NamedNullException.Assert(cache, nameof(cache));
            _keys = new ConcurrentDictionary<string, string>();
        }

        public string Name => typeof(IMemoryCache).FullName;

        public Task<ResponseCacheResult> GetAsync(string key)
        {
            if (_cache.TryGetValue<ResponseCacheEntry>(key, out var result))
            {
                return Task.FromResult(new ResponseCacheResult(result, hasValue: true));
            }

            return Task.FromResult(ResponseCacheResult.NoValue);
        }

        public Task SetAsync(string key, ResponseCacheEntry entry, TimeSpan expiration)
        {
            _cache.Set(key, entry,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(expiration));
            _keys.TryAdd(key, key);
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
            _keys.TryRemove(key, out _);
        }

        public void RemoveKeyHeadWith(string keyHead)
        {
            var keys = _keys.Keys
                .Where(x => x.StartsWith(keyHead, StringComparison.Ordinal))
                .ToArray();

            foreach (var key in keys)
            {
                Remove(key);
            }
        }
    }
}
