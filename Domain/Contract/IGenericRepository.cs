using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contract
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        Task<TEntity?> GetAsync(TKey id);

        Task<TEntity?> GetAsync(Specification<TEntity> specifications);


        Task<IEnumerable<TEntity>> GetAllAsync(bool isTrackable = false);

        Task<IEnumerable<TEntity>> GetAllAsync(Specification<TEntity> specifications);

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
