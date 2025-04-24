using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, IMemberRepository memberRepository, IClientRepository clientRepository, IImageServices imageServices, INotificationSerivces notificationSerivces) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IMemberRepository _memberRespository = memberRepository;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IImageServices _imageService = imageServices;
    private readonly INotificationSerivces _notificationServices = notificationSerivces;

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

            // Handle image if present
            if (form.ProjectImage != null && form.ProjectImage.Length > 0)
            {
                // Save image
                form.ImageName = await _imageService.Create(form.ProjectImage, "projects");
            }
            else
                form.ImageName = $"/images/defaultproject.png";

            // Remap with factory
            var projectEntity = ProjectFactory.Create(form);

            // Set Id
            projectEntity.Id = Guid.NewGuid();

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
                _imageService.Delete(form.ImageName!);
                return null!;
            }
                
            // Commit the transaction
            await _projectRepository.CommitTransactionAsync();

            // Send notifcations to other team members
            if (result > 0)
            {
                string Message = $"Project added: {projectEntity.ProjectName}!";
                await _notificationServices.AddNotificationAsync(2, Message, projectEntity.Id.ToString(), projectEntity.ImageUrl!, 1);
            }

            // Return the client
            return ProjectFactory.Create(projectEntity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            _imageService.Delete(form.ImageName!);
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

    public int GetProjectCount()
    {
        return _projectRepository.GetCount();
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

            // Handle image if present
            if (form.ProjectImage != null && form.ProjectImage.Length > 0)
            {
                // Save image
                form.ImageName = await _imageService.Update(form.ProjectImage, "projects", entity.ImageUrl!);
            }

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

            string Message = $"Project: {updatedEntity.ProjectName} was updated";
            await _notificationServices.AddNotificationAsync(2, Message, updatedEntity.Id.ToString(), updatedEntity.ImageUrl!, 1);
   

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"! Failed to update project: {ex.Message}");
            await _projectRepository.RollbackTransactionAsync();
            _imageService.Delete(form.ImageName!);
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
            if (save < 1)
            {
                await _projectRepository.RollbackTransactionAsync();
                return false;
            }
                

            // Commit transaction
            await _projectRepository.CommitTransactionAsync();

            // Remove image
            _imageService.Delete(entity.ImageUrl!);

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
