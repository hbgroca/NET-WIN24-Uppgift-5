using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;

public class MemberRepository(DataContext context) : BaseRepository<MemberEntity>(context), IMemberRepository
{
    // Fake method
    public void FakeMethod()
    {
        // Do nothing
    }

    public override async Task<IEnumerable<MemberEntity>> GetAllAsync()
    {
        try
        {
            // Get all values and return as list
            var result = await _dbSet.Include(x => x.Address).ToListAsync();
            return result ?? [];
        }
        catch
        {
            Debug.WriteLine("GetAllAsync - Error getting entities");
            return [];
        }
    }

    public override async Task<MemberEntity?> GetOneAsync(Expression<Func<MemberEntity, bool>> expression)
    {
        if (expression == null)
            return null;

        try
        {
            // Get the first result from db that matches the expression
            var result = await _dbSet.Include(x => x.Address).FirstOrDefaultAsync(expression);
            return result;
        }
        catch
        {
            Debug.WriteLine("GetAsync - Error getting entity");
            return null;
        }
    }
}