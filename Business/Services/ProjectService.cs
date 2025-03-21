using Business.Dtos;
using Business.Factories;
using Business.Helpers;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    // Create
    public async Task<ProjectModel> CreateProjectAsync(ProjectRegistrationform form)
    {
        if (form == null)
        {
            Debug.WriteLine("Registrationform missing.");
            return null!;
        }
        try
        {
            // Begin transaction
            await _projectRepository.BeginTransactionAsync();

            // Remap with factory
            var projectEntity = ProjectFactory.Create(form);
            projectEntity.Id = GenerateGuid.NewGuid();

            // Set the date created and updated
            projectEntity.CreateDate = DateOnly.FromDateTime(DateTime.Now);
            projectEntity.EndDate = DateOnly.FromDateTime(DateTime.Now);

            // Create the client in dbcontext
            await _projectRepository.CreateAsync(projectEntity);

            // Save the changes
            var result = await _projectRepository.SaveAsync();
            if (result == 0)
                throw new Exception("Error while saving the project");

            // Commit the transaction
            await _projectRepository.CommitTransactionAsync();

            // Return the client
            return ProjectFactory.Create(projectEntity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            await _projectRepository.RollbackTransactionAsync();
            return null!;
        }
    }

    // Read
    public async Task<ProjectModel> GetProjectAsync(Expression<Func<ProjectEntity, bool>> expression)
    {
        var project = await _projectRepository.GetOneAsync(expression);
        if (project == null)
            return null!;
        return ProjectFactory.Create(project);
    }

    public async Task<IEnumerable<ProjectModel>> GetProjectsAsync(Expression<Func<ProjectEntity, bool>> expression)
    {
        var project = await _projectRepository.GetAllAsync(expression);
        if (project == null)
            return null!;
        return project.Select(ProjectFactory.Create);
    }

    public async Task<IEnumerable<ProjectModel>> GetAllProjectsAsync()
    {
        var project = await _projectRepository.GetAllAsync();
        if (project == null)
            return null!;

        Debug.WriteLine($"! - Returning {project.Count()}");
        return project.Select(ProjectFactory.Create);
    }

    // Update

    // Delete

}
