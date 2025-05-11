using Store.Es.Core;
using Store.Es.Core.Entities;
using Store.Es.Core.Repositories.Contract;
using Store.Es.Repository.Data.Contexts;
using Store.Es.Repository.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _context;
        private Hashtable _repositories;
        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        {
          return await _context.SaveChangesAsync();
        }

        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            // this will create object each time I want to create it once
            //var repository = new GenericRepository<TEntity, TKey>(_context);
            //return repository;
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity, TKey>(_context);
                _repositories.Add(type, repository);
            }
            return _repositories[type] as IGenericRepository<TEntity, TKey>;
        }
    }
}
