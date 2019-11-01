using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services
{
    public static class AppHost
    {
        public static IAppHostBuilder CreateBuilder()
        {
            return new DefaultAppHostBuilder();
        }
    }
}
