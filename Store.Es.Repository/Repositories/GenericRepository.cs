using Microsoft.EntityFrameworkCore;
using Store.Es.Core.Entities;
using Store.Es.Core.Repositories.Contract;
using Store.Es.Core.Specifications;
using Store.Es.Repository.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Repository.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _context;


        public GenericRepository(StoreDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
             _context.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if(typeof(TEntity) == typeof(Product))
            {
                return await _context.Products.Include(p => p.Brand).Include(p => p.Type).ToListAsync() as IEnumerable<TEntity>;
            }
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecifications<TEntity, TKey> spec)
        {
            return await ApplySpecifications(spec).ToListAsync();    
        }
        public async Task<TEntity> GetWithSpecAsync(ISpecifications<TEntity, TKey> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }
        public async Task<int> GetCountAsync(ISpecifications<TEntity, TKey> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }
        private IQueryable<TEntity> ApplySpecifications(ISpecifications<TEntity, TKey> spec)
        {
           return SpecificationsEvaluator<TEntity, TKey>.GetQuery(_context.Set<TEntity>(), spec);
        }
        public async Task<TEntity> GetAsync(TKey id)
        {
            if(typeof(TEntity) == typeof(Product))
            {
                return await _context.Products.
                    Include(p => p.Brand).
                    Include(p => p.Type).
                    FirstOrDefaultAsync(p => p.Id == id as int?) as TEntity; 
            }
            return await _context.Set<TEntity>().FindAsync(id);
        }



        public void Update(TEntity entity)
        {
            _context.Update(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _context.RemoveRange(entities);
        }
    }
}
