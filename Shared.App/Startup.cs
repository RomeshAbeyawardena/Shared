using Humanizer;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shared.Library.Extensions;
using System.IO;
using System.Linq;

namespace Shared.App
{
    public class Startup
    {
        private readonly ICryptographicProvider cryptographicProvider;
        private readonly IEncryptionService encryptionService;

        public async Task Start()
        {
            var customerList = new List<Customer>();
            var initialList = new List<Initial>();

            initialList.Add(new Initial
            {
                Id = 1,
                Created = new DateTimeOffset(2019, 11, 08, 10, 00, 00, TimeSpan.FromHours(0)),
                Modified = new DateTimeOffset(2019, 11, 08, 10, 30, 00, TimeSpan.FromHours(0)),
                Title = "MR",
                LongTitle = string.Empty
            });

            initialList.Add(new Initial
            {
                Id = 2,
                Created = new DateTimeOffset(2019, 11, 08, 10, 00, 00, TimeSpan.FromHours(0)),
                Modified = new DateTimeOffset(2019, 11, 08, 10, 30, 00, TimeSpan.FromHours(0)),
                Title = "Mrs",
                LongTitle = string.Empty
            });

            var initialExpression = ExpressionBuilder.Create().And("Id", value: 1);
            var initialExpression2 = ExpressionBuilder.Create().And("Id", value: 2);
            customerList.Add(new Customer
            {
                Id = 1,
                Initial = initialList.AsQueryable().SingleOrDefault(initialExpression.ToExpression<Initial>()),
                InitialId = 1,
                Created = new DateTimeOffset(2019, 11, 08, 10, 00, 00, TimeSpan.FromHours(0)),
                Modified = new DateTimeOffset(2019, 11, 08, 10, 30, 00, TimeSpan.FromHours(0)),
                FirstName = "John",
                MiddleName = "Maccy",
                LastName = "Doe"
            });

            customerList.Add(new Customer
            {
                Id = 2,
                InitialId = 2,
                Initial = initialList.AsQueryable().SingleOrDefault(initialExpression2.ToExpression<Initial>()),
                Created = new DateTimeOffset(2019, 11, 08, 10, 00, 00, TimeSpan.FromHours(0)),
                Modified = new DateTimeOffset(2019, 11, 08, 10, 30, 00, TimeSpan.FromHours(0)),
                FirstName = "Jane",
                MiddleName = "Lindsey",
                LastName = "Doe",
            });

            initialExpression = ExpressionBuilder.Create()
                .And("FirstName", value: "John")
                .And("InitialId", value: 1)
                .Or("LastName", value: "Doe");

            initialExpression2 = ExpressionBuilder.Create()
                .And("FirstName",  value: "Jane")
                .And("InitialId",  value: 2)
                .Or("LastName", value: "Doe");

            var customer = customerList.AsQueryable().Where(initialExpression2.ToExpression<Customer>());
            var customer2 = customerList.AsQueryable().Where(initialExpression2.ToExpression<Customer>());
        }

        public async Task<CryptoData> GetCryptoDataFromUserInput()
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

            var generatedKey = cryptographicProvider.GenerateKey(password, memorialWord, 10000, 32);

            var generatedIV = cryptographicProvider.GenerateKey(password, memorialWord, 10000, 16);
            return new CryptoData(generatedIV, generatedKey);
        }

        public Startup(ICryptographicProvider cryptographicProvider, IEncryptionService encryptionService)
        {
            this.cryptographicProvider = cryptographicProvider;
            this.encryptionService = encryptionService;
        }
    }

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

    public struct SecureReadInfo
    {
        public int Length {get; set; }
        public Memory<byte> Value {get ;set;}
        public bool Escaped {get;set;}
    }

    public class MyServiceBroker : DefaultServiceBroker
    {
        public override Assembly[] GetAssemblies => new [] { Assembly.GetAssembly(typeof(DefaultServiceRegistration)) };
    }
}
