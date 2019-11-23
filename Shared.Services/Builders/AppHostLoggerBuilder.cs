using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

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
