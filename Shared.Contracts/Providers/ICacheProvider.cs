using Shared.Contracts.Services;
using Shared.Domains;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts.Providers
{
    public interface ICacheProvider
    {
        T GetOrDefault<T>(string cacheName, Func<T> value, CacheType cacheType = CacheType.DistributedCache)
            where T: class;
        Task<T> GetOrDefaultAsync<T>(string cacheName, Func<Task<T>> value, CacheType cacheType = CacheType.DistributedCache)
            where T: class;
        ICacheService GetCacheService(CacheType cacheType = CacheType.DistributedCache);
    }
}
