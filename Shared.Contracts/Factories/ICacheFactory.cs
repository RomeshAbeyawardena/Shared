using Shared.Contracts.Services;
using Shared.Domains.Enumerations;

namespace Shared.Contracts.Factories
{
    public interface ICacheFactory
    {
        ICacheService GetCacheService(CacheType cacheServiceType);
    }
}
