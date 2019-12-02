using Shared.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Library.Extensions
{
    public static class CacheServiceExtensions
    {
        public static async Task<IEnumerable<T>> Append<T>(this ICacheService cacheService, string cacheKeyName, T value)
        {
            var cachedValue = await cacheService.Get<IEnumerable<T>>(cacheKeyName);

            if(cachedValue == null)
                cachedValue = Array.Empty<T>();

            cachedValue  = cachedValue.Append(value);

            return await cacheService.Set(cacheKeyName, cachedValue);
        }
    }
}
