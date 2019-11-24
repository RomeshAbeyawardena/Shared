using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Contracts.Providers
{
    public interface ICryptographicProvider
    {
        bool IsHashValid(byte[] hash, byte[] compareHash);
        byte[] ComputeHash(byte[] raw, string algName);
        byte[] GenerateKey(IEnumerable<byte> salt, IEnumerable<byte> plainText, int iterations, int keyLength, HashAlgorithmName? hashAlgorithm = null);
        IEnumerable<byte[]> ExtractDigestValues(Encoding encoding, string base64value, char separator); 
    }
}
