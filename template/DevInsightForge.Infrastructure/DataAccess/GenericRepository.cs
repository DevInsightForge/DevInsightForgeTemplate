using DevInsightForge.Application.Interfaces.DataAccess;
using DevInsightForge.Domain.Entities.Common;
using DevInsightForge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DevInsightForge.Infrastructure.DataAccess;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(DatabaseContext dbContext)
    {
        if (dbContext is null) throw new ArgumentNullException(nameof(dbContext));
        _dbSet = dbContext.Set<TEntity>();
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Task.Run(() => _dbSet.Update(entity), cancellationToken);
    }

    public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public virtual async Task<TEntity?> GetByIdAsync(dynamic id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public virtual async Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> where)
    {
        return await _dbSet.AsNoTracking().MaxAsync(where);
    }

    public virtual async Task<TEntity?> GetWhereAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] include)
    {
        var query = _dbSet.AsNoTracking().AsQueryable();

        if (include != null)
        {
            query = include.Aggregate(query, (current, expression) => current.Include(expression));
            var sqlquery = query.ToQueryString();
        }

        return await query.FirstOrDefaultAsync(where);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().AnyAsync(predicate);
    }

    public virtual async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
    }
}
