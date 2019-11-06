using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Contracts
{
    public interface ICryptographicProvider
    {
        byte[] GenerateKey(IEnumerable<byte> salt, IEnumerable<byte> plainText, int iterations, int keyLength, HashAlgorithmName? hashAlgorithm = null);
    }
}
