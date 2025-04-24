using System.Linq.Expressions;

namespace Data.Interfaces;
public interface IBaseRepository<TEntity> where TEntity : class
{
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task<bool> CreateAsync(TEntity entity);
    bool Delete(TEntity entity);
    Task<bool> ExistInDb(Expression<Func<TEntity, bool>> expression);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression);
    int GetCount();
    Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> expression);
    Task RollbackTransactionAsync();
    Task<int> SaveAsync();
    bool Update(TEntity entity);
}