using Business.Dtos;
using Business.Models;
using Data.Entities;
using System.Linq.Expressions;

namespace Business.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectModel> CreateProjectAsync(AddProjectFormModel form);
        Task<bool> Delete(Guid id);
        Task<IEnumerable<ProjectModel>> GetAllProjectsAsync();
        Task<ProjectModel> GetProjectAsync(Expression<Func<ProjectEntity, bool>> expression);
        Task<IEnumerable<ProjectModel>> GetProjectsAsync(Expression<Func<ProjectEntity, bool>> expression);
    }
}