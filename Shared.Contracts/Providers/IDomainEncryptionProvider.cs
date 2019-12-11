using Shared.Domains.Enumerations;
using System.Threading.Tasks;

namespace Shared.Contracts.Providers
{
    public interface IDomainEncryptionProvider
    {
        Task<TDest> Decrypt<TSource, TDest>(TSource value, SymmetricAlgorithmType symmetricAlgorithmType, byte[] initialVector);
        Task<TDest> Encrypt<TSource, TDest>(TSource value, SymmetricAlgorithmType symmetricAlgorithmType, byte[] key, byte[] salt, byte[] initialVector, int iterations);
    }
}