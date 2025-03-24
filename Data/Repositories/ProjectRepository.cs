using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;

public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity>(context), IProjectRepository
{
    public override async Task<IEnumerable<ProjectEntity>> GetAllAsync()
    {
        try
        {
            // Get all values and return as list
            var result = await _dbSet
                .Include(x => x.Client)
                    .ThenInclude(x => x.Address)
                .Include(m => m.Members)
                    .ThenInclude(a => a.Address)
                .ToListAsync();
            return result ?? [];
        }
        catch
        {
            Debug.WriteLine("GetAllAsync - Error getting entities");
            return [];
        }
    }

    public override async Task<ProjectEntity?> GetOneAsync(Expression<Func<ProjectEntity, bool>> expression)
    {
        try
        {
            var result = await _dbSet
                .Include(x => x.Client)
                    .ThenInclude(x => x.Address)
                .Include(m => m.Members)
                    .ThenInclude(a => a.Address)
                .FirstOrDefaultAsync(expression);
            if(result == null) {
                Debug.WriteLine("GetOneAsync - No entity found");
                return null!;
            }
            return result;
        }
        catch
        {
            Debug.WriteLine("GetAsync - Error getting entity");
            return null!;
        }
    }
}
