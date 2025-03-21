using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
}
