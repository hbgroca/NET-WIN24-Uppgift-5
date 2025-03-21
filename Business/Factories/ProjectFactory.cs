using Business.Dtos;
using Business.Models;
using Data.Entities;

namespace Business.Factories;

public class ProjectFactory
{
    public static ProjectEntity Create(ProjectRegistrationform form)
    {
        if (form == null)
            return null!;
        return new ProjectEntity
        {
            ImageUrl = form.ImageUrl,
            ProjectName = form.ProjectName,
            Description = form.Description,
            Budget = form.Budget,
            ClientId = form.Client.Id,
            Members = form.Members.Select(MemberFactory.Create).ToList(),
            StartDate = form.StartDate,
            EndDate = form.EndDate,
        };
    }

    public static ProjectModel Create(ProjectEntity entity)
    {
        if (entity == null)
            return null!;
        return new ProjectModel
        {
            Id = entity.Id,
            ImageUrl = entity.ImageUrl,
            ProjectName = entity.ProjectName,
            Description = entity.Description,
            Budget = entity.Budget,
            Client = ClientFactory.Create(entity.Client),
            Members = entity.Members.Select(MemberFactory.Create).ToList(),
            StartDate = entity.StartDate,
            IsCompleted = entity.IsCompleted,
            EndDate = entity.EndDate,
            CreateDate = entity.CreateDate,
            UpdateDate = entity.UpdateDate,
        };
    }

    public static ProjectEntity Create(ProjectModel model)
    {
        if (model == null)
            return null!;
        return new ProjectEntity
        {
            Id = model.Id,
            ImageUrl = model.ImageUrl,
            ProjectName = model.ProjectName,
            Description = model.Description,
            Budget = model.Budget,
            ClientId = model.Client.Id,
            Client = ClientFactory.Create(model.Client),
            Members = model.Members.Select(MemberFactory.Create).ToList(),
            StartDate = model.StartDate,
            IsCompleted = model.IsCompleted,
            EndDate = model.EndDate,
            CreateDate = model.CreateDate,
            UpdateDate = model.UpdateDate,
        };
    }
}
