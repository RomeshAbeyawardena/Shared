﻿using Shared.Contracts;
using Shared.Services;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Shared.Library.Extensions;
using Shared.Contracts.Factories;
using Shared.Domains;
using MessagePack;
using Shared.Contracts.Providers;
using Shared.Contracts.Services;
using Shared.Services.Builders;
using Shared.Library;

namespace Shared.App
{
    public class Startup
    {
        private readonly ISerializerFactory serializerFactory;
        private readonly ICryptographicProvider cryptographicProvider;
        private readonly IEncryptionService encryptionService;
        private readonly IEventHandlerFactory eventHandlerFactory;
        private readonly INotificationHandlerFactory notificationHandlerFactory;

        private class CustomerEventHandler : DefaultEventHandler<IEvent<Customer>>
        {
            public override async Task<IEvent<Customer>> Push(IEvent<Customer> @event)
            {
                await Task.Delay(10);
                Console.WriteLine("Pushing customer {0}", @event.Result.FirstName);
                
                return DefaultEvent.Create(true, result: @event.Result);
            }

            public override async Task<IEvent<Customer>> Send<TCommand>(TCommand command)
            {
                await Task.Delay(10);
                var customer = new Customer {
                FirstName = "John",
                LastName = "Doe"
                };

                Console.WriteLine("Command {0} requested", command.Name);
                return DefaultEvent.Create(customer);
            }
        }


        private class CustomerNotificationSubscriber : DefaultNotificationSubscriber<IEvent<Customer>>
        {
            public override void OnChange(IEvent<Customer> @event)
            {
                Console.WriteLine("Customer {0} {1} notified", @event.Result.FirstName, @event.Result.LastName);
            }

            public override async Task OnChangeAsync(IEvent<Customer> @event)
            {
                await Task.Delay(100);
                Console.WriteLine("Customer {0} {1} notified", @event.Result.FirstName, @event.Result.LastName);
            }
        }

        public async Task Start()
        {
            var @event = DefaultEvent.Create(new Customer {
                FirstName = "John",
                LastName = "Doe"
            });
            await eventHandlerFactory.Push(@event);

            await eventHandlerFactory.Send<IEvent<Customer>, ICommand>(DefaultCommand
                .Create<Customer>("Fetch",  DictionaryBuilder.Create<string, object>().ToDictionary()));

            //notificationHandlerFactory.Subscribe(new CustomerNotificationSubscriber());
            await notificationHandlerFactory.NotifyAsync(@event);
        }

        public async Task<CryptoData> GetCryptoDataFromUserInput(SymmetricAlgorithmType symmetricAlgorithmType)
        {
            RequestPassword:
            Console.Write("Password: ");
            var securePassword = await ConsoleExtensions.SecureRead('*');
            
            if(securePassword.Escaped || securePassword.Length < 7)
                goto RequestPassword;

            var password = securePassword.Value.ToArray();

            RequestMemorialWord:
            Console.Write("OK\r\nMemorable word: ");
            var secureMemorialWord = await ConsoleExtensions.SecureRead('*');

            if(secureMemorialWord.Escaped || secureMemorialWord.Length < 6)
                goto RequestMemorialWord;

            var memorialWord = secureMemorialWord.Value.ToArray();

            var generatedKey = cryptographicProvider.GenerateKey(password, memorialWord, 100000, 32);

            var generatedIV = encryptionService.GenerateIv(symmetricAlgorithmType);

            return new CryptoData(generatedIV, generatedKey);
        }

        public Startup(ISerializerFactory serializerFactory, ICryptographicProvider cryptographicProvider, 
            IEncryptionService encryptionService, IEventHandlerFactory eventHandlerFactory, INotificationHandlerFactory notificationHandlerFactory)
        {
            this.serializerFactory = serializerFactory;
            this.cryptographicProvider = cryptographicProvider;
            this.encryptionService = encryptionService;
            this.eventHandlerFactory = eventHandlerFactory;
            this.notificationHandlerFactory = notificationHandlerFactory;
        }
    }
    [MessagePackObject(true)]
    public class Customer
    {
        public int Id { get; set; }
        public int InitialId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }

        public virtual Initial Initial { get; set; }
    }
    [MessagePackObject(true)]
    public class Initial
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string LongTitle { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }
    }

    public class CryptoData
    {
        public CryptoData(byte[] iV, byte[] key)
        {
            Iv = iV;
            Key = key;
        }

        public byte[] Key { get; }
        public byte[] Iv { get; }
    }

    public class MyServiceBroker : DefaultServiceBroker
    {
        public override Assembly[] GetAssemblies => new [] { DefaultAssembly, Assembly.GetAssembly(typeof(CryptoData)) };
    }
}
