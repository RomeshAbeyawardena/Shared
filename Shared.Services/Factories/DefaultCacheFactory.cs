﻿using Shared.Contracts;
using Shared.Domains;
using Shared.Library.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services
{
    public class DefaultCacheFactory : ICacheFactory
    {
        public ICacheService GetCacheService(CacheType cacheServiceType)
        {
            return (ICacheService)serviceProvider.Resolve(_cacheServiceSwitch.Case(cacheServiceType));
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