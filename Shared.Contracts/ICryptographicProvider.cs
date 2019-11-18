using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Shared.Contracts
{
    public interface ICryptographicProvider
    {
        bool IsHashValid(byte[] hash, byte[] compareHash);
        byte[] ComputeHash(byte[] raw, string algName);
        byte[] GenerateKey(IEnumerable<byte> salt, IEnumerable<byte> plainText, int iterations, int keyLength, HashAlgorithmName? hashAlgorithm = null);
    }
}
