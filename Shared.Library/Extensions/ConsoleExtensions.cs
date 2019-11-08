using Shared.Domains;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Shared.Library.Extensions
{
    public static class ConsoleExtensions
    {
        public static async Task<SecureReadInfo> SecureRead(char secureChar)
        {
            using var memoryStream = new MemoryStream();
            using var streamWriter = new StreamWriter(memoryStream);
            var currentKeyInfo = Console.ReadKey(true);
            var secureInfoLength = 0;
            while(currentKeyInfo.Key != ConsoleKey.Enter)
            {
                if(currentKeyInfo.Key == ConsoleKey.Escape)
                    return new SecureReadInfo(true);
                                                   
                Console.Write(secureChar);
                secureInfoLength++;
                await streamWriter.WriteAsync(currentKeyInfo.KeyChar);
                currentKeyInfo = Console.ReadKey(true);
            }
            await streamWriter.FlushAsync();
            Console.WriteLine();
            
            return new SecureReadInfo(memoryStream.ToArray(), secureInfoLength);
        }
    }
}
