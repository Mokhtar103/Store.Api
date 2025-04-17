using Domain.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T>(IQueryable<T> baseQuery, Specification<T> specifications) where T : class
        {
            var query = baseQuery;

            if(specifications.Criteria is not null)
                query = query.Where(specifications.Criteria);

            query = specifications.Includes.Aggregate(query, (currentQuery, include) => currentQuery.Include(include));

            return query;
        }
    }
}
