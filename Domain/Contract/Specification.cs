using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contract
{
    public abstract class Specification<T> where T : class
    {
        protected Specification(Expression<Func<T, bool>> criteria)
        { 
            Criteria = criteria;
        }

        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } = new();

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Take { get; private set; }

        public bool isPaginated { get; private set; }


        protected void AddInclude(Expression<Func<T, object>> expression)
            => Includes.Add(expression);

        protected void SetOrderBy(Expression<Func<T, object>> orderBy)
            => OrderBy = orderBy;

        protected void SetOrderByDescending(Expression<Func<T, object>> orderByDescending)
            => OrderByDescending = orderByDescending;

        protected void ApplyPagination(int pageIndex, int pageSize)
        {
            isPaginated = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
        }
    }
}
