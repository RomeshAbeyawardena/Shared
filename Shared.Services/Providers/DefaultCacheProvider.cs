using Shared.Contracts.Factories;
using Shared.Contracts.Providers;
using Shared.Contracts.Services;
using System;
using System.Threading.Tasks;
using Shared.Domains.Enumerations;

namespace Shared.Services.Providers
{
    public class DefaultCacheProvider : ICacheProvider
    {
        private readonly ICacheFactory _cacheFactory;

        public T GetOrDefault<T>(string cacheName, Func<T> value, CacheType cacheType = CacheType.DistributedCache)
            where T: class
        {
            return GetOrDefaultAsync<T>(cacheName, async() => await Task.FromResult(value()), cacheType).Result;
        }

        public async Task<T> GetOrDefaultAsync<T>(string cacheName, Func<Task<T>> value, CacheType cacheType = CacheType.DistributedCache)
            where T: class
        {
            var cacheService =  GetCacheService(cacheType);
            var result = await cacheService.Get<T>(cacheName);

            if(result != null)
                return result;

            result = await value();

            return await cacheService.Set(cacheName, result);
        }

        public ICacheService GetCacheService(CacheType cacheType) => _cacheFactory.GetCacheService(cacheType);

        public DefaultCacheProvider(ICacheFactory cacheFactory)
        {
            _cacheFactory = cacheFactory;
        }
    }
}
