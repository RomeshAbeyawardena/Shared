using Shared.Contracts;
using Shared.Contracts.Factories;
using Shared.Domains;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly IMemoryStreamManager memoryStreamManager;
        private readonly ISymmetricAlgorithmFactory symmetricAlgorithmFactory;

        public byte[] GenerateBytes(string key, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.ASCII;

            return encoding.GetBytes(key);
        }

        public byte[] EncryptString(SymmetricAlgorithmType symmetricAlgorithmType, string plainText, byte[] key, byte[] iV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));
            if (key == null || key.Length == 0)
                throw new ArgumentNullException(nameof(key));
            if (iV == null || iV.Length == 0)
                throw new ArgumentNullException(nameof(iV));

            byte[] encrypted;

            using (var aesAlg = GetSymmetricAlgorithm(symmetricAlgorithmType))
            {
                aesAlg.Key = key;
                aesAlg.IV = iV;

                // Create an encryptor to perform the stream transform.
                using var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = memoryStreamManager.GetStream(false))
                {
                    using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }

            return encrypted;
        }

        public string DecryptBytes(SymmetricAlgorithmType symmetricAlgorithmType, byte[] bytes, byte[] key, byte[] iV)
        {
            if (bytes == null || bytes.Length <= 0)
                throw new ArgumentNullException(nameof(bytes));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iV == null || iV.Length <= 0)
                throw new ArgumentNullException(nameof(key));

            string plaintext = null;

            // Create an AesCryptoServiceProvider object
            // with the specified key and IV.
            using (var aesAlg = GetSymmetricAlgorithm(symmetricAlgorithmType))
            {
                aesAlg.Key = key;
                aesAlg.IV = iV;

                // Create a decryptor to perform the stream transform.
                using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                // Create the streams used for decryption.
                using (var msDecrypt = memoryStreamManager.GetStream(bytes, false))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }

        private SymmetricAlgorithm GetSymmetricAlgorithm(SymmetricAlgorithmType symmetricAlgorithmType)
        {
            return symmetricAlgorithmFactory.GetSymmetricAlgorithm(symmetricAlgorithmType);
        }

        public EncryptionService(IMemoryStreamManager memoryStreamManager, ISymmetricAlgorithmFactory symmetricAlgorithmFactory)
        {
            this.memoryStreamManager = memoryStreamManager;
            this.symmetricAlgorithmFactory = symmetricAlgorithmFactory;
        }
    }
}
