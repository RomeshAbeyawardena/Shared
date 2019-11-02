using Shared.Domains;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contracts
{
    public interface ICacheFactory
    {
        ICacheService GetCacheService(CacheType cacheServiceType);
    }
}
