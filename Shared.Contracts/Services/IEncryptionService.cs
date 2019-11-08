using System.Text;

namespace Shared.Contracts
{
    public interface IEncryptionService
    {
        byte[] GenerateBytes(string key, Encoding encoding = null);
        byte[] EncryptString(string plainText, byte[] key, byte[] iV);
        string DecryptBytes(byte[] bytes, byte[] key, byte[] iV);
    }
}
