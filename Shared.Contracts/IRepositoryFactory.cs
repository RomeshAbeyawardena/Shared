using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Contracts
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> GetRepository<TDbContext, TEntity>() 
            where TDbContext: DbContext
            where TEntity: class;
    }
}
