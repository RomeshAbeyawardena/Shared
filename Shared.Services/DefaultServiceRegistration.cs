using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using Shared.Contracts;
using Shared.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services
{
    public class DefaultServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services, Action<IServiceCollection> registerExternalServices = null)
        {
            registerExternalServices?.Invoke(services);
            services.AddSingleton<RecyclableMemoryStreamManager>()
                .AddSingleton<IEncryptionService, EncryptionService>()
                .AddSingleton<ICacheFactory, DefaultCacheFactory>()
                .AddSingleton<IMemoryStreamManager, MemoryStreamManager>()
                    .AddSingleton<IRepositoryFactory, RepositoryFactory>()
                    .AddSingleton<IBinarySerializer, BinarySerializer>()
                    .AddSingleton<IMessagePackBinarySerializer, MessagePackBinarySerializer>()
                    .AddSingleton<IOptions<DefaultCloneOptions>>(new Options<DefaultCloneOptions>(opt =>
                    {
                        opt.DefaultCloneType = Domains.CloneType.Deep;
                        opt.UseMessagePack = true;
                    }))
                    .AddSingleton(typeof(ICloner<>), typeof(DefaultCloner<>));
        }
    }
}
