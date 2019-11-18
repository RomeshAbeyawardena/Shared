using Shared.Contracts;
using Shared.Services;
using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shared.Library.Extensions;
using Shared.Contracts.Factories;
using Shared.Domains;
using MessagePack;

namespace Shared.App
{
    public class Startup
    {
        private readonly ISerializerFactory serializerFactory;
        private readonly ICryptographicProvider cryptographicProvider;
        private readonly IEncryptionService encryptionService;

        public async Task Start()
        {
            var symmetricAlgorithmType = SymmetricAlgorithmType.Aes;
            var emailAddress = Console.ReadLine();
            var gData = await GetCryptoDataFromUserInput(symmetricAlgorithmType);
            var encrypted = await encryptionService.EncryptString(symmetricAlgorithmType, emailAddress, gData.Key, gData.Iv);
            var binarySerializer = serializerFactory.GetSerializer(SerializerType.MessagePack);
            var serialized = binarySerializer.Serialize(new Customer {
                    Id = 1,
                    InitialId = 1,
                    FirstName = "Krishna",
                    MiddleName = "Julia",
                    LastName = "Ellis",
                    Created = DateTimeOffset.Now,
                    Modified = DateTimeOffset.Now
                });

            var decryptedEmail = await encryptionService.DecryptBytes(symmetricAlgorithmType, encrypted, gData.Key, gData.Iv);

            Console.WriteLine(decryptedEmail);

            var deserialized = binarySerializer.Deserialize<Customer>(serialized);
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

        public Startup(ISerializerFactory serializerFactory, ICryptographicProvider cryptographicProvider, IEncryptionService encryptionService)
        {
            this.serializerFactory = serializerFactory;
            this.cryptographicProvider = cryptographicProvider;
            this.encryptionService = encryptionService;
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
        public override Assembly[] GetAssemblies => new [] { Assembly.GetAssembly(typeof(DefaultServiceRegistration)) };
    }
}
