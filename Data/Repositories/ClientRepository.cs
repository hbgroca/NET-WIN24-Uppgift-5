using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Data.Repositories;

public class ClientRepository(DataContext context) : BaseRepository<ClientEntity>(context), IClientRepository
{
    // Fake method
    public void FakeMethod()
    {
        // Do nothing
    }

    public override async Task<IEnumerable<ClientEntity>> GetAllAsync()
    {
        try
        {
            // Get all values and return as list
            var result = await _dbSet
                .Include(x => x.Address)
                .Include(x => x.Projects)
                .ToListAsync();

            result.Sort((x, y) => x.ClientName.CompareTo(y.ClientName));
            return result ?? [];
        }
        catch
        {
            Debug.WriteLine("GetAllAsync - Error getting entities");
            return [];
        }
    }
}
