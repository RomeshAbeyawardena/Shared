using Shared.Domains;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public interface IEncryptionService
    {
        byte[] GenerateKey(SymmetricAlgorithmType symmetricAlgorithmType);
        byte[] GenerateIv(SymmetricAlgorithmType symmetricAlgorithmType);
        byte[] GenerateBytes(string key, Encoding encoding = null);
        Task<byte[]> EncryptString(SymmetricAlgorithmType symmetricAlgorithmType, string plainText, byte[] key, byte[] iV);
        Task<string> DecryptBytes(SymmetricAlgorithmType symmetricAlgorithmType,byte[] bytes, byte[] key, byte[] iV);
    }
}
