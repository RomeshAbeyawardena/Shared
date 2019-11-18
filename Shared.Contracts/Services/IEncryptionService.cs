using Shared.Domains;
using System.Text;

namespace Shared.Contracts
{
    public interface IEncryptionService
    {
        byte[] GenerateBytes(string key, Encoding encoding = null);
        byte[] EncryptString(SymmetricAlgorithmType symmetricAlgorithmType, string plainText, byte[] key, byte[] iV);
        string DecryptBytes(SymmetricAlgorithmType symmetricAlgorithmType,byte[] bytes, byte[] key, byte[] iV);
    }
}
