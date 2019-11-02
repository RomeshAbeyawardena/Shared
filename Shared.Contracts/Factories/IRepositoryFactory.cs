using Microsoft.EntityFrameworkCore;

namespace Shared.Contracts
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> GetRepository<TDbContext, TEntity>() 
            where TDbContext: DbContext
            where TEntity: class;
    }
}
