using Shared.Contracts;
using System;
using System.Threading.Tasks;

namespace Shared.Services
{
    internal class DefaultAppHost : IAppHost
    {
        private readonly Type startupType;
        private IServiceProvider serviceProvider;

        protected object StartupService => serviceProvider.GetService(startupType);

        public DefaultAppHost(Type startupType, IServiceProvider serviceProvider)
        {
            this.startupType = startupType;
            this.serviceProvider = serviceProvider;
        }

        public object Run(string methodName)
        {
            var method = startupType.GetMethod(methodName);
            return method.Invoke(StartupService, Array.Empty<object>());
        }

        public async Task RunAsync(string methodName)
        {
            var runMethod = Run(methodName) as Task;
            await runMethod;
        }

        public async Task<T> RunAsync<T>(string methodName)
        {
            var runMethod = Run(methodName) as Task<T>;
            return await runMethod;
        }
    }

    internal class DefaultAppHost<TStartup> : DefaultAppHost, IAppHost<TStartup>
    {
        protected new TStartup StartupService => (TStartup)base.StartupService;

        public DefaultAppHost(IServiceProvider serviceProvider) 
            : base(typeof(TStartup), serviceProvider)
        {

        }

        public object Run(Func<TStartup, object> getMember)
        {
            return getMember(StartupService);
        }

        public async Task RunAsync(Func<TStartup, Task> getMemberTask)
        {
            await getMemberTask(StartupService);
        }

        public async Task<T> RunAsync<T>(Func<TStartup, Task<T>> getMemberTask)
        {
            return await getMemberTask(StartupService);
        }
    }
}
