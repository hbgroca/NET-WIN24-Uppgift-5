using Data.Entities;
using System.Linq.Expressions;

namespace Data.Interfaces;

public interface IProjectRepository : IBaseRepository<ProjectEntity>
{
    Task<ProjectEntity?> GetOneAsync(Expression<Func<ProjectEntity, bool>> expression);
}