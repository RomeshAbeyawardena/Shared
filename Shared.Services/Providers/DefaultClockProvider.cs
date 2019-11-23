using Microsoft.Extensions.Internal;
using Shared.Contracts.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services.Providers
{
    public sealed class DefaultClockProvider : IClockProvider
    {
        private readonly ISystemClock systemClock;

        public DefaultClockProvider(ISystemClock systemClock)
        {
            this.systemClock = systemClock;
        }

        public DateTimeOffset Now => systemClock.UtcNow;

        public DateTime DateTime => Now.UtcDateTime;
    }
}
