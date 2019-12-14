using Shared.Contracts;
using Shared.Contracts.Providers;
using Shared.Domains.Enumerations;
using Shared.Library.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Services
{
    public class DefaultCryptographicProvider : ICryptographicProvider
    {
        public byte[] GenerateKey(ICryptographicInfo cryptographicInfo,
            int cb)
        {
            if(cryptographicInfo == null)
                throw new ArgumentNullException(nameof(cryptographicInfo));

            using (var pdb = new Rfc2898DeriveBytes(cryptographicInfo.Key.ToArray(), 
                cryptographicInfo.Salt.ToArray(),  
                cryptographicInfo.Iterations))
            {
                return pdb.GetBytes(cb);
            }
        }

        public byte[] ComputeHash(byte[] raw, string algName)
        {
           using (var hMac = HashAlgorithm.Create(algName))
            return hMac.ComputeHash(raw);
        }

        public bool IsHashValid(byte[] hash, byte[] compareHash)
        {
            return hash.SequenceEqual(compareHash);
        }

        public IEnumerable<byte[]> ExtractDigestValues(Encoding encoding, string base64value, char separator)
        {
            var list = new List<byte[]>();
            var decipheredValue = Convert.FromBase64String(base64value);
            if (decipheredValue.Length < 1)
                throw new ArgumentException(nameof(base64value));

            var rawValue = decipheredValue.GetString(encoding);

            if(string.IsNullOrEmpty(rawValue))
                return list.ToArray();

            var values = rawValue.Split(separator);

            list.AddRange(values
                .Select(value => value.GetBytes(encoding)));

            return list.ToArray();
        }
    }
}
