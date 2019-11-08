using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using Shared.Contracts;
using Shared.Contracts.Providers;
using Shared.Library;
using Shared.Services.Providers;
using System;
using Microsoft.Extensions.Internal;
using Shared.Contracts.Factories;
using Shared.Services.Factories;
using Shared.Domains;

namespace Shared.Services
{
    public class DefaultServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services, Action<IServiceCollection> registerExternalServices = null)
        {
            registerExternalServices?.Invoke(services);

            services
                .AddSingleton<ISystemClock, SystemClock>()
                .AddSingleton<IMapperProvider, MapperProvider>()
                .AddSingleton<ISerializerFactory, DefaultSerializerFactory>()
                .AddSingleton<IClockProvider, DefaultSystemClockProvider>()
                .AddSingleton<RecyclableMemoryStreamManager>()
                .AddSingleton<IEncryptionService, EncryptionService>()
                .AddSingleton<ICryptographicProvider, DefaultCryptographicProvider>()
                .AddSingleton<ICacheFactory, DefaultCacheFactory>()
                .AddSingleton<IMemoryStreamManager, MemoryStreamManager>()
                .AddSingleton<IRepositoryFactory, DefaultRepositoryFactory>()
                .AddSingleton<IBinarySerializer, BinarySerializer>()
                .AddSingleton<IMessagePackBinarySerializer, MessagePackBinarySerializer>()
                .AddSingleton(DefaultSwitch.Create<SerializerType, Type>()
                    .CaseWhen(SerializerType.Binary, typeof(IBinarySerializer))
                    .CaseWhen(SerializerType.MessagePack, typeof(IMessagePackBinarySerializer)))
                .AddSingleton<IOptions<DefaultCloneOptions>>(new Options<DefaultCloneOptions>(opt =>
                {
                    opt.DefaultCloneType = Domains.CloneType.Deep;
                    opt.UseMessagePack = true;
                }))
                .AddSingleton(typeof(ICloner<>), typeof(DefaultCloner<>));
        }
    }
}
