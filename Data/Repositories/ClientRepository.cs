using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;

public class ClientRepository(DataContext context) : BaseRepository<ClientEntity>(context), IClientRepository
{
    public override async Task<IEnumerable<ClientEntity>> GetAllAsync()
    {
        try
        {
            var result = await _dbSet
                .Include(x => x.Address)
                .Include(x => x.Projects)
                .OrderBy(x => x.ClientName)
                .ToListAsync();

            return result ?? [];
        }
        catch
        {
            Debug.WriteLine("GetAllAsync - Error getting entities");
            return [];
        }
    }

    public override async Task<ClientEntity?> GetOneAsync(Expression<Func<ClientEntity, bool>> expression)
    {
        if (expression == null)
            return null;

        try
        {
            // Get the first result from db that matches the expression
            var result = await _dbSet
                .Include(x => x.Address)
                .FirstOrDefaultAsync(expression);
            return result;
        }
        catch
        {
            Debug.WriteLine("GetAsync - Error getting entity");
            return null;
        }
    }
}
