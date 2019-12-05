using Shared.Contracts;
using Shared.Services;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Shared.Contracts.Factories;
using Shared.Domains;
using MessagePack;
using Shared.Contracts.Providers;
using Shared.Contracts.Services;
using Shared.Services.Builders;
using Microsoft.Extensions.Configuration;
using Shared.Services.Extensions;
using System.Collections.Generic;

namespace Shared.App
{
    public class Startup
    {
        private readonly ISerializerFactory serializerFactory;
        private readonly ICryptographicProvider cryptographicProvider;
        private readonly IEncryptionService encryptionService;
        private readonly IMediator mediator;
        private readonly IEncodingProvider encodingProvider;
        private readonly IConfiguration configuraton;

        private class CustomerEventHandler : DefaultEventHandler<IEvent<Customer>>
        {
            public override async Task<IEvent<Customer>> Push(IEvent<Customer> @event)
            {
                await Task.Delay(10).ConfigureAwait(false);
                Console.WriteLine("Pushing customer {0}", @event.Result.FirstName);
                
                return DefaultEvent.Create(true, result: @event.Result);
            }

            public override async Task<IEvent<Customer>> Send<TCommand>(TCommand command)
            {
                await Task.Delay(10).ConfigureAwait(false);
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
                await Task.Delay(100).ConfigureAwait(false);
                Console.WriteLine("Customer {0} {1} notified", @event.Result.FirstName, @event.Result.LastName);
            }
        }

        public async Task Start()
        {
            var a = DictionaryBuilder.Create<string, object>().Add("Test", true).ToObject();
        }

        
        public Startup(ISerializerFactory serializerFactory, ICryptographicProvider cryptographicProvider, 
            IEncryptionService encryptionService, IMediator mediator, IEncodingProvider encodingProvider,
            IConfiguration configuraton)
        {
            this.serializerFactory = serializerFactory;
            this.cryptographicProvider = cryptographicProvider;
            this.encryptionService = encryptionService;
            this.mediator = mediator;
            this.encodingProvider = encodingProvider;
            this.configuraton = configuraton;
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
        public DateTimeOffset? RegistrationDate { get;set; }
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
        public override IEnumerable<Assembly> GetAssemblies => new [] { DefaultAssembly, Assembly.GetAssembly(typeof(CryptoData)) };
    }
}
