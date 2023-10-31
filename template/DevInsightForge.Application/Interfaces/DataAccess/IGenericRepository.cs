using DevInsightForge.Domain.Entities.Common;
using System.Linq.Expressions;

namespace DevInsightForge.Application.Interfaces.DataAccess;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(dynamic id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> where);
    Task<TEntity?> GetWhereAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] include);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);
}
