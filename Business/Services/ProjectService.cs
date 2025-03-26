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

public class ProjectService(IProjectRepository projectRepository, IMemberRepository memberRepository, IClientRepository clientRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IMemberRepository _memberRespository = memberRepository;
    private readonly IClientRepository _clientRepository = clientRepository;

    // Create
    public async Task<ProjectModel> CreateProjectAsync(AddProjectFormModel form)
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

            // Set Id
            projectEntity.Id = GenerateGuid.NewGuid();

            // Add Members
            if(form.Members.Count() == 0)
            {
                Debug.WriteLine("Error while creating project entity, members missing");
                return null!;
            }
            foreach(MemberModel member in form.Members)
            {
                var entity = await _memberRespository.GetOneAsync(x => x.Id == member.Id.ToString());
                if(entity != null)
                {
                    projectEntity.Members.Add(entity);
                }
            }

            // Add Client
            projectEntity.Client = await _clientRepository.GetOneAsync(x => x.Id == form.ClientId);

            // Set the date created and updated
            projectEntity.CreateDate = DateOnly.FromDateTime(DateTime.Now);
            projectEntity.UpdateDate = DateOnly.FromDateTime(DateTime.Now);

            // Create the project in dbcontext
            await _projectRepository.CreateAsync(projectEntity);

            // Save the changes
            var result = await _projectRepository.SaveAsync();
            if (result == 0)
            {
                Debug.WriteLine("Error while saving project");
                return null!;
            }
                

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
    public async Task<bool> Update(EditProjectFormModel form)
    {
        if(form == null)
        {
            Debug.WriteLine("EditProjectForm missing");
            return false;
        }

        // Begin transaction
        await _projectRepository.BeginTransactionAsync();

        try
        {
            var entity = await _projectRepository.GetOneAsync(p => p.Id == form.Id);
            if (entity == null)
                return false!;

            var updatedEntity = ProjectFactory.Update(entity, form);

            // Add Members
            if (form.Members.Count() == 0)
            {
                Debug.WriteLine("Error while creating project entity, members missing");
                return false;
            }
            // Clear old members
            updatedEntity.Members.Clear();
            foreach (MemberModel member in form.Members)
            {
                var memberEntity = await _memberRespository.GetOneAsync(x => x.Id == member.Id.ToString());
                if (memberEntity != null)
                {
                    updatedEntity.Members.Add(memberEntity);
                }
            }

            // Handle Image
            string? oldImageUrl = entity.ImageUrl;
            if (!string.IsNullOrWhiteSpace(form.ImageName))
                updatedEntity.ImageUrl = form.ImageName;

            // Add Client
            updatedEntity.Client = await _clientRepository.GetOneAsync(x => x.Id == form.ClientId);

            // Set the update date
            updatedEntity.UpdateDate = DateOnly.FromDateTime(DateTime.Now);

            // Update the dbset
            _projectRepository.Update(updatedEntity);

            // Save the changes
            var result = await _projectRepository.SaveAsync();
            if (result == 0)
            {
                Debug.WriteLine("Error while saving project");
                return false;
            }
                
            // Commit the transaction
            await _projectRepository.CommitTransactionAsync();

            // Remove image if updated
            if (form.ProjectImage is not null && form.ImageName is not null)
            {
                var cutString = $"{Environment.CurrentDirectory}\\wwwroot\\uploaded\\projects\\{oldImageUrl?.Substring(19)}";
                if (File.Exists(cutString))
                    Debug.WriteLine($"!!! - Trying to remove file: {cutString}");
                    File.Delete(cutString);
            }

            return true;

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"! Failed to update project: {ex.Message}");
            await _projectRepository.RollbackTransactionAsync();
            return false;
        }
    }


    // Delete
    public async Task<bool> Delete(Guid id)
    {
        // Begin transaction
        await _projectRepository.BeginTransactionAsync();

        try
        {
            // Get the entity
            var entity = await _projectRepository.GetOneAsync(x => x.Id == id);
            if (entity == null)
                return false;

            // Delete from dbset
            _projectRepository.Delete(entity);

            // Save changes
            var save = await _projectRepository.SaveAsync();
            if (save == 0)
                return false;

            // Commit transaction
            await _projectRepository.CommitTransactionAsync();

            // Remove image
            var cutString = $"{Environment.CurrentDirectory}\\wwwroot\\uploaded\\projects\\{entity.ImageUrl?.Substring(19)}";
            if (File.Exists(cutString))
                File.Delete(cutString);

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            // Rollback transaction if error
            await _projectRepository.RollbackTransactionAsync();
            return false;
        }
    }

}
