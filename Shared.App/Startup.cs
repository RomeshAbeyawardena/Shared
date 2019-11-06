using Humanizer;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;
using Shared.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shared.Library.Extensions;
using System.IO;

namespace Shared.App
{
    public class Startup
    {
        private readonly ICryptographicProvider cryptographicProvider;
        private readonly IEncryptionService encryptionService;

        public async Task Start()
        {
            var cryptoData = await GetCryptoDataFromUserInput();
            Console.Write("OK\r\nMessage: ");
            var encrypted = encryptionService.EncryptString(Console.ReadLine(), cryptoData.Key, cryptoData.Iv);
            
            Console.WriteLine(BitConverter.ToString(encrypted));

            cryptoData = await GetCryptoDataFromUserInput();

            var decrypted = encryptionService.DecryptBytes(encrypted, cryptoData.Key, cryptoData.Iv);
            Console.WriteLine("Decrypted Message: {0}", decrypted);
            Console.ReadKey();
        }

        public async Task<CryptoData> GetCryptoDataFromUserInput()
        {
            RequestPassword:
            Console.Write("Password: ");
            var securePassword = await SecureRead('*');
            
            if(securePassword.Escaped || securePassword.Length < 7)
                goto RequestPassword;

            var password = securePassword.Value.ToArray();

            RequestMemorialWord:
            Console.Write("OK\r\nMemorable word: ");
            var secureMemorialWord = await SecureRead('*');

            if(secureMemorialWord.Escaped || secureMemorialWord.Length < 6)
                goto RequestMemorialWord;

            var memorialWord = secureMemorialWord.Value.ToArray();

            var generatedKey = cryptographicProvider.GenerateKey(password, memorialWord, 10000, 32);

            var generatedIV = cryptographicProvider.GenerateKey(password, memorialWord, 10000, 16);
            return new CryptoData(generatedIV, generatedKey);
        }

        public static async Task<SecureReadInfo> SecureRead(char secureChar)
        {
            var secureInfo = new SecureReadInfo();
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);
            var currentKeyInfo = Console.ReadKey(true);
            secureInfo.Length = 0;
            while(currentKeyInfo.Key != ConsoleKey.Enter)
            {
                if(currentKeyInfo.Key == ConsoleKey.Escape)
                    break;
                                                   
                Console.Write(secureChar);
                secureInfo.Length++;
                await streamWriter.WriteAsync(currentKeyInfo.KeyChar);
                currentKeyInfo = Console.ReadKey(true);
            }
            await streamWriter.FlushAsync();
            Console.WriteLine();
            secureInfo.Value = memoryStream.ToArray();
            return secureInfo;
        }

        public Startup(ICryptographicProvider cryptographicProvider, IEncryptionService encryptionService)
        {
            this.cryptographicProvider = cryptographicProvider;
            this.encryptionService = encryptionService;
        }
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
