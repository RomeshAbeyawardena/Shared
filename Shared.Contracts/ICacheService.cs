using System.Threading.Tasks;

namespace Shared.Contracts
{
    public interface ICacheService
    {
        Task<T> Get<T>(string cacheKeyName) where T : class;
        Task<T> Set<T>(string cacheKeyName, T value) where T : class;
    }
}
