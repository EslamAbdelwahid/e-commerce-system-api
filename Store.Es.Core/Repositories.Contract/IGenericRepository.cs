using Store.Es.Core.Entities;
using Store.Es.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Core.Repositories.Contract
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> spec);
        Task<TEntity> GetWithSpecAsync(ISpecifications<TEntity, TKey> spec);

        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetAsync(TKey id);
        Task<int> GetCountAsync(ISpecifications<TEntity, TKey> spec);
        Task AddAsync(TEntity entity);

        void Update(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
    }
}
