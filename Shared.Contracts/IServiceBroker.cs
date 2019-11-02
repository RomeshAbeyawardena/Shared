﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Contracts
{
    public interface IServiceBroker
    {
        Assembly[] GetAssemblies { get; }
        void RegisterServiceAssemblies(IServiceCollection services, params Assembly[] assemblies);
    }
}
