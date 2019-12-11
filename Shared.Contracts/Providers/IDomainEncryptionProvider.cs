using Shared.Domains.Enumerations;
using System.Threading.Tasks;

namespace Shared.Contracts.Providers
{
    public interface IDomainEncryptionProvider
    {
        Task<TDest> Decrypt<TSource, TDest>(TSource value, ICryptographicInfo cryptographicInfo);
        Task<TDest> Encrypt<TSource, TDest>(TSource value, ICryptographicInfo cryptographicInfo);
    }
}