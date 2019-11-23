﻿using Microsoft.Extensions.DependencyInjection;
using Shared.Contracts.Providers;
using Shared.Services.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDefaultEntityProvider<TEntity>(this IServiceCollection services, 
            Action<IDefaultEntityProvider<TEntity>> defaultEntityProviderRegistration = null)
            where TEntity: class
        {
            var defaultEntityProvider = DefaultEntityProvider.Create<TEntity>();
            defaultEntityProviderRegistration?.Invoke(defaultEntityProvider);
            return services.AddSingleton(defaultEntityProvider);
        }
    }
}
