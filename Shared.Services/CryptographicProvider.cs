using Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Shared.Services
{
    public class CryptographicProvider : ICryptographicProvider
    {
        public byte[] GenerateKey(IEnumerable<byte> salt, IEnumerable<byte> password, int iterations, int cb, HashAlgorithmName? hashAlgorithmName = null)
        {
            using (var pdb = new Rfc2898DeriveBytes (password.ToArray(), salt.ToArray(), iterations, hashAlgorithmName ?? HashAlgorithmName.SHA512))
            {
                return pdb.GetBytes(cb);
            }
        }
    }
}
