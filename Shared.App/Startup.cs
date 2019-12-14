using Shared.Contracts;
using Shared.Services;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Shared.Contracts.Factories;
using MessagePack;
using Shared.Contracts.Providers;
using Shared.Contracts.Services;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Shared.Services.DapperExtensions;
using System.Data;
using Shared.Contracts.DapperExtensions;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Library.Attributes;
using System.Text;
using Shared.Domains.Enumerations;

namespace Shared.App
{
    public class Startup
    {
        private readonly ISerializerFactory serializerFactory;
        private readonly ICryptographicProvider cryptographicProvider;
        private readonly IEncryptionService encryptionService;
        private readonly IMediator mediator;
        private readonly IEncodingProvider encodingProvider;
        private readonly IDomainEncryptionProvider domainEncryptionProvider;
        private readonly IConfiguration configuraton;

        public class CryptographicInfo : ICryptographicInfo
        {
            public CryptographicInfo(SymmetricAlgorithmType symmetricAlgorithmType, IEnumerable<byte> key, 
                IEnumerable<byte> salt, IEnumerable<byte> initialVector, int iterations)
            {
                SymmetricAlgorithmType = symmetricAlgorithmType;
                Key = key;
                Salt = salt;
                InitialVector = initialVector;
                Iterations = iterations;
            }

            public SymmetricAlgorithmType SymmetricAlgorithmType { get; }
            public IEnumerable<byte> Key { get; }
            public IEnumerable<byte> Salt { get; }
            public IEnumerable<byte> InitialVector { get; }
            public int Iterations { get; }
        }

        public async Task Start()
        {
            var cryptographicInfo = new CryptographicInfo(SymmetricAlgorithmType.Aes,
                                        Guid.NewGuid().ToString().GetBytes(Encoding.ASCII),
                                        Guid.NewGuid().ToString().GetBytes(Encoding.ASCII),
                                        "abb6526e130b4a24".GetBytes(Encoding.ASCII), 100000);
            

            var encryptedCustomer = await domainEncryptionProvider.Encrypt<CustomerDto, Customer>(new CustomerDto { 
                EmailAddress = "sarah.catlin@hotmail.com",
                FirstName = "Sarah",
                MiddleName = "Middleton",
                LastName = "Catlin",
                DateOfBirth = new DateTime(2019, 09, 11)
                }, cryptographicInfo).ConfigureAwait(false);

            var decryptedCustomer = await domainEncryptionProvider.Decrypt<Customer, CustomerDto>(encryptedCustomer, cryptographicInfo).ConfigureAwait(false);
        }

        
        public Startup(ISerializerFactory serializerFactory, ICryptographicProvider cryptographicProvider, 
            IEncryptionService encryptionService, IMediator mediator, IEncodingProvider encodingProvider,
            IDomainEncryptionProvider domainEncryptionProvider,
            IConfiguration configuraton)
        {
            this.serializerFactory = serializerFactory;
            this.cryptographicProvider = cryptographicProvider;
            this.encryptionService = encryptionService;
            this.mediator = mediator;
            this.encodingProvider = encodingProvider;
            this.domainEncryptionProvider = domainEncryptionProvider;
            this.configuraton = configuraton;
        }
    }

    public class CustomerDapperContext : DapperContext
    {
        public CustomerDapperContext(IFormatProvider formatProvider, IDbConnection dbConnection)
            : base(formatProvider, dbConnection)
        {

        }

        public IMapping<Customer> Customers { get; set; }
    }

    public class CustomerValidator : DefaultBaseValidator<Customer>
    {
        public CustomerValidator(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {

        }

        protected override void OnValidate(Customer model)
        {
            ValidateModel(model)
                .IsNotNull(member => member.EmailAddress)
                .IsNotNull(member => member.FirstName)
                .IsValid(member => member.DateOfBirth, DateTime.Now.AddYears(-11), (member, compare) => member < compare)
                .IsInRange(member => member.RegistrationDate, DateTime.Now.AddYears(-5), DateTime.Now, 
                    (member, minimumValue, maximimValue) => member >= minimumValue && member <= maximimValue);
        }

        protected override async Task OnValidateAsync(Customer model)
        {
            await ValidateModel(model)
                .IsDuplicateEntryAsync(a => a.FirstName, async(serviceProvider, member) => { return true; })
                .ConfigureAwait(false);
        }
    }

    [MessagePackObject(true)]
    [Table("Customer")]
    public class Customer
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }
        public int TitleId { get; set; }

        [Encryptable]
        public IEnumerable<byte> EmailAddress { get; set; }
        [Encryptable]
        public IEnumerable<byte> FirstName { get; set; }
        [Encryptable]
        public IEnumerable<byte> MiddleName { get; set; }
        [Encryptable]
        public IEnumerable<byte> LastName { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }
        public DateTimeOffset? RegistrationDate { get;set; }
        
        [EncryptionKey]
        public IEnumerable<byte> Key { get; set; }
        
    }

     public class CustomerDto
    {
        
        public int Id { get; set; }
        public int TitleId { get; set; }
        [Encryptable]
        public string EmailAddress { get; set; }

        [Encryptable]
        public string FirstName { get; set; }

        [Encryptable]
        public string MiddleName { get; set; }

        [Encryptable]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }
        public DateTimeOffset? RegistrationDate { get;set; }

        [NotMapped]
        public virtual Title Title { get; set; }
    }

    [MessagePackObject(true)]
    public class Title
    {
        public int Id { get; set; }
        public string Name { get; set; }
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
