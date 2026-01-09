using Blog.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services
{
    public class MemoryCacheService(IMemoryCache cache) : ICacheService
    {
     
        private readonly ConcurrentDictionary<string, byte> _cacheKeys = new ConcurrentDictionary<string, byte>();
        private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);

        public async Task<T?> GetOrCreateAsync<T>(string key, Func<Task<T?>> factory, TimeSpan? expiration = null,CancellationToken cancellationToken = default) where T : class
        {
            if (cache.TryGetValue(key, out T? cachedValue))
            {            
                return cachedValue;
            }
        
            var value = await factory();

            if (value != null)
            {
                await SetAsync(key, value, expiration, cancellationToken);
            }

            return value;
        }

        public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            if (cache.TryGetValue(key, out T? cachedValue))
            {
               
                return Task.FromResult(cachedValue);
            }

            return Task.FromResult<T?>(null);
        }

        public Task SetAsync<T>(string key,T value,TimeSpan? expiration = null, CancellationToken cancellationToken = default) where T : class
        {
            var cacheExpiration = expiration ?? DefaultExpiration;

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(cacheExpiration)
                .RegisterPostEvictionCallback((evictedKey, evictedValue, reason, state) =>
                {
                    _cacheKeys.TryRemove(evictedKey.ToString()!, out _);
                
                });

            cache.Set(key, value, cacheEntryOptions);
            _cacheKeys.TryAdd(key, 0);

            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            cache.Remove(key);
            _cacheKeys.TryRemove(key, out _);
            return Task.CompletedTask;
        }

        public Task RemoveByPatternAsync(string pattern, CancellationToken cancellationToken = default)
        {
            var keysToRemove = _cacheKeys.Keys
                .Where(k => k.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var key in keysToRemove)
            {
                cache.Remove(key);
                _cacheKeys.TryRemove(key, out _);
            }
            return Task.CompletedTask;
        }

        public Task RemoveMultipleAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
        {
            foreach (var key in keys)
            {
                cache.Remove(key);
                _cacheKeys.TryRemove(key, out _);
            }
            return Task.CompletedTask;
        }
    }
}
