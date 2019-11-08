using Shared.Domains;

namespace Shared.Contracts.Factories
{
    public interface ICacheFactory
    {
        ICacheService GetCacheService(CacheType cacheServiceType);
    }
}
