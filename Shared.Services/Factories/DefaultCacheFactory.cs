﻿using Shared.Contracts;
using Shared.Contracts.Factories;
using Shared.Contracts.Services;
using Shared.Domains;
using Shared.Library.Extensions;
using System;

namespace Shared.Services.Factories
{
    public class DefaultCacheFactory : ICacheFactory
    {
        public ICacheService GetCacheService(CacheType cacheServiceType)
        {
            return (ICacheService)serviceProvider.Resolve(_cacheServiceSwitch.Case(cacheServiceType));
        }

        ICacheService ICacheFactory.GetCacheService(CacheType cacheServiceType)
        {
            throw new NotImplementedException();
        }

        public DefaultCacheFactory(IServiceProvider serviceProvider, ISwitch<CacheType, Type> cacheServiceSwitch)
        {
            this.serviceProvider = serviceProvider;
            _cacheServiceSwitch = cacheServiceSwitch;
        }

        private readonly ISwitch<CacheType, Type> _cacheServiceSwitch;
        private readonly IServiceProvider serviceProvider;
    }
}
