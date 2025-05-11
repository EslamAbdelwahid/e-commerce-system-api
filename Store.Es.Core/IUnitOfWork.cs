using Store.Es.Core.Entities;
using Store.Es.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();

        IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
    }
}
