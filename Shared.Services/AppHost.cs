using Shared.Contracts.Builders;
using Shared.Services.Builders;

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
