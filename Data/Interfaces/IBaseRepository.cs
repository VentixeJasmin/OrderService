using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task<IEnumerable<TEntity>> GetAsync();
    Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression);
    Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> expression, TEntity updatedEntity);
    Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression);
    Task<bool> AlreadyExistsAsync(Expression<Func<TEntity, bool>> expression);
    Task<int> SaveAsync();
}
