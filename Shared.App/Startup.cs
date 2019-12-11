﻿using Shared.Contracts;
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
using Shared.Services.DapperExtensions;
using System.Data;
using Shared.Contracts.DapperExtensions;
using System.Globalization;
using System.Data.Common;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.Library.Attributes;
using System.Text;

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

        
        public async Task Start()
        {
            var key = Guid.NewGuid().ToString().GetBytes(Encoding.ASCII);
            var salt = Guid.NewGuid().ToString().GetBytes(Encoding.ASCII);
            var initialVector = "abb6526e130b4a24".GetBytes(Encoding.ASCII);
            var a = await domainEncryptionProvider.Encrypt<CustomerDto, Customer>(new CustomerDto { 
                EmailAddress = "sarah.catlin@hotmail.com",
                FirstName = "Sarah",
                MiddleName = "Middleton",
                LastName = "Catlin",
                DateOfBirth = new DateTime(2019, 09, 11)
                }, Domains.Enumerations.SymmetricAlgorithmType.Aes, key, salt, initialVector, 100000).ConfigureAwait(false);

            var decryptedCustomer = await domainEncryptionProvider.Decrypt<Customer, CustomerDto>(a, Domains.Enumerations.SymmetricAlgorithmType.Aes,
                initialVector).ConfigureAwait(false);
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
        public override ValidationResult Validate(Customer model)
        {
            ValidateModel(model)
                .IsNotNull(member => member.EmailAddress)
                .IsNotNull(member => member.FirstName)
                .IsValid(member => member.DateOfBirth, DateTime.Now.AddYears(-11), (member, compare) => member < compare)
                .IsInRange(member => member.RegistrationDate, DateTime.Now.AddYears(-5), DateTime.Now, 
                    (member, minimumValue, maximimValue) => member >= minimumValue && member <= maximimValue);
            return ValidationResult.Success;
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
