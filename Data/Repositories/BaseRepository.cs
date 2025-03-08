using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;

public abstract class BaseRepository<TEntity>(DataContext context) : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly DataContext _context = context;
    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    private IDbContextTransaction _transaction = null!;

    #region Transaction Management
    public virtual async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public virtual async Task CommitTransactionAsync()
    {
        if (_transaction == null)
            return;

        await _transaction.CommitAsync();
        _transaction.Dispose();
    }

    public virtual async Task RollbackTransactionAsync()
    {
        if (_transaction == null)
            return;

        await _transaction.RollbackAsync();
        _transaction.Dispose();
    }

    #endregion

    public virtual async Task<int> SaveAsync()
    {
        try
        {
            // Return the number of changes made to the database
            var result = await _context.SaveChangesAsync();
            return result;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"CreateAsync - Error saving entity: {ex}");
            return -1;
        }
    }


    // CREATE
    public virtual async Task<bool> CreateAsync(TEntity entity)
    {
        if (entity == null)
            return false;
        try
        {
            // Add entity to staging memory
            await _dbSet.AddAsync(entity);
            return true;
        }
        catch
        {
            Debug.WriteLine("CreateAsync - Error creating entity");
            return false;
        }
    }

    // READ
    public virtual async Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> expression)
    {
        if (expression == null)
            return null;

        try
        {
            // Get the first result from db that matches the expression
            var result = await _dbSet.FirstOrDefaultAsync(expression);
            return result;
        }
        catch
        {
            Debug.WriteLine("GetAsync - Error getting entity");
            return null;
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        try
        {
            // Get all values and return as list
            var result = await _dbSet.ToListAsync();
            return result ?? [];
        }
        catch
        {
            Debug.WriteLine("GetAllAsync - Error getting entities");
            return [];
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression)
    {
        try
        {
            // Get all values that matches the expression
            if (expression == null)
                return null!;

            var result = await _dbSet.Where(expression).ToListAsync();
            return result ?? [];
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Update - Error getting entity: {ex.Message}");
            return [];
        }
    }

    public virtual async Task<bool> ExistInDb(Expression<Func<TEntity, bool>> expression)
    {
        try
        {
            var result = await _dbSet.FirstOrDefaultAsync(expression);
            if (result != null)
                return true;

            return false;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ExistInDb - Error checking if entity exists: {ex.Message}");
            return false;
        }
    }


    // UPDATE
    public virtual bool Update(TEntity entity)
    {
        try
        {
            // Update entity in staging memory
            _dbSet.Update(entity);
            return true;
        }
        catch
        {
            Debug.WriteLine("Update - Error updating entity");
            return false;
        }
    }


    // DELETE
    public virtual bool Delete(TEntity entity)
    {
        try
        {
            // Remove entity from staging memory
            _dbSet.Remove(entity);
            return true;
        }
        catch
        {
            Debug.WriteLine("Delete - Error deleting entity");
            return false;
        }
    }
}