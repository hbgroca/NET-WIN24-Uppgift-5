using Business.Dtos;
using Business.Factories;
using Business.Helpers;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using System.Diagnostics;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, IAddressRepository addressRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IAddressRepository _addressRepository = addressRepository;

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

        // Read

        // Update

        // Delete
    }
}
