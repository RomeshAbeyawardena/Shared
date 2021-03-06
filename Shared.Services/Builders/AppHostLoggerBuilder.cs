﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Shared.Services.Builders
{
    public class AppHostLoggerBuilder : ILoggingBuilder
    {
        public AppHostLoggerBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}
