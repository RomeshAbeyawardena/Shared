﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.IO;
using Shared.Contracts;
using Shared.Contracts.Providers;
using Shared.Services.Providers;
using System;
using Microsoft.Extensions.Internal;
using Shared.Contracts.Factories;
using Shared.Services.Factories;
using Shared.Domains;
using System.Security.Cryptography;
using Shared.Contracts.Services;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services
{
    public class DefaultServiceRegistration : IServiceRegistration
    {
        public void RegisterServices(IServiceCollection services)
        {
            services
                .AddSingleton(DefaultSwitch.Create<string, Encoding>()
                    .CaseWhen("ASCII", Encoding.ASCII)
                    .CaseWhen("Unicode", Encoding.Unicode)
                    .CaseWhen("UTF7", Encoding.UTF7, "UTF-7")
                    .CaseWhen("UTF8", Encoding.UTF8, "UTF-8")
                    .CaseWhen("UTF32", Encoding.UTF32, "UTF-32")
                    .CaseWhen("BigEndianUnicode", Encoding.BigEndianUnicode, "BE-Unicode"))
                .AddSingleton<IEncodingProvider, DefaultEncodingProvider>()
                .AddSingleton<IMediator, DefaultMediator>()
                .AddSingleton<IEventHandlerFactory, DefaultEventHandlerFactory>()
                .AddSingleton<INotificationHandlerFactory, DefaultNotificationHandlerFactory>()
                .AddSingleton(typeof(INotificationHandler<>), typeof(DefaultNotificationHandler<>))
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
                .AddSingleton<ISymmetricAlgorithmFactory, DefaultSymmetricAlgorithmFactory>()
                .AddSingleton<IMessagePackBinarySerializer, MessagePackBinarySerializer>()
                .AddSingleton<AesCryptoServiceProvider>()
                .AddSingleton<RSACryptoServiceProvider>()
                .AddSingleton<TripleDESCryptoServiceProvider>()
                .AddSingleton<RNGCryptoServiceProvider>()
                
                .AddSingleton(DefaultSwitch.Create<SymmetricAlgorithmType, Type>()
                    .CaseWhen(SymmetricAlgorithmType.Aes, typeof(AesCryptoServiceProvider))
                    .CaseWhen(SymmetricAlgorithmType.Rsa, typeof(RSACryptoServiceProvider))
                    .CaseWhen(SymmetricAlgorithmType.TripleDES, typeof(TripleDESCryptoServiceProvider))
                    .CaseWhen(SymmetricAlgorithmType.Rng, typeof(RNGCryptoServiceProvider)))
                .AddSingleton(DefaultSwitch.Create<SerializerType, Type>()
                    .CaseWhen(SerializerType.Binary, typeof(IBinarySerializer))
                    .CaseWhen(SerializerType.MessagePack, typeof(IMessagePackBinarySerializer)))
                .AddSingleton(typeof(ICloner<>), typeof(DefaultCloner<>));
        }
    }
}
