using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Store.Es.Core.Entities;
using Store.Es.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Es.Repository
{
    public class SpecificationsEvaluator<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity, TKey> spec)
        {
            var query = inputQuery;
            if(spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }
            if(spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if(spec.OrderByDesc  is not null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }
            if(spec.IsPaginationEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            //foreach(var s in spec.Includes)
            //{
            //    query.Include(s);
            //}
            return query;
        }
    }
}
