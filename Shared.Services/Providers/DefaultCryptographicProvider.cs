using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Shared.Services
{
    public class DefaultCryptographicProvider : ICryptographicProvider
    {
        public byte[] GenerateKey(IEnumerable<byte> salt, IEnumerable<byte> password, int iterations, int cb, HashAlgorithmName? hashAlgorithmName = null)
        {
            using (var pdb = new Rfc2898DeriveBytes (password.ToArray(), salt.ToArray(), iterations, hashAlgorithmName ?? HashAlgorithmName.SHA512))
            {
                return pdb.GetBytes(cb);
            }
        }

        public byte[] ComputeHash(byte[] raw, string algName)
        {
           using var hMac = HashAlgorithm.Create(algName);
           return hMac.ComputeHash(raw);
        }

        public bool IsHashValid(byte[] hash, byte[] compareHash)
        {
            return hash.SequenceEqual(compareHash);
        }
    }
}
