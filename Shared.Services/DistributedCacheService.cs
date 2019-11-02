using Microsoft.Extensions.Caching.Distributed;
using Shared.Contracts;
using System;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class DistributedMemoryCacheService : ICacheService
    {
        private readonly IDistributedCache distributedCache;
        private readonly IMessagePackBinarySerializer messagePackBinarySerializer;

        public async Task<T> Get<T>(string cacheKeyName) where T : class
        {
            var cachedResult = await distributedCache.GetAsync(cacheKeyName);

            if(cachedResult == null || cachedResult.Length == 0)
                return default;

            return messagePackBinarySerializer.Deserialize<T>(cachedResult);
        }

        public async Task<T> Set<T>(string cacheKeyName, T value) where T : class
        {
            var data = Array.Empty<byte>();
            
            if(value != null)
                messagePackBinarySerializer.Serialize(value);

             await distributedCache.SetAsync(cacheKeyName, data);

            return value;
        }

        public async Task RemoveAsync(string key)
        {
            await distributedCache.RemoveAsync(key);
        }

        public DistributedMemoryCacheService(IDistributedCache distributedCache, 
            IMessagePackBinarySerializer messagePackBinarySerializer)
        {
            this.distributedCache = distributedCache;
            this.messagePackBinarySerializer = messagePackBinarySerializer;
        }
    }
}
