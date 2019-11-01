using Shared.Contracts;

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
