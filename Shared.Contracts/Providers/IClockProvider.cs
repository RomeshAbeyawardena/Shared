using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contracts.Providers
{
    public interface IClockProvider
    {
        DateTimeOffset Now {get;}
        DateTime DateTime {get;}
    }
}
